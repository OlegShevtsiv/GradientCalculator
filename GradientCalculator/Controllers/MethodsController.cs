using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GradientCalculator.Middlewares.Filters;
using GradientCalculator.Models.Request;
using GradientCalculator.Models.Response;
using GradientCalculator.Services.ResponseRequestLoggerService;
using GradientCalculator.Services.ResponseRequestLoggerService.Models;
using GradientCalculator.ViewModels;
using GradientMethods;
using GradientMethods.ExceptionResult;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace GradientCalculator.Controllers
{
    [ServiceFilter(typeof(LanguageActionFilter))]
    public class MethodsController : Controller
    {
        private readonly IStringLocalizer<CommonResource> _localizer;

        private readonly LiteDbStorageService _respReqLoggerService;

        public MethodsController(IStringLocalizer<CommonResource> localizer, LiteDbStorageService respReqLoggerService)
        {
            this._localizer = localizer;
            this._respReqLoggerService = respReqLoggerService;
        }

        [HttpGet]
        public IActionResult GradientDescend()
        {
            ViewBag.ActionName = nameof(this.GradientDescend);

            return View(new CalcEquationVM());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult GradientDescend (EquationRequest req)
        {
            ViewBag.ActionName = nameof(this.GradientDescend);
            ViewBag.InputedValuesOfvariables = req.ValuesOfVariables.Keys.ToList();

            CalcEquationVM model = new CalcEquationVM(req);

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                Equation equation = new Equation(req.Equation);

                var result = GradientMethods.GradientMethods.GradientDescent(equation, req.ValuesOfVariables.ToDictionary(k => k.Key, v => v.Value ?? 0.0), req.Accuracy, out int iterationsAmount);

                int acuracyAmountAfterComa = 0;

                double accuracy = req.Accuracy;

                while (accuracy < 1)
                {
                    accuracy *= 10;
                    acuracyAmountAfterComa++;
                }

                var f_x = Math.Round(equation[result], acuracyAmountAfterComa);

                ViewBag.IsMinimum = true;

                model.CalculationResultModel = new EquationCalcResponse(equation.VariablesValues, f_x, iterationsAmount);
            }
            catch (LocalizedException exc)
            {
                ViewBag.ErrorMessage = exc.GetLocalizedMessage(Thread.CurrentThread.CurrentUICulture);
            }
            catch (Exception exc)
            {
                ViewBag.ErrorMessage = _localizer["calculation_error"];
            }

            this._respReqLoggerService.AddNewResponseRequestLog(new ResponseRequestLog(HttpContext.Request.Path.Value + HttpContext.Request.QueryString.Value,

                                                                           model.CalculationResultModel == null ? null : new CalcResponseLog(model.CalculationResultModel.ExtremumPoint.ToDictionary(k => k.Name, v => v.Value),
                                                                                               model.CalculationResultModel.FunctionValue,
                                                                                               model.CalculationResultModel.IterationsAmount),

                                                                           new CalcRequestLog(req.Equation, req.ValuesOfVariables, req.Accuracy),

                                                                           ResponseRequestLogType.CALCULATION)
                                                                           {
                                                                               ErrorMessage = ViewBag.ErrorMessage ?? string.Empty
                                                                           });


            return View(model);
        }


        [HttpGet]
        public IActionResult Newton()
        {
            ViewBag.ActionName = nameof(this.Newton);

            return View(new CalcEquationVM());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Newton (EquationRequest req)
        {
            ViewBag.ActionName = nameof(this.Newton);
            ViewBag.InputedValuesOfvariables = req.ValuesOfVariables.Keys.ToList();

            CalcEquationVM model = new CalcEquationVM(req);

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            int iterationsAmount = 0;

            try
            {
                Equation equation = new Equation(req.Equation);

                bool? IsMinimum = null;

                var result = GradientMethods.GradientMethods.Newton(equation, req.ValuesOfVariables.ToDictionary(k => k.Key, v => v.Value ?? 0.0), req.Accuracy, ref iterationsAmount, out IsMinimum);

                ViewBag.IsMinimum = IsMinimum;

                int acuracyAmountAfterComa = 0;

                double accuracy = req.Accuracy;

                while (accuracy < 1)
                {
                    accuracy *= 10;
                    acuracyAmountAfterComa++;
                }

                var f_x = Math.Round(equation[result], acuracyAmountAfterComa);

                model.CalculationResultModel = new EquationCalcResponse(equation.VariablesValues, f_x, iterationsAmount);
            }
            catch (LocalizedException exc)
            {
                ViewBag.ErrorMessage = exc.GetLocalizedMessage(Thread.CurrentThread.CurrentUICulture);
            }
            catch (Exception exc)
            {
                ViewBag.ErrorMessage = _localizer["calculation_error"];
            }

            this._respReqLoggerService.AddNewResponseRequestLog(new ResponseRequestLog(HttpContext.Request.Path.Value + HttpContext.Request.QueryString.Value,

                                                                                       model.CalculationResultModel == null ? null : new CalcResponseLog(model.CalculationResultModel.ExtremumPoint.ToDictionary(k => k.Name, v => v.Value), 
                                                                                                           model.CalculationResultModel.FunctionValue, 
                                                                                                           model.CalculationResultModel.IterationsAmount),

                                                                                       new CalcRequestLog(req.Equation, req.ValuesOfVariables, req.Accuracy),
                                                                                       ResponseRequestLogType.CALCULATION)
                                                                                       {
                                                                                           ErrorMessage = ViewBag.ErrorMessage ?? string.Empty
                                                                                       });

            return View(model);
        }
    }
}
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

        public MethodsController(IStringLocalizer<CommonResource> localizer)
        {
            this._localizer = localizer;
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

                var result = GradientMethod.GradientDescent(equation, req.ValuesOfVariables.ToDictionary(k => k.Key, v => v.Value ?? 0.0), req.Accuracy, out int iterationsAmount);

                var f_x = equation[result];

                model.CalculationResultModel = new EquationCalcResponse(equation.VariablesValues, f_x, iterationsAmount);
            }
            catch (LocalizedException exc)
            {
                ViewBag.Error = exc.GetLocalizedMessage(Thread.CurrentThread.CurrentUICulture);
            }
            catch (Exception exc)
            {
                ViewBag.Error = _localizer["calculation_error"];
            }

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

                var result = GradientMethod.Newton(equation, req.ValuesOfVariables.ToDictionary(k => k.Key, v => v.Value ?? 0.0), req.Accuracy, ref iterationsAmount);

                var f_x = equation[result];

                model.CalculationResultModel = new EquationCalcResponse(equation.VariablesValues, f_x, iterationsAmount);
            }
            catch (LocalizedException exc)
            {
                ViewBag.Error = exc.GetLocalizedMessage(Thread.CurrentThread.CurrentUICulture);
            }
            catch (Exception exc)
            {
                ViewBag.Error = _localizer["calculation_error"];
            }

            return View(model);
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GradientCalculator.Middlewares.Filters;
using GradientCalculator.Models.Request;
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
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult GradientDescend (EquationRequest req)
        {
            ViewBag.InputedValuesOfvariables = req.ValuesOfVariables.Keys.ToList();

            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                Equation equation = new Equation(req.Equation);

                var result = GradientMethod.GradientDescent(equation, req.ValuesOfVariables.ToDictionary(k => k.Key, v => v.Value ?? 0.0), req.Accuracy, out int iterationsAmount);

                var f_x = equation[result];
            }
            catch (LocalizedException exc)
            {
                ViewBag.Error = exc.GetLocalizedMessage(Thread.CurrentThread.CurrentUICulture);
            }
            catch (Exception exc)
            {
                ViewBag.Error = _localizer["calculation_error"];
            }

            return View();
        }


        [HttpGet]
        public IActionResult Newton()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Newton (EquationRequest req)
        {
            ViewBag.InputedValuesOfvariables = req.ValuesOfVariables.Keys.ToList();

            if (!ModelState.IsValid)
            {
                return View();
            }

            int iterationsAmount = 0;

            try
            {
                Equation equation = new Equation(req.Equation);

                var result = GradientMethod.Newton(equation, req.ValuesOfVariables.ToDictionary(k => k.Key, v => v.Value ?? 0.0), req.Accuracy, ref iterationsAmount);

                var f_x = equation[result];
            }
            catch (LocalizedException exc)
            {
                ViewBag.Error = exc.GetLocalizedMessage(Thread.CurrentThread.CurrentUICulture);
            }
            catch (Exception exc) 
            {
                ViewBag.Error = _localizer["calculation_error"];
            }

            return View();
        }
    }
}
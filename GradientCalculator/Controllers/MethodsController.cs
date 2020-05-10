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
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace GradientCalculator.Controllers
{
    [ServiceFilter(typeof(LanguageActionFilter))]
    public class MethodsController : Controller
    {
        private readonly IStringLocalizer<SharedResource> _localizer;

        public MethodsController(IStringLocalizer<SharedResource> localizer)
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
            ViewBag.InputedValuesOfvariables = Equation.VarsConvertList.Where(v => req.ValuesOfVariables.Keys.Contains(v.Value)).ToDictionary(k => k.Value, v => v.Key);

            if (!ModelState.IsValid || !double.TryParse(req.Accuracy, System.Globalization.NumberStyles.Any, Thread.CurrentThread.CurrentCulture.NumberFormat, out var accuracy) || accuracy < 0 || accuracy > 0.1)
            {
                if (!double.TryParse(req.Accuracy, System.Globalization.NumberStyles.Any, Thread.CurrentThread.CurrentCulture.NumberFormat, out accuracy) || accuracy < 0 || accuracy > 0.1) 
                {
                    ViewBag.InvalidAccuracyErrorMessage = new StringBuilder(_localizer["error_invalid_accuracy"]).ToString();
                }

                return View();
            }

            var result = GradientMethod.GradientDescent(new Equation(req.Equation), req.ValuesOfVariables.ToDictionary(k => k.Key, v => v.Value ?? 0.0), accuracy, out int iterationsAmount);

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
            ViewBag.InputedValuesOfvariables = Equation.VarsConvertList.Where(v => req.ValuesOfVariables.Keys.Contains(v.Value)).ToDictionary(k => k.Value, v => v.Key);

            if (!ModelState.IsValid || !double.TryParse(req.Accuracy, out var accuracy) || accuracy < 0 || accuracy > 0.1)
            {
                if (!double.TryParse(req.Accuracy, out accuracy) || accuracy < 0 || accuracy > 0.1)
                {
                    ViewBag.InvalidAccuracyErrorMessage = _localizer["error_invalid_accuracy"];
                }

                return View();
            }


            int iterationsAmount = 0;
            var result = GradientMethod.Newton(new Equation(req.Equation), req.ValuesOfVariables.ToDictionary(k => k.Key, v => v.Value ?? 0.0), accuracy, ref iterationsAmount);

            return View();
        }
    }
}
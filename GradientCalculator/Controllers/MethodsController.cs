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

            if (!ModelState.IsValid)
            {
                return View();
            }

            var result = GradientMethod.GradientDescent(new Equation(req.Equation), req.ValuesOfVariables.ToDictionary(k => k.Key, v => v.Value ?? 0.0), req.Accuracy, out int iterationsAmount);

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

            if (!ModelState.IsValid)
            {
                return View();
            }


            int iterationsAmount = 0;
            var result = GradientMethod.Newton(new Equation(req.Equation), req.ValuesOfVariables.ToDictionary(k => k.Key, v => v.Value ?? 0.0), req.Accuracy, ref iterationsAmount);

            return View();
        }
    }
}
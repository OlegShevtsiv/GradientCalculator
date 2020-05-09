using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using GradientCalculator.Middlewares.Filters;
using GradientCalculator.Models.Request;
using GradientMethods;
using Microsoft.AspNetCore.Mvc;

namespace GradientCalculator.Controllers
{
    [ServiceFilter(typeof(LanguageActionFilter))]
    public class MethodsController : Controller
    {
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

            if (ModelState.IsValid) 
            {
                
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
            ViewBag.InputedValuesOfvariables = Equation.VarsConvertList.Where(v => req.ValuesOfVariables.Keys.Contains(v.Value)).ToDictionary(k => k.Value, v => v.Key);
            if (ModelState.IsValid)
            {

            }
            return View();
        }
    }
}
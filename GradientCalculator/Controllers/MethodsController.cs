using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GradientCalculator.Middlewares.Filters;
using Microsoft.AspNetCore.Mvc;

namespace GradientCalculator.Controllers
{
    [ServiceFilter(typeof(LanguageActionFilter))]
    public class MethodsController : Controller
    {
        public IActionResult GradientDescend()
        {
            return View();
        }

        public IActionResult Newton()
        {
            return View();
        }
    }
}
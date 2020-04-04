using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using GradientCalculator.Models;
using GradientCalculator.Middlewares.Filters;
using System.Threading;

namespace GradientCalculator.Controllers
{
    [ServiceFilter(typeof(LanguageActionFilter))]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult NotFoundPage() 
        {
            if (HttpContext.Request.Path.Value != $"/{Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName.ToLower()}/Home/NotFoundPage")
            {
                HttpContext.Response.Redirect($"/{Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName.ToLower()}/Home/NotFoundPage", true);
            }

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

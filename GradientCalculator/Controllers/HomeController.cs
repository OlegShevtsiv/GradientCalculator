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
using GradientCalculator.Configs;
using GradientCalculator.Models.Request;
using GradientMethods;
using GradientMethods.ExceptionResult;
using Microsoft.Extensions.Localization;
using GradientCalculator.Services.ResponseRequestLoggerService.Models;
using GradientCalculator.Services.ResponseRequestLoggerService;

namespace GradientCalculator.Controllers
{
    [ServiceFilter(typeof(LanguageActionFilter))]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IStringLocalizer<CommonResource> _localizer;
        private readonly LiteDbStorageService _respReqLoggerService;


        public HomeController(ILogger<HomeController> logger, IStringLocalizer<CommonResource> localizer, LiteDbStorageService respReqLoggerService)
        {
            _logger = logger;
            this._localizer = localizer;
            this._respReqLoggerService = respReqLoggerService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public IActionResult OrdinaryCalculator()
        {
            return View(new MathExpressionRequest());
        }

        [HttpPost]
        public IActionResult OrdinaryCalculator(MathExpressionRequest expresiion)
        {
            ViewBag.InputedValuesOfvariables = expresiion.ValuesOfVariables.Keys.ToList();

            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                Equation equation = new Equation(expresiion.Equation);

                var result = equation[expresiion.ValuesOfVariables.ToDictionary(k => k.Key, v => v.Value ?? 0.0)];

                ViewBag.Result = result;
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
                                                                                       new CalcResponseLog(null, ViewBag.Result, null),
                                                                                       new CalcRequestLog(expresiion.Equation, expresiion.ValuesOfVariables, null),
                                                                                       ResponseRequestLogType.CALCULATION)
                                                                                       {
                                                                                            ErrorMessage = ViewBag.ErrorMessage ?? string.Empty
                                                                                       });

            return View(expresiion);
        }

        public IActionResult Plot2D()
        {
            ViewBag.Lang = (string)HttpContext.Items[co.CookieLangFieldName];
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

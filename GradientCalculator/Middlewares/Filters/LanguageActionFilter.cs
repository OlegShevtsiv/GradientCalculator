using GradientCalculator.Configs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GradientCalculator.Middlewares.Filters
{
    public class LanguageActionFilter : ActionFilterAttribute
    {
        private readonly ILogger _logger;

        public LanguageActionFilter(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger("LanguageActionFilter");
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.RouteData.Values.TryGetValue("url", out _) && context.RouteData.Values["action"].ToString().ToLower().Equals("notfoundpage"))
            {
                _logger.LogInformation($"Unknown request path!");
                _ = Thread.CurrentThread.CurrentUICulture;
            }
            else if (!context.RouteData.Values.ContainsKey("culture"))
            {
                _logger.LogInformation($"Setting the default culture");

                Thread.CurrentThread.CurrentCulture = new CultureInfo(co.DefaultLang_UA);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(co.DefaultLang_UA);
            }
            else
            {
                string culture = context.RouteData.Values["culture"].ToString();

                CookieOptions options = new CookieOptions()
                {
                    HttpOnly = false,
                    Expires = DateTimeOffset.UtcNow.DateTime.AddMonths(1)
                };
                context.HttpContext.Response.Cookies.Delete(co.CookieLangFieldName);
                switch (culture.ToUpper())
                {
                    case "EN":
                        _logger.LogInformation($"Setting the culture from the URL: {co.Lang_EN}");
                        context.HttpContext.Response.Cookies.Append(co.CookieLangFieldName, "en", options);
                        break;
                    case "UK":
                        _logger.LogInformation($"Setting the culture from the URL: {co.DefaultLang_UA}");
                        context.HttpContext.Response.Cookies.Append(co.CookieLangFieldName, "uk", options);
                        break;
                    case "UA":
                        _logger.LogInformation($"Setting the culture from the URL: {co.DefaultLang_UA}");
                        context.HttpContext.Response.Cookies.Append(co.CookieLangFieldName, "uk", options);
                        break;
                    default:
                        _logger.LogInformation($"Unknown culture '{culture}'! Setting the default culture.");

                        context.HttpContext.Response.Redirect($"/{Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName.ToLower()}/Home/NotFoundPage", true);
                        break;
                }
            }

            base.OnActionExecuting(context);
        }
    }
}

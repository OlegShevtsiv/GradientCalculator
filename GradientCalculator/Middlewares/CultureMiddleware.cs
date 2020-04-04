using GradientCalculator.Controllers;
using GradientCalculator.Configs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GradientCalculator.Middlewares
{
    public class CultureMiddleware
    {
        private readonly RequestDelegate _next;

        public CultureMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            //CookieOptions options = new CookieOptions()
            //{
            //    HttpOnly = false,
            //    Expires = DateTimeOffset.UtcNow.DateTime.AddMonths(1),
            //    IsEssential = true
            //};
            if (!context.Request.Cookies.TryGetValue(co.CookieLangFieldName, out string lang))
            {
                lang = "uk";
            }
            else if (string.IsNullOrEmpty(context.Request.Path.Value) || context.Request.Path.Value == "/")
            {
               context.Response.Redirect(ValuesController.RewrightUrlToNewLang((context.Request.Path.Value + context.Request.QueryString.Value).ToLower(), lang));
            }

            if (context.Request.Path.Value.ToLower().StartsWith("/uk") ||
                context.Request.Path.Value.ToLower().StartsWith("/ua") ||
                string.IsNullOrEmpty(context.Request.Path.Value) ||
                context.Request.Path.Value == "/")
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(co.DefaultLang_UA);
                Thread.CurrentThread.CurrentCulture = new CultureInfo(co.DefaultLang_UA);
            }
            else if (context.Request.Path.Value.ToLower().StartsWith("/en"))
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(co.Lang_EN);
                Thread.CurrentThread.CurrentCulture = new CultureInfo(co.Lang_EN);
            }
            await _next.Invoke(context);
        }
    }

    public static class CultureExtensions
    {
        public static IApplicationBuilder UseCulture(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CultureMiddleware>();
        }
    }
}

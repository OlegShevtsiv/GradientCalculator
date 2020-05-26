using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using GradientCalculator.Configs;
using GradientCalculator.Data.Sqlite;
using GradientCalculator.Models;
using GradientCalculator.Services.ResponseRequestLoggerService;
using GradientCalculator.Services.ResponseRequestLoggerService.Models;
using GradientCalculator.ViewModels;
using GradientMethods;
using GradientMethods.ExceptionResult;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;

namespace GradientCalculator.Controllers
{
    public class ValuesController : Controller
    {
        private readonly IStringLocalizer<CommonResource> _localizer;

        public ValuesController(IStringLocalizer<CommonResource> localizer)
        {
            this._localizer = localizer;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangeCulture(ChangeLangVM model)
        {

            if (model.newCulture.ToLower().Equals("uk"))
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(co.DefaultLang_UA);
            }
            else if (model.newCulture.ToLower().Equals("en"))
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(co.Lang_EN);
            }

            return LocalRedirect(RewrightUrlToNewLang(model.url, model.newCulture));
        }

        [NonAction]
        public static List<GCLanguage> GetAvLang(string current)
        {
            List<GCLanguage> langs = new List<GCLanguage>
                     {
                         new GCLanguage("uk", "UK"),
                         new GCLanguage("en", "EN")
                     };

            langs.RemoveAll(l => l.nativeLang.Equals(current.ToLower()));
            return langs;
        }

        [NonAction]
        public static string RewrightUrlToNewLang(string url, string newCulture)
        {
            if (!url.StartsWith("/"))
            {
                url = "/" + url;
            }

            if (url.StartsWith("/uk"))
            {
                url = url.Replace("/uk", "");
            }
            else if (url.StartsWith("/ua"))
            {
                url = url.Replace("/ua", "");
            }
            else if (url.StartsWith("/en"))
            {
                url = url.Replace("/en", "");
            }

            if (url.Contains("//"))
            {
                url = url.Replace("//", "/");
            }

            if (!url.StartsWith("/"))
            {
                url = "/" + url;
            }

            url = "/" + newCulture + url;

            return url;
        }

        [Route("/GetCalculationLogs/{token}")]
        public IActionResult GetCalculationLogs(string token) // not works
        {
            if (token != "tafKQsveRYqWjgQNv7OEJj9uxZzpPDea") 
            {
                return RedirectToActionPermanent(nameof(HomeController.NotFoundPage), "Home");
            }
            else 
            {
                using (LiteDbStorageService s = new LiteDbStorageService())
                {
                    var jsonSetting = new JsonSerializerOptions()
                    {
                        IgnoreNullValues = true,
                        DictionaryKeyPolicy = JsonNamingPolicy.CamelCase
                    };

                    //var result = new JsonResult(s.GetResponseRequestLogs(ResponseRequestLogType.CALCULATION), jsonSetting);

                    return Json(s.GetResponseRequestLogs(ResponseRequestLogType.CALCULATION), jsonSetting);
                }
            }
        }

        [Route("/GetExceptionsLogs/{token}")]
        public IActionResult GetExceptionsLogs(string token) 
        {
            if (token != "tafKQsveRYqWjgQNv7OEJj9uxZzpPDea")
            {
                return RedirectToActionPermanent(nameof(HomeController.NotFoundPage), "Home");
            }
            else
            {
                using (SqliteContext s = new SqliteContext())
                {
                    var result = s.ExceptionLogs.ToList();

                    return Json(result);
                }
            }
        }

        
    }
}
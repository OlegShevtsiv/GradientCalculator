using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GradientCalculator.Middlewares.Filters;
using GradientCalculator.Models.Response;
using GradientMethods;
using GradientMethods.ExceptionResult;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace GradientCalculator.Controllers
{
    [Route("api/Methods/[action]")]
    [ApiController]
    //[ServiceFilter(typeof(LanguageActionFilter))]
    public class MethodsAPIController : ControllerBase
    {
        private readonly IStringLocalizer<SharedResource> _localizer;
        public MethodsAPIController(IStringLocalizer<SharedResource> localizer)
        {
            this._localizer = localizer;
        }

        [HttpPost]
        public WebScriptResponseResult GetVariables(string eq)
        {
            try
            {
                Equation equation = new Equation(eq);

                return new WebScriptResponseResult(equation.Variables.OrderBy(v => v.Index).Select(v => new KeyValuePair<int, char>(v.Index, Equation.VarsConvertList[v.Index])).ToList());
            }
            catch (LocalizedException exc)
            {
                return new WebScriptResponseResult(exc.GetLocalizedMessage(Thread.CurrentThread.CurrentUICulture));
            }
            catch (Exception exc)
            {
                return new WebScriptResponseResult(_localizer["equation_is_not_valid"]);
            }
        }
    }
}
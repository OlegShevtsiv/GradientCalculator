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
        private readonly IStringLocalizer<CommonResource> _localizer;
        public MethodsAPIController(IStringLocalizer<CommonResource> localizer)
        {
            this._localizer = localizer;
        }

        [HttpPost]
        public WebScriptResponseResult GetVariables(string eq)
        {
            try
            {
                Equation equation = new Equation(eq);

                Random valueGenerator = new Random();

                _ = equation[equation.VariablesValues.Select(v => new KeyValuePair<int, double>(key: v.Index, valueGenerator.NextDouble())).ToList()];

                return new WebScriptResponseResult(equation.VariablesValues.OrderBy(v => v.Index).Select(v => new { index = v.Index, name = v.Name }).ToList());
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
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
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
    public class MethodsAPIController : ControllerBase
    {
        private readonly IStringLocalizer<MethodsAPIController> _localizer;
        public MethodsAPIController(IStringLocalizer<MethodsAPIController> localizer)
        {
            this._localizer = localizer;
        }

        [HttpPost]
        public WebScriptResponseResult GetVariables(string eq) 
        {
            try
            {
                Equation equation = new Equation(eq);
                if (equation.Variables.Count == 0) 
                {
                    return new WebScriptResponseResult(null, this._localizer["equation_doesnt_contain_any_variables"]);
                }
                //return new WebScriptResponseResult(equation.Variables.OrderBy(v => v.Index).Select(v => new KeyValuePair<int, char>(v.Index, Equation.VarsConvertList[v.Index])).ToDictionary(k => k.Key, v => v.Value));

                return new WebScriptResponseResult(equation.Variables.OrderBy(v => v.Index).Select(v => new KeyValuePair<int, char>(v.Index, Equation.VarsConvertList[v.Index])).ToList());
            }
            catch (Exception exc)
            {
                return new WebScriptResponseResult(null, exc.Message);
            }
        }
    }
}
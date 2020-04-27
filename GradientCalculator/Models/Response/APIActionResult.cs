using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GradientCalculator.Models.Response
{
    public class APIActionResult : StatusCodeResult
    {
        public string Message { get; set; }

        public Exception Exception { get; set; } = null;

        public APIActionResult(int statusCode, string message) : base(statusCode)
        {
            this.Message = message;
        }

        public APIActionResult(int statusCode, Exception exception, string message = "") : base(statusCode)
        {
            this.Exception = exception;
            this.Message = message;
        }
    }
}

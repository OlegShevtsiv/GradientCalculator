using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Rezervist.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }
        public async Task Invoke(HttpContext context, ILogger<ErrorHandlingMiddleware> _logger/*, RezContext _context*/)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
#if DEBUG
                await HandleExceptionAsync(context, ex, _logger);
#else
               //_context.ErrorLogger.Add(new ErrorLogger(context, ex));
               // _context.SaveChanges();
               // await next(context);
#endif
            }
        }
        private static Task HandleExceptionAsync(
            HttpContext context,
            Exception exception,
            ILogger<ErrorHandlingMiddleware> _logger)
        {
            var code = HttpStatusCode.InternalServerError; // 500 if unexpected
            _logger.LogError("Unhandled excetion. {0}", exception);
            var result = JsonConvert.SerializeObject(
                new
                {
                    Error = exception.InnerException,
                    ErrorDescription = exception.Message
                });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            return context.Response.WriteAsync(result);
        }
    }
}

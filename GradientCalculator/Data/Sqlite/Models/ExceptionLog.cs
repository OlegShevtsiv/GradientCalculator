using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GradientCalculator.Data.Sqlite.Models
{
    public class ExceptionLog
    {
        public int Id { get; set; }

        public string TargetSite { get; set; }

        public string Message { get; set; }

        public string StackTrace { get; set; }

        public string InnerExceptionMessage { get; set; }

        public string Headers { get; set; }

        public string ContentBody { get; set; }

        public string RequestPath { get; set; }

        public string HttpRequestType { get; set; }


        public DateTime Time { get; set; } = DateTime.Now;

        public ExceptionLog()
        {
        }

        public ExceptionLog(HttpContext httpContext, Exception ex) : this()
        {
            #region Exception
            this.InnerExceptionMessage = ex.InnerException?.Message;
            this.Message = ex?.Message + "; Source: " + ex.Source;
            this.TargetSite = ex?.TargetSite.ToString();
            this.StackTrace = ex.StackTrace;
            #endregion

            #region HttpContext
            string header = string.Empty;

            foreach (var h in httpContext.Request.Headers)
            {
                header += $"<{h.Key}> - {h.Value} \n ";
            }

            this.Headers = header;

            this.RequestPath = httpContext.Request.Path.Value + (httpContext.Request.QueryString.HasValue ? httpContext.Request.QueryString.Value : string.Empty);

            this.HttpRequestType = httpContext.Request.Method;


            if (httpContext.Request.Body.CanSeek && httpContext.Request.Body.Length > 0 && httpContext.Request.Body.CanRead)
            {
                var reader = new StreamReader(httpContext.Request.Body);
                reader.BaseStream.Seek(0, SeekOrigin.Begin);
                this.ContentBody = reader.ReadToEnd();
            }
            #endregion
        }

    }
}

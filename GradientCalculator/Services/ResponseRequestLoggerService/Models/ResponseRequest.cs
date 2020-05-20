using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GradientCalculator.Services.ResponseRequestLoggerService.Models
{
    public class ResponseRequestLog
    {
        public string Id { get; set; }

        public string Route { get; set; }

        public object Response { get; set; }

        public object Request { get; set; }

        public string ErrorMessage { get; set; }

        public DateTime DateTime { get; set; }

        public ResponseRequestLog()
        {
            this.Id = Guid.NewGuid().ToString();
            this.DateTime = DateTime.Now;
            this.ErrorMessage = string.Empty;
        }

        public ResponseRequestLog(string route, object response, object request) : this()
        {
            this.Route = route;
            this.Response = response;
            this.Request = request;
        }
    }
}

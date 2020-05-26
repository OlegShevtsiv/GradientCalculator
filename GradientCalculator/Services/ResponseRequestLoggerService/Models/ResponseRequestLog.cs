using System;
using System.Collections.Generic;

namespace GradientCalculator.Services.ResponseRequestLoggerService.Models
{
    public enum ResponseRequestLogType 
    {
        CALCULATION,
        ROUTE_REDIRECT,
        NOT_FOUND_404
    }
    
    public class ResponseRequestLog
    {
        public string Id { get; set; }

        public string Type { get; private set; }

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
            this.Type = ResponseRequestLogType.ROUTE_REDIRECT.ToString();
        }

        public ResponseRequestLog(string route, object response, object request, ResponseRequestLogType type) : this()
        {
            this.Route = route;
            this.Response = response;
            this.Request = request;
            this.Type = type.ToString();
        }

        public void SetType(ResponseRequestLogType type) 
        {
            this.Type = type.ToString();
        }
    }


    public class CalcRequestLog
    {
        public string Equation { get; set; }

        public Dictionary<int, double?> ValuesOfVariables { get; set; }

        public double? Accuracy { get; set; }


        public CalcRequestLog()
        {

        }

        public CalcRequestLog(string equation, Dictionary<int, double?> valuesOfVariables, double? accuracy)
        {
            this.Equation = equation;
            this.ValuesOfVariables = valuesOfVariables;
            this.Accuracy = accuracy;
        }

    }

    public class CalcResponseLog
    {
        public Dictionary<string, double> ExtremumPoint { get; set; }

        public double? FunctionValue { get; set; }

        public int? IterationsAmount { get; set; }

        public CalcResponseLog()
        {

        }

        public CalcResponseLog(Dictionary<string, double> extremumPoint, double? functionValue, int? iterationsAmount)
        {
            this.ExtremumPoint = extremumPoint;
            this.FunctionValue = functionValue;
            this.IterationsAmount = iterationsAmount;
        }
    }


}

using System.Collections.Generic;

namespace GradientCalculator.Models.Response
{
    public class EquationCalcResponse
    {
        public readonly List<GradientMethods.VarValue> ExtremumPoint;

        public readonly double? FunctionValue;

        public readonly int? IterationAmount;

        public EquationCalcResponse()
        {
            this.ExtremumPoint = new List<GradientMethods.VarValue>();
            this.FunctionValue = null;
            this.IterationAmount = null;
        }

        public EquationCalcResponse(List<GradientMethods.VarValue> extremumPoint, double functionValue, int iterationAmount)
        {
            this.ExtremumPoint = extremumPoint;
            this.FunctionValue = functionValue;
            this.IterationAmount = iterationAmount;
        }
    }
}

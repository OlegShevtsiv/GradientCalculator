using System.Collections.Generic;

namespace GradientCalculator.Models.Response
{
    public class EquationCalcResponse
    {
        public readonly List<GradientMethods.VarValue> ExtremumPoint;

        public readonly double? FunctionValue;

        public readonly int? IterationsAmount;

        public EquationCalcResponse()
        {
            this.ExtremumPoint = new List<GradientMethods.VarValue>();
            this.FunctionValue = null;
            this.IterationsAmount = null;
        }

        public EquationCalcResponse(List<GradientMethods.VarValue> extremumPoint, double functionValue, int iterationsAmount)
        {
            this.ExtremumPoint = extremumPoint;
            this.FunctionValue = functionValue;
            this.IterationsAmount = iterationsAmount;
        }
    }
}

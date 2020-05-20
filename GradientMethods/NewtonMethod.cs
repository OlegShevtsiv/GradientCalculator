using GradientMethods.ExceptionResult;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace GradientMethods
{
    public static partial class GradientMethods
    {
        /// <summary>
        /// Gets extremum of function near specified point
        /// </summary>
        /// <param name="function"></param>
        /// <param name="valuesOfVariables"></param>
        /// <param name="accuracy"></param>
        /// <param name="iterationsAmount"></param>
        /// <returns></returns>
        static public IEnumerable<KeyValuePair<int, double>> Newton(this Equation function, IEnumerable<KeyValuePair<int, double>> valuesOfVariables, double accuracy, ref int iterationsAmount, out bool? isMinimum)
        {
            isMinimum = null;

            valuesOfVariables = valuesOfVariables.OrderBy(v => v.Key).ToList();

            List<double> G = function.GetGradient(valuesOfVariables).Select(v => v.Value).ToList();

            double S = 0.0d;

            foreach (var v in G)
            {
                S += Math.Pow(v, 2);
            }

            if (Math.Abs(Math.Sqrt(S)) <= accuracy)
            {

                int acuracyAmountAfterComa = 0;

                var tempAccuracy = accuracy;

                while (tempAccuracy < 1)
                {
                    tempAccuracy *= 10;
                    acuracyAmountAfterComa++;
                }

                isMinimum = function.CheckIsMinimum(valuesOfVariables);

                return valuesOfVariables.Select(v => new KeyValuePair<int, double>(v.Key, Math.Round(v.Value, acuracyAmountAfterComa))).ToList();
            }

            iterationsAmount++;
            Dictionary<int, double> nextPoint = new Dictionary<int, double>();

            var invertibleHessian = function.GetHessian(valuesOfVariables).GetInvertible();

            for (int i = 0; i < valuesOfVariables.Count(); i++)
            {
                nextPoint.Add(valuesOfVariables.ElementAt(i).Key, valuesOfVariables.ElementAt(i).Value - invertibleHessian[i].Multiply(G) );
            }

            return Newton(function, nextPoint, accuracy, ref iterationsAmount, out isMinimum);
        }
    }
}

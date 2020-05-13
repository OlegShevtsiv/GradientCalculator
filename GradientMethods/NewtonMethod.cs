using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace GradientMethods
{
    public partial class GradientMethod
    {

        static public IEnumerable<KeyValuePair<int, double>> Newton(Equation function, IEnumerable<KeyValuePair<int, double>> valuesOfVariables, double accuracy, ref int iterationsAmount)
        {
            valuesOfVariables = valuesOfVariables.OrderBy(v => v.Key).ToList();

            List<double> G = Gradient(function, valuesOfVariables).Select(v => v.Value).ToList();

            double S = 0.0d;

            foreach (var v in G) 
            {
                S += Math.Pow(v, 2);
            }

            if (Math.Abs(Math.Sqrt(S)) <= accuracy)
            {

                int acuracyAmountAfterComa = 0;

                while (accuracy < 1) 
                {
                    accuracy *= 10;
                    acuracyAmountAfterComa++;
                }

                return valuesOfVariables.Select(v => new KeyValuePair<int, double>(v.Key, Math.Round(v.Value, acuracyAmountAfterComa))).ToList();
            }

            iterationsAmount++;
            Dictionary<int, double> nextPoint = new Dictionary<int, double>();

            for(int i = 0; i < valuesOfVariables.Count(); i++)
            {
                nextPoint.Add(valuesOfVariables.ElementAt(i).Key, valuesOfVariables.ElementAt(i).Value - MultiplyVectors(GetInvertibleMatrix(Hessian(function, valuesOfVariables))[i], G));
            }

            return Newton(function, nextPoint, accuracy, ref iterationsAmount);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;

namespace GradientMethods
{
    public partial class GradientMethod
    {
        static public IEnumerable<KeyValuePair<int, double>> GradientDescent(Equation function, IEnumerable<KeyValuePair<int, double>> valuesOfVariables, double accuracy, out int iterationsAmount)
        {
            iterationsAmount = 0;

            double S;

            Dictionary<int, double> G = new Dictionary<int, double>();// gradient
            Dictionary<int, double> M0 = new Dictionary<int, double>(valuesOfVariables.OrderBy(v => v.Key).ToList());// current point 
            Dictionary<int, double> M1 = new Dictionary<int, double>(); // point to find

            double a = 1.0d;
            do
            {
                S = 0.0d;
                G = new Dictionary<int, double>(Gradient(function, M0));
                M1 = new Dictionary<int, double>();

                foreach (var x in valuesOfVariables)
                {
                    M1.Add(x.Key, M0[x.Key] - a * G[x.Key]); // step in the direction of the antigriant
                    S += Math.Pow(G[x.Key], 2);
                }

                double res1 = function[M0];
                double res2 = function[M1];

                if (res2 >= res1) //check monotony
                {
                    a /= 1.1;
                }
                else
                {
                    M0 = M1;
                    iterationsAmount++;
                    a = 1;
                }
            }
            while ((a >= accuracy) && (Math.Abs(Math.Sqrt(S)) >= accuracy));

            int acuracyAmountAfterComa = 0;

            while (accuracy < 1)
            {
                accuracy *= 10;
                acuracyAmountAfterComa++;
            }

            return M0.Select(v => new KeyValuePair<int, double>(v.Key, Math.Round(v.Value, acuracyAmountAfterComa))).ToList();
        }
    }
}

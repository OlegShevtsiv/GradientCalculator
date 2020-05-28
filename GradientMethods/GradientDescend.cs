using GradientMethods.ExceptionResult;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GradientMethods
{
    public static partial class GradientMethods
    {
        static public IEnumerable<KeyValuePair<int, double>> GradientDescent(Equation function, IEnumerable<KeyValuePair<int, double>> valuesOfVariables, double accuracy, out int iterationsAmount)
        {
            iterationsAmount = 0;

            int acuracyAmountAfterComa = 0;

            double tempAccuracy = accuracy;

            while (tempAccuracy < 1)
            {
                tempAccuracy *= 10;
                acuracyAmountAfterComa++;
            }

            double Sum;

            Dictionary<int, double> G = new Dictionary<int, double>();// gradient
            Dictionary<int, double> M0 = new Dictionary<int, double>(valuesOfVariables.OrderBy(v => v.Key).ToList());// current point 
            Dictionary<int, double> M1 = new Dictionary<int, double>(); // point to find

            double a = 1.0d;

            double b = 1.1;

            int checkingMonotonyAmount = 1;

            do
            {
                Sum = 0.0d;
                G = new Dictionary<int, double>(function.GetGradient(M0));
                M1 = new Dictionary<int, double>();

                foreach (var x in valuesOfVariables)
                {
                    M1.Add(x.Key, M0[x.Key] - a * G[x.Key]); // step in the direction of the antigriant
                    Sum += Math.Pow(G[x.Key], 2);
                }

                double res1 = function[M0];
                double res2 = function[M1];

                if (res2 >= res1) //check monotony
                {
                    if (checkingMonotonyAmount % 10 == 0) 
                    {
                        b += 0.5;
                    }
                    a /= b;
                    checkingMonotonyAmount++;
                }
                else
                {
                    checkingMonotonyAmount = 1;
                    b = 1.1;
                    M0 = M1;
                    iterationsAmount++;

                    if (iterationsAmount >= 1_000_000)
                    {
                        throw new LocalizedException("extremum_not_found");
                    }

                    a = 1;
                }
            }
            while ((a >= accuracy) && (Math.Abs(Math.Sqrt(Sum)) >= accuracy));

            return M0.Select(v => new KeyValuePair<int, double>(v.Key, Math.Round(v.Value, acuracyAmountAfterComa))).ToList();
        }
    }
}

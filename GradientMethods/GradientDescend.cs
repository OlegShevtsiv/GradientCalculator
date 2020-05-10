using System;
using System.Collections.Generic;
using System.Linq;

namespace GradientMethods
{
    public partial class GradientMethod
    {
        static public List<double> GradientDescent(Equation function, IEnumerable<KeyValuePair<char, double>> valuesOfVariables, double accuracy, out int iterationsAmount)
        {
            iterationsAmount = 0;

            double S;

            Dictionary<char, double> G = new Dictionary<char, double>();// gradient
            Dictionary<char, double> M0 = new Dictionary<char, double>(valuesOfVariables);// current point 
            Dictionary<char, double> M1 = new Dictionary<char, double>(); // point to find

            double a = 1.0d;
            do
            {
                S = 0.0d;
                G = Gradient(function, M0);
                M1 = new Dictionary<char, double>();

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
            return M0.Values.ToList();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;

namespace GradientMethods
{
    public partial class GradientMethod
    {
        static public List<double> GradientDescent(Equation F, IEnumerable<KeyValuePair<char, double>> X, double eps, ref int iterAmount)
        {
            double S;
            Dictionary<char, double> G = new Dictionary<char, double>();// gradient
            Dictionary<char, double> M0 = new Dictionary<char, double>(X.ToDictionary(k => k.Key, v => v.Value));// current point 
            Dictionary<char, double> M1 = new Dictionary<char, double>(); // point to find
            double a = 1.0d;
            do
            {
                S = 0.0d;
                G = Gradient(F, M0);
                M1 = new Dictionary<char, double>();

                foreach (var x in X)
                {
                    M1.Add(x.Key, M0[x.Key] - a * G[x.Key]); // step in the direction of the antigriant
                    S += Math.Pow(G[x.Key], 2);
                }

                double res1 = F[M0];
                double res2 = F[M1];

                if (res2 >= res1) //check monotony
                {
                    a /= 1.1;
                }
                else
                {
                    M0 = M1;
                    iterAmount++;
                    a = 1;
                }
            }
            while ((a >= eps) && (Math.Abs(Math.Sqrt(S)) >= eps));
            return M0.Values.ToList();
        }
    }
}

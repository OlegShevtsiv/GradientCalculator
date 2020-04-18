using System;
using System.Collections.Generic;
using System.Linq;

namespace GradientMethods
{
    public partial class GradientMethod
    {
        static public List<double> Newton(Equation F, Dictionary<char, double> X, double eps, ref int iterAmount)
        {
            Dictionary<char, double> G = Gradient(F, X);
            double S = 0.0d;

            for (int i = 0; i < X.Count; i++)
            {
                S += Math.Pow(G[Equation.VarsConvertList[i]], 2);
            }
            if (Math.Abs(Math.Sqrt(S)) <= eps)
            {
                return X.Values.ToList();
            }
            iterAmount++;
            Dictionary<char, double> M1 = new Dictionary<char, double>();
            for (int i = 0; i < X.Count; i++)
            {
                M1.Add(Equation.VarsConvertList[i], X[Equation.VarsConvertList[i]] - MultVec(GetInvertibleMatrix(Hessian(F, X))[i], G.Values.ToList()));
            }
            return Newton(F, M1, eps, ref iterAmount);
        }
    }
}

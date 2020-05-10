using System;
using System.Collections.Generic;
using System.Linq;

namespace GradientMethods
{
    public partial class GradientMethod
    {

        static public List<double> Newton(Equation function, Dictionary<char, double> valuesOfVariables, double accuracy, ref int iterationsAmount)
        {
            Dictionary<char, double> G = Gradient(function, valuesOfVariables);
            double S = 0.0d;

            for (int i = 0; i < valuesOfVariables.Count; i++)
            {
                S += Math.Pow(G[Equation.VarsConvertList[i]], 2);
            }
            if (Math.Abs(Math.Sqrt(S)) <= accuracy)
            {
                return valuesOfVariables.Values.ToList();
            }
            iterationsAmount++;
            Dictionary<char, double> nextPoint = new Dictionary<char, double>();
            for (int i = 0; i < valuesOfVariables.Count; i++)
            {
                nextPoint.Add(Equation.VarsConvertList[i], valuesOfVariables[Equation.VarsConvertList[i]] - MultiplyVectors(GetInvertibleMatrix(Hessian(function, valuesOfVariables))[i], G.Values.ToList()));
            }
            return Newton(function, nextPoint, accuracy, ref iterationsAmount);
        }
    }
}

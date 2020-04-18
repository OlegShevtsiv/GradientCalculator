using System;
namespace GradientMethods
{
    public static class MathExtension
    {
        public static double Acot(double val)
        {
            return (Math.PI / 2) - Math.Atan(val);
        }

        public static double Cot(double val)
        {
            return 1 / Math.Tan(val);
        }
    }
}

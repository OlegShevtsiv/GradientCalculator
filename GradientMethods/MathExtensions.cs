using System;
namespace GradientMethods
{
    public static class MathExtension
    {
        public static double Acot(double value)
        {
            return (Math.PI / 2) - Math.Atan(value);
        }

        public static double Cot(double value)
        {
            return 1 / Math.Tan(value);
        }
    }
}

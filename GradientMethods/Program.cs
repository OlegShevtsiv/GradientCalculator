using GradientMethods.ExceptionResult;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;

namespace GradientMethods
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Equation eq = new Equation("0.1 * (x1 - x2 ^ 2)^2 +( 1.5 - x2 ^ 2) ^ 2");
                //Equation eq = new Equation("(x1 + 10 * x2)^2 + 5*(x3 - x4)^2 + (x2 - 2*x3)^4 + 10*(x1-x4)^2");
                //Equation eq = new Equation("(x2-x1^2)^2 + 2*(1-x1)^2");

                //Equation eq = new Equation("(x1^2 + x2-11)^2 + (x1+x2^2 -7)^2");



                Console.WriteLine(eq.Display());

                Dictionary<int, double> inputVars;

                int iterAmount;
                double eps;

                int choise = 1;
                while (choise == 1)
                {
                    Console.Write("Enter accuracy of calculations: ");
                    eps = double.TryParse(Console.ReadLine(), out var resEps) ? resEps : 0.00001d;

                    Console.WriteLine("------ Enter initial point ------");
                    inputVars = new Dictionary<int, double>();
                    foreach (var v in eq.VariablesValues.OrderBy(vr => vr.Index))
                    {
                        Console.Write($"------ {v} = ");
                        inputVars.Add(v.Index, Convert.ToDouble(Console.ReadLine()));
                        Console.WriteLine();
                    }

                    Console.WriteLine("============== RESULTS ==============");


                    //Console.WriteLine("Gradient Descent Method: ");
                    iterAmount = 0;
                    var GDResult = GradientMethods.Newton(eq, inputVars, eps, ref iterAmount, out _);

                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    Console.Write("M(");
                    for (int i = 0; i < GDResult.Count() - 1; i++)
                    {
                        Console.Write($"{GDResult.ElementAt(i).Value},");
                    }
                    Console.Write($"{GDResult.ElementAt(GDResult.Count() - 1).Value})");
                    Console.ResetColor();

                    Console.WriteLine($"\nF(M) = {eq[GDResult]}");

                    Console.WriteLine($"Iterations amount: {iterAmount}");


                    Console.WriteLine("-------------------------------------");

                    Console.WriteLine("Enter '1' to continue and enter another button to exit.");
                    Console.Write(">>> ");
                    choise = Convert.ToInt32(Console.ReadLine());
                }


                //Equation eq = new Equation("x0 + sin(x1 / 2) - cos(x0 * 9)*tan(x3) + |sin(x2 + x1)| + x8"); // 18.2

                //Console.WriteLine(eq);

                //Dictionary<char, double> inputVars = new Dictionary<char, double>();

                //Console.WriteLine("Enter values of variables: ");
                //foreach (var v in eq.Variables.OrderBy(vr => vr.Index))
                //{
                //    Console.WriteLine($"{v} = ");
                //    inputVars.Add(Equation.VarsConvertList[v.Index], Convert.ToDouble(Console.ReadLine()));
                //}

                //Console.WriteLine("================");
                //Console.WriteLine(eq[inputVars]);


                //Equation eq1 = new Equation("|sin(x1 + 2* cos(x0^2 - 1) * tan(6 - cot(sin(x1 + x0 + 10))))|");
                //Console.WriteLine(eq[new Dictionary<char, double>
                //{
                //    {Equation.VarsConvertList[0], 3.1},
                //    {Equation.VarsConvertList[1], 2},
                //    {Equation.VarsConvertList[2], 1},
                //    {Equation.VarsConvertList[3], 1.5},
                //    {Equation.VarsConvertList[8], 1}
                //}]);


            }
            catch (LocalizedException exc)
            {
                Console.WriteLine(exc.GetLocalizedMessage(new CultureInfo("uk")));
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }

            Console.ReadKey();
        }
    }
}

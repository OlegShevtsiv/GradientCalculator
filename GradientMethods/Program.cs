using System;
using System.Collections.Generic;
using System.Linq;

namespace GradientMethods
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Equation eq = new Equation("0.1*(x1-x2^2)^2+(1.5-x2^2)^2");

                Console.WriteLine(eq);

                Dictionary<char, double> inputVars;

                int iterAmount;
                double eps;

                int choise = 1;
                while (choise == 1)
                {
                    Console.Write("Enter accuracy of calculations: ");
                    eps = Convert.ToDouble(Console.ReadLine());

                    Console.WriteLine("------ Enter initial point ------");
                    inputVars = new Dictionary<char, double>();
                    foreach (var v in eq.Variables.OrderBy(vr => vr.Index))
                    {
                        Console.Write($"------ {v} = ");
                        inputVars.Add(Equation.VarsConvertList[v.Index], Convert.ToDouble(Console.ReadLine()));
                        Console.WriteLine();

                    }

                    Console.WriteLine("============== RESULTS ==============");


                    Console.WriteLine("Gradient Descent Method: ");
                    iterAmount = 0;
                    List<double> GDResult = GradientMethod.GradientDescent(eq, inputVars, eps, ref iterAmount);

                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    Console.Write("M(");
                    for (int i = 0; i < GDResult.Count - 1; i++)
                    {
                        Console.Write($"{GDResult[i].ToString()},");
                    }
                    Console.Write($"{GDResult[GDResult.Count - 1]})");
                    Console.ResetColor();

                    Console.WriteLine($"\nF(M) = {Math.Round(eq[inputVars.Select(i => new KeyValuePair<char, double>(key: i.Key, value: GDResult[inputVars.Keys.ToList().IndexOf(i.Key)]))], 10)}");

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
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }
        }
    }
}

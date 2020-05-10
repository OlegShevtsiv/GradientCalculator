﻿using GradientMethods.ExceptionResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace GradientMethods
{
    public class VarValue
    {
        private char Name;
        public int Index { get; set; }

        public VarValue(string value)
        {
            if (value.Length != 2 || !int.TryParse(value[1].ToString(), out var res))
            {
                throw new LocalizedException("error_creating_value_of_variable");
            }

            this.Name = value[0];
            this.Index = res;
        }

        public override string ToString()
        {
            return $"{this.Name}{this.Index}".ToLower();
        }
    }


    /// <summary>
    /// * write legend!!!!
    /// </summary>
    public sealed class Equation
    {
        public double this[IEnumerable<KeyValuePair<char, double>> valuesOfVariables]
        {
            get
            {
                return this.Calculate(valuesOfVariables.ToDictionary(k => k.Key, v => v.Value));
            }
        }


        private string equation;
        private Dictionary<char, double> valuesOfVariables;
        private List<int> variablesIndexes = new List<int>();

        public List<VarValue> Variables
        {
            get
            {
                return this.variablesIndexes.Select(v => new VarValue($"x{v}")).ToList();
            }
        }

        public Equation(string equation)
        {
            this.equation = this.Normalyze(equation);
        }

        private double Calculate(Dictionary<char, double> _valuesOfVariables)
        {
            this.valuesOfVariables = _valuesOfVariables;

            if (_valuesOfVariables.Count != this.variablesIndexes.Count)
            {
                throw new LocalizedException("incorect_input_list_of_variable_values");
            }

            foreach (var el in this.Constants)
            {
                _valuesOfVariables.Add(el.Key, el.Value);
            }

            string tempEquation = this.equation;
            while (this.ContainsFunctions(tempEquation))
            {
                tempEquation = this.CalcFunctions(tempEquation);
            }

            bool may_unary = true;
            Stack<string> operands = new Stack<string>();
            Stack<char> operations = new Stack<char>();

            for (int i = 0; i < tempEquation.Length; i++)
            {
                if (tempEquation[i] == '(')
                {
                    operations.Push('(');
                    may_unary = true;
                }
                else if (tempEquation[i] == ')')
                {
                    while (operations.First() != '(')
                    {
                        this.CalcProcessIteration(ref operands, _valuesOfVariables, operations.First());
                        operations.Pop();
                    }

                    operations.Pop();
                    may_unary = false;
                }
                else if (this.isOperator(tempEquation[i]))
                {
                    char curop = tempEquation[i];
                    if (may_unary && (i == 0 || !char.IsLetterOrDigit(tempEquation[i - 1])))
                    {
                        curop = (char)(-curop);
                    }

                    while (operations.Any() && (!this.isRightAssociative(curop) && this.GetPriority(operations.First()) >= this.GetPriority(curop) ||
                                                this.isRightAssociative(curop) && this.GetPriority(operations.First()) > this.GetPriority(curop)))
                    {
                        this.CalcProcessIteration(ref operands, _valuesOfVariables, operations.First());
                        operations.Pop();
                    }
                    operations.Push(curop);
                    may_unary = true;
                }
                else
                {
                    string operand = string.Empty;

                    if (char.IsLetter(tempEquation[i]))
                    {
                        operand = tempEquation[i].ToString();
                        operands.Push(_valuesOfVariables[operand[0]].ToString());
                    }
                    else if (char.IsDigit(tempEquation[i]))
                    {
                        operand = this.GetConstant(tempEquation, out i, i);
                        operands.Push(operand);
                    }
                    may_unary = false;
                }
            }

            while (operations.Any())
            {
                this.CalcProcessIteration(ref operands, _valuesOfVariables, operations.First());
                operations.Pop();
            }

            if (operands.Count != 1)
            {
                throw new LocalizedException("equation_is_not_valid");
            }

            double res;

            double.TryParse(operands.First(), out res);

            return res;
        }

        private bool isRightAssociative(char op)
        {
            if (op == '^' || this.isUnary(op, out _))
            {
                return true;
            }
            return false;
        }

        private bool isUnary(char op, out char newOp)
        {
            if ((char)(-op) == '+' || (char)(-op) == '-')
            {
                newOp = (char)(-op);
                return true;
            }
            newOp = ' ';
            return false;
        }

        private string GetConstant(string str, out int lastIndex, int startIndex = 0)
        {
            if (string.IsNullOrEmpty(str) || !char.IsDigit(str[startIndex]))
            {
                throw new LocalizedException("constant_not_found");
            }
            else
            {
                string numb = string.Empty;
                while (startIndex < str.Length && (char.IsDigit(str[startIndex])
                                                    || str[startIndex] == '.'
                                                    || str[startIndex] == ','))
                {
                    numb += str[startIndex];
                    startIndex++;
                }

                if (double.TryParse(numb, out _))
                {
                    lastIndex = startIndex - 1;
                    return numb;
                }
                throw new LocalizedException("error_geting_constant");
            }
        }

        private void CalcProcessIteration(ref Stack<string> operands, Dictionary<char, double> vars, char operation)
        {
            if (operands.Count < 1)
            {
                throw new LocalizedException("calculation_error");
            }
            char unary_op;
            if (this.isUnary(operation, out unary_op))
            {
                string l = operands.Pop();
                double oper;

                if (!double.TryParse(l, out oper))
                {
                    oper = vars[l[0]];
                }

                switch (unary_op)
                {
                    case '+':
                        operands.Push(oper.ToString());
                        break;
                    case '-':
                        operands.Push((-oper).ToString());
                        break;
                }
            }
            else
            {
                double first;
                double second;

                string s = operands.Pop();
                string f = operands.Pop();

                if (!double.TryParse(f, out first))
                {
                    first = vars[f[0]];
                }

                if (!double.TryParse(s, out second))
                {
                    second = vars[s[0]];
                }

                switch (operation)
                {
                    case '+':
                        operands.Push((first + second).ToString());
                        break;
                    case '-':
                        operands.Push((first - second).ToString());
                        break;
                    case '*':
                        operands.Push((first * second).ToString());
                        break;
                    case '/':
                        operands.Push((first / second).ToString());
                        break;
                    case '÷':
                        operands.Push((first / second).ToString());
                        break;
                    case '^':
                        operands.Push((Math.Pow(first, second)).ToString());
                        break;
                }
            }
        }


        private bool ContainsFunctions(string equationPart)
        {
            if (equationPart.Contains("sin") ||
                equationPart.Contains("cos") ||
                equationPart.Contains("asin") ||
                equationPart.Contains("acos") ||
                equationPart.Contains("tan") ||
                equationPart.Contains("cot") ||
                equationPart.Contains("arctan") ||
                equationPart.Contains("arccot") ||
                equationPart.Contains("sinh") ||
                equationPart.Contains("cosh") ||
                equationPart.Contains("tanh") ||
                equationPart.Contains("ln") ||
                equationPart.Contains("lg") ||
                equationPart.Contains("|"))
            {
                return true;
            }
            return false;
        }

        private delegate double MathFunction(double parametr);

        private string CalcFunctions(string equationPart)
        {
            string GetPart(string mathFunctionName, MathFunction mathFunction)
            {
                int startFuncIndex = equationPart.IndexOf(mathFunctionName, StringComparison.InvariantCultureIgnoreCase);

                int openingBracketIndex;

                if (mathFunctionName == "|")
                {
                    openingBracketIndex = startFuncIndex + 1;
                }
                else
                {
                    openingBracketIndex = equationPart.IndexOf('(', startFuncIndex);
                }

                int openBracketsAmount = openingBracketIndex > 0 ? 1 : 0;

                int closedBracketIndex = 0;

                if (openBracketsAmount == 0)
                {
                    throw new LocalizedException("error_braket_missing");
                }

                if (mathFunctionName == "|")
                {
                    for (int i = openingBracketIndex; i < equationPart.Length; i++)
                    {
                        if (equationPart[i] == '|')
                        {
                            closedBracketIndex = i - 1;
                            break;
                        }
                    }
                }
                else
                {
                    for (int i = openingBracketIndex + 1; i < equationPart.Length; i++)
                    {
                        if (equationPart[i] == '(')
                        {
                            openBracketsAmount++;
                        }
                        else if (equationPart[i] == ')' && openBracketsAmount > 0)
                        {
                            openBracketsAmount--;
                            closedBracketIndex = i;
                        }
                        if (openBracketsAmount == 0)
                        {
                            break;
                        }
                    }
                }

                string newEquation = equationPart.Substring(openingBracketIndex, closedBracketIndex - openingBracketIndex + 1);

                foreach (var vcl in VarsConvertList)
                {
                    if (newEquation.Contains(vcl.Value.ToString()))
                    {
                        newEquation = newEquation.Replace(vcl.Value.ToString(), $"x{vcl.Key}");
                    }
                }

                string partToReplace;

                if (mathFunctionName == "|")
                {
                    partToReplace = equationPart.Substring(startFuncIndex, closedBracketIndex - startFuncIndex + 2);
                }
                else
                {
                    partToReplace = equationPart.Substring(startFuncIndex, closedBracketIndex - startFuncIndex + 1);
                }

                Equation funcCalcEq = new Equation(newEquation);

                Dictionary<char, double> valOfVars = new Dictionary<char, double>();

                if (funcCalcEq.variablesIndexes.Count > 0)
                {
                    foreach (var v in funcCalcEq.variablesIndexes)
                    {
                        valOfVars.Add(this.valuesOfVariables.First(vov => vov.Key == VarsConvertList[v]).Key, this.valuesOfVariables.First(vov => vov.Key == VarsConvertList[v]).Value);
                    }
                }

                double result = mathFunction(funcCalcEq.Calculate(valOfVars));

                equationPart = equationPart.Replace(partToReplace, result.ToString().Replace(',', '.'));

                return equationPart;
            }

            if (equationPart.Contains("asin"))
            {
                return GetPart(nameof(Math.Asin), Math.Asin);
            }
            if (equationPart.Contains("acos"))
            {
                return GetPart(nameof(Math.Acos), Math.Acos);
            }
            if (equationPart.Contains("arctan"))
            {
                return GetPart("arctan", Math.Atan);
            }
            if (equationPart.Contains("arccot"))
            {
                return GetPart("arccot", MathExtension.Acot);
            }
            if (equationPart.Contains("sinh"))
            {
                return GetPart(nameof(Math.Sinh), Math.Sinh);
            }
            if (equationPart.Contains("cosh"))
            {
                return GetPart(nameof(Math.Cosh), Math.Cosh);
            }
            if (equationPart.Contains("tanh"))
            {
                return GetPart(nameof(Math.Tanh), Math.Tanh);
            }
            if (equationPart.Contains("sin"))
            {
                return GetPart(nameof(Math.Sin), Math.Sin);
            }
            if (equationPart.Contains("cos"))
            {
                return GetPart(nameof(Math.Cos), Math.Cos);
            }
            if (equationPart.Contains("tan"))
            {
                return GetPart(nameof(Math.Tan), Math.Tan);
            }
            if (equationPart.Contains("cot"))
            {
                return GetPart("cot", MathExtension.Cot);
            }
            if (equationPart.Contains("ln"))
            {
                return GetPart("ln", Math.Log);
            }
            if (equationPart.Contains("lg"))
            {
                return GetPart("lg", Math.Log10);
            }
            if (equationPart.Contains("|"))
            {
                return GetPart("|", Math.Abs);
            }
            return equationPart;
        }

        private int GetPriority(char op)
        {
            if (op == '+' || op == '-')
            {
                return 1;
            }
            if (op == '*' || op == '/' || op == '÷')
            {
                return 2;
            }
            if (op == '^')
            {
                return 3;
            }
            if (this.isUnary(op, out _))
            {
                return 4;
            }

            return -1;
        }

        private bool isOperator(char o)
        {
            if (o == '+' 
             || o == '-' 
             || o == '*'
             || o == '/' 
             || o == '÷' 
             || o == '^')
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Removes off white spaces, checks whether input string is validate otherwise throws exception, and replace all variables like x{number=0..9} to one char symbol
        /// </summary>
        /// <param name="eq"></param>
        /// <returns></returns>
        private string Normalyze(string eq)
        {
            if (string.IsNullOrEmpty(eq))
            {
                throw new LocalizedException("equation_is_empthy");
            }

            if (new Regex(@"\p{IsCyrillic}").IsMatch(eq))
            {
                throw new LocalizedException("equation_not_valid_it_contains_not_allowed_cyrillic_symbols");
            }

            if (eq.Count(e => e == '(') != eq.Count(e => e == ')'))
            {
                throw new LocalizedException("equation_not_valid_amounts_of_)_and_(_are_not_equal");
            }

            eq = eq.ToLower();

            eq = eq.Replace(" ", string.Empty);

            if (eq.Contains("x"))
            {
                List<char> vars = new List<char>();

                for (int i = eq.IndexOf("x", StringComparison.InvariantCulture); i < eq.Length - 1; i++)
                {
                    if (eq[i] == 'x')
                    {
                        if (char.IsNumber(eq[i + 1]))
                        {
                            vars.Add(eq[i + 1]);
                        }
                        else
                        {
                            throw new LocalizedException("equation_not_valid_it_has_to_contains_variables_matching_pattern_X_(_number_0__9_)_");
                        }
                    }
                }

                this.equationWithoutNormalizatoin = eq;

                this.variablesIndexes = vars.Distinct().Select(v => int.TryParse(v.ToString(), out int res) ? res : -1).ToList();

                if (this.variablesIndexes.Count() > 0)
                {
                    int ind = 0;
                    foreach (var v in this.Variables)
                    {
                        if (int.TryParse(v.ToString()[1].ToString(), out ind))
                        {
                            eq = eq.Replace(v.ToString(), VarsConvertList[ind].ToString());
                        }
                    }
                }
            }

            eq = eq.Replace(",", ".");

            return eq;
        }

        private string equationWithoutNormalizatoin;

        /// <summary>
        /// Returns equation string.
        /// </summary>
        public override string ToString()
        {
            return this.equationWithoutNormalizatoin;
        }

        /// <summary>
        /// Convert varible of equation x{number=0..9} to one symbol (it is for comfort of calculation)
        /// </summary>
        public static readonly Dictionary<int, char> VarsConvertList = new Dictionary<int, char>//to incapsulate in future
        { 
             {0, 'm'},
             {1, 'b'},
             {2, 'q'},
             {3, 'd'},
             {4, 'f'},
             {5, 'u'},
             {6, 'v'},
             {7, 'j'},
             {8, 'y'},
             {9, 'z'}
        };


        /// <summary>
        /// base math constants
        /// </summary>
        private readonly Dictionary<char, double> Constants = new Dictionary<char, double>
        {
            {'π', Math.PI},
            {'p', Math.PI},
            {'e', Math.E}
        };
    }
}
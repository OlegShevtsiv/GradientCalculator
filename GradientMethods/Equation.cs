using GradientMethods.ExceptionResult;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace GradientMethods
{
    public sealed class VarValue
    {
        public readonly string Name;
        private string pattern => @"^x[0-9]$";

        public readonly int Index;

        public double Value { get; set; } = 0.0;

        public VarValue(string value)
        {
            if (!Regex.IsMatch(value, pattern, RegexOptions.IgnoreCase))
            {
                throw new LocalizedException("error_creating_value_of_variable");
            }

            this.Name = value.ToLower();
            int.TryParse(value[1].ToString(), out this.Index);
        }

        public override string ToString()
        {
            return this.Name;
        }
    }

    public sealed class Equation
    {
        private readonly string equation;

        /// <summary>
        /// base math constants
        /// </summary>
        private readonly Dictionary<char, double> Constants = new Dictionary<char, double>
        {
            {'π', Math.PI},
            {'p', Math.PI},
            {'e', Math.E}
        };

        public List<VarValue> VariablesValues { get; private set; }

        /// <summary>
        /// <para> Use this legend to write your equation correct:</para> 
        /// <para> Variables -- use pattern x{number=0..9}</para> 
        /// <para> Math constants -- π or p, e</para> 
        /// <para> Functions -- sin(), cos(), asin(), acos(), tan(), cot(), arctan(), arccot(), sinh(), cosh(), tanh(), ln(), lg(), || </para> 
        /// </summary>
        /// <param name="equation">Equation string</param>
        public Equation(string equation)
        {
            this.equation = this.Normalyze(equation);
        }

        public double this[IEnumerable<KeyValuePair<int, double>> valuesOfVariables]
        {
            get
            {
                if (valuesOfVariables == null || valuesOfVariables.Count() != this.VariablesValues.Count)
                {
                    throw new LocalizedException("incorect_input_list_of_variable_values");
                }

                valuesOfVariables = valuesOfVariables.OrderBy(v => v.Key).ToList();

                for (int i = 0; i < valuesOfVariables.Count(); i++)
                {
                    if (!this.VariablesValues.Exists(v => v.Index == valuesOfVariables.ElementAt(i).Key))
                    {
                        throw new LocalizedException("incorect_input_list_of_variable_values");
                    }
                    this.VariablesValues[i].Value = valuesOfVariables.ElementAt(i).Value;
                }

                return this.Calculate();
            }
        }

        private double Calculate()
        {
            string tempEquation = this.equation;
            while (this.ContainsFunctions(tempEquation))
            {
                tempEquation = this.CalcFunctions(tempEquation);
            }

            bool may_unary = true;
            Stack<double> operands = new Stack<double>();
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
                        this.CalcProcessIteration(ref operands, operations.First());
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
                        this.CalcProcessIteration(ref operands, operations.First());
                        operations.Pop();
                    }
                    operations.Push(curop);
                    may_unary = true;
                }
                else
                {
                    double operand;

                    if (char.IsLetter(tempEquation[i]))
                    {
                        if (tempEquation[i] == 'x')
                        {
                            i++;

                            int index = int.TryParse(tempEquation[i].ToString(), out var ind) ? ind : throw new LocalizedException("calculation_error");

                            operand = this.VariablesValues.First(v => v.Index == index).Value;

                            operands.Push(operand);
                        }
                        else if (this.Constants.ContainsKey(tempEquation[i]))
                        {
                            operand = this.Constants[tempEquation[i]];

                            operands.Push(operand);
                        }
                        else
                        {
                            throw new LocalizedException("calculation_error");
                        }
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
                this.CalcProcessIteration(ref operands, operations.First());
                operations.Pop();
            }

            if (operands.Count != 1)
            {
                throw new LocalizedException("equation_is_not_valid");
            }

            return operands.Pop();
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
            if ((char)(-op) == '+' || (char)(-op) == '-' || (char)(-op) == '–')
            {
                newOp = (char)(-op);
                return true;
            }
            newOp = ' ';
            return false;
        }

        private double GetConstant(string str, out int lastIndex, int startIndex = 0)
        {
            if (string.IsNullOrEmpty(str) || !char.IsDigit(str[startIndex]))
            {
                throw new LocalizedException("constant_not_found");
            }
            else
            {
                string numb = string.Empty;
                while (startIndex < str.Length && (char.IsDigit(str[startIndex])
                                                    || str[startIndex] == '.'))
                {
                    numb += str[startIndex];
                    startIndex++;
                }

                if (double.TryParse(numb, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out var result))
                {
                    lastIndex = startIndex - 1;
                    return result;
                }

                throw new LocalizedException("error_geting_constant");
            }
        }

        private void CalcProcessIteration(ref Stack<double> operands, char operation)
        {
            if (operands.Count < 1)
            {
                throw new LocalizedException("calculation_error");
            }

            char unary_op;
            if (this.isUnary(operation, out unary_op))
            {
                double oper = operands.Pop();

                switch (unary_op)
                {
                    case '+':
                        operands.Push(oper);
                        break;
                    case '-':
                        operands.Push(-oper);
                        break;
                    case '–':
                        operands.Push(-oper);
                        break;
                }
            }
            else
            {
                double second = operands.Pop();
                double first = operands.Pop();

                switch (operation)
                {
                    case '+':
                        operands.Push(first + second);
                        break;
                    case '-':
                        operands.Push(first - second);
                        break;
                    case '–':
                        operands.Push(first - second);
                        break;
                    case '*':
                        operands.Push(first * second);
                        break;
                    case '/':
                        if (second == 0.0d) 
                        {
                            throw new LocalizedException("dividing_by_zero");
                        }
                        operands.Push(first / second);
                        break;
                    case '÷':
                        if (second == 0.0d)
                        {
                            throw new LocalizedException("dividing_by_zero");
                        }
                        operands.Push(first / second);
                        break;
                    case '^':
                        var result = Math.Pow(first, second);

                        if (double.IsNaN(result) || double.IsInfinity(result)) 
                        {
                            throw new LocalizedException("calculation_error");
                        }

                        operands.Push(result);
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

                if (funcCalcEq.VariablesValues?.Count > 0)
                {
                    for (int i = 0; i < funcCalcEq.VariablesValues.Count; i++)
                    {
                        funcCalcEq.VariablesValues[i].Value = this.VariablesValues.First(v => v.Index == funcCalcEq.VariablesValues[i].Index).Value;
                    }
                }

                double result = mathFunction(funcCalcEq.Calculate());

                if (double.IsNaN(result) || double.IsInfinity(result))
                {
                    throw new LocalizedException("calculation_error");
                }

                equationPart = equationPart.Replace(partToReplace, result.ToString().Replace(',', '.'));

                return equationPart;
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
            return equationPart;
        }

        private int GetPriority(char op)
        {
            if (op == '+' || op == '-' || op == '–')
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
             || o == '–'
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
        /// Removes all white spaces, checks whether input string is validate otherwise throws exception, and replace all variables like x{number=0..9} to one char symbol
        /// </summary>
        /// <param name="eq"></param>
        /// <returns></returns>
        private string Normalyze(string eq)
        {
            this.VariablesValues = new List<VarValue>();

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

            eq = eq.Replace(",", ".");

            if (eq.Contains("x"))
            {
                if (eq.Length == 1) 
                {
                    throw new LocalizedException("equation_not_valid_it_has_to_contains_variables_matching_pattern_X_(_number_0__9_)_");
                }

                for (int i = 0; i < eq.Length - 1; i++)
                {
                    if (eq[i] == 'x')
                    {
                        i++;
                        if (char.IsNumber(eq[i]) && int.TryParse(eq[i].ToString(), out var index))
                        {
                            if (!this.VariablesValues.Exists(v => v.Index == index))
                            {
                                this.VariablesValues.Add(new VarValue($"x{index}"));
                            }
                        }
                        else
                        {
                            throw new LocalizedException("equation_not_valid_it_has_to_contains_variables_matching_pattern_X_(_number_0__9_)_");
                        }
                    }
                }

                this.VariablesValues = this.VariablesValues.OrderBy(v => v.Index).ToList();
            }

            return eq;
        }

        /// <summary>
        /// Returns equation string.
        /// </summary>
        public string Display()
        {
            return this.equation;
        }
    }
}

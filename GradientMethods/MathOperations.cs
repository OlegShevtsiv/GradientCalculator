using GradientMethods.ExceptionResult;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GradientMethods
{
    public partial class GradientMethod
    {
        /// <summary>
        /// Method of finding a partial derivative
        /// </summary>
        /// <param name="function"></param>
        /// <param name="valuesOfVariables"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        static double df(Equation function, Dictionary<char, double> valuesOfVariables, char index)
        {
            if (valuesOfVariables == null)
            {
                throw new ArgumentNullException(nameof(valuesOfVariables));
            }
            const double step = 0.000001;
            if (!valuesOfVariables.ContainsKey(index))
            {
                throw new KeyNotFoundException();
            }

            Dictionary<char, double> X_h = new Dictionary<char, double>(valuesOfVariables);

            X_h[index] += step;

            return (function[X_h] - function[valuesOfVariables]) / step;
        }

        /// <summary>
        /// Jacobi matrix
        /// </summary>
        /// <param name="function"></param>
        /// <param name="valuesOfVariables"></param>
        /// <returns></returns>
        static Dictionary<char, double> Gradient(Equation function, Dictionary<char, double> valuesOfVariables)
        {
            if (valuesOfVariables == null)
            {
                throw new ArgumentNullException(nameof(valuesOfVariables));
            }

            Dictionary<char, double> gradient = new Dictionary<char, double>();


            foreach (var x in valuesOfVariables)
            {
                gradient.Add(x.Key, df(function, valuesOfVariables, x.Key));
            }

            return gradient;
        }

        static List<List<double>> GetInvertibleMatrix(List<List<double>> matrix)
        {
            if (matrix == null)
            {
                throw new ArgumentNullException(nameof(matrix));
            }
            if (matrix.Count != matrix[0].Count)
            {
                throw new ArgumentException("Matrix is not square!!!");
            }
            if (matrix.Count == 1)
            {
                return new List<List<double>> { new List<double> { 1.0 / matrix[0][0] } };
            }

            int n = matrix.Count;
            List<double> Koef = GetKoef(matrix);
            List<List<double>> E = new List<List<double>>(GetUnitMatrix(n));
            List<List<double>> OA = new List<List<double>>();
            for (int i = 0; i < n; i++)
            {
                OA.Add(new List<double>());
                for (int j = 0; j < n; j++)
                {
                    OA[i].Add(Koef[n - 2] * E[i][j]);
                }
            }

            List<List<double>> A1 = new List<List<double>>(matrix);

            for (int i = n - 2; i >= 0; i--)
            {
                for (int i1 = 0; i1 < n; i1++)
                {
                    for (int j1 = 0; j1 < n; j1++)
                    {
                        if (i > 0)
                        {
                            OA[i1][j1] += Koef[i - 1] * A1[i1][j1];
                        }
                        else
                        {
                            OA[i1][j1] = -(1 / Koef[n - 1]) * (A1[i1][j1] + OA[i1][j1]);
                        }
                    }
                }
                A1 = MultiplyMatrixes(A1, matrix);
            }

            List<List<double>> CheckResultMutrix = MultiplyMatrixes(OA, matrix);
            bool isCorrect = true;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (i == j)
                    {
                        if (Math.Round(CheckResultMutrix[i][j]).CompareTo(1.0d) != 0)
                        {
                            isCorrect = false;
                            break;
                        }
                    }
                    else if (Math.Round(CheckResultMutrix[i][j]).CompareTo(0.0d) != 0)
                    {
                        isCorrect = false;
                        break;
                    }
                }
            }

            if (!isCorrect)
            {
                throw new Exception("Сalculation error!");
            }

            return OA;
        }

        /// <summary>
        /// Gets matrix with all zero elements except diagonal, which initialized of '1'
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        static List<List<double>> GetUnitMatrix(int size)
        {
            List<List<double>> unit_matrix = new List<List<double>>();
            for (int i = 0; i < size; i++)
            {
                unit_matrix.Add(UnitVector(size, i));
            }
            return unit_matrix;
        }


        /// <summary>
        /// Coefficients of the characteristic polynomial
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        static List<double> GetKoef(List<List<double>> matrix)
        {
            if (matrix == null)
            {
                throw new ArgumentNullException(nameof(matrix));
            }
            if (matrix.Count != matrix[0].Count)
            {
                throw new ArgumentException("Matrix is not square!!!");
            }
            int n = matrix.Count;
            List<List<double>> AA = new List<List<double>>(matrix);
            double S;
            List<double> Sk = new List<double>();
            for (int k = 0; k < n; k++)
            {
                S = 0.0d;
                if (k > 0)
                {
                    AA = MultiplyMatrixes(AA, matrix);
                }
                for (int i = 0; i < n; i++)
                {
                    S += AA[i][i];
                }
                Sk.Add(S);
            }
            List<double> Koef = new List<double>();
            Koef.Add(-Sk.First());
            for (int i = 1; i < n; i++)
            {
                S = 0.0d;
                for (int j = 0; j < i; j++)
                {
                    S += Sk[j] * Koef[i - j - 1];
                }
                Koef.Add(-(Sk[i] + S) / (i + 1));
            }
            return Koef;
        }

        static List<List<double>> MultiplyMatrixes(List<List<double>> matrix_A, List<List<double>> matrix_B)
        {
            if (matrix_A == null)
            {
                throw new ArgumentNullException(nameof(matrix_A));
            }
            if (matrix_A.Count != matrix_A[0].Count)
            {
                throw new ArgumentException("Matrix is not square!!!");
            }
            if (matrix_B == null)
            {
                throw new ArgumentNullException(nameof(matrix_B));
            }
            if (matrix_B.Count != matrix_B[0].Count)
            {
                throw new ArgumentException("Matrix is not square!!!");
            }
            if (matrix_A[0].Count != matrix_B.Count)
            {
                throw new ArgumentException("Impossible to multply two matrixes!!!");
            }
            double S;
            int n = matrix_A.Count;
            List<List<double>> Result = new List<List<double>>();
            for (int i = 0; i < n; i++)
            {
                Result.Add(new List<double>());
                for (int j = 0; j < n; j++)
                {
                    S = 0.0d;
                    for (int k = 0; k < n; k++)
                    {
                        S += matrix_A[i][k] * matrix_B[k][j];
                    }
                    Result[i].Add(S);
                }
            }
            return Result;
        }



        static double MultiplyVectors(List<double> vector_A, List<double> vector_B)
        {
            if (vector_A == null || vector_B == null)
            {
                throw new ArgumentNullException();
            }
            if (vector_A.Count != vector_B.Count)
            {
                throw new ArgumentException("Range of vectors are not equal!!!");
            }
            int n = vector_A.Count;
            double Sum = 0.0d;
            for (int i = 0; i < n; i++)
            {
                Sum += vector_A[i] * vector_B[i];
            }
            return Sum;
        }

        static List<double> SumVectors(List<double> vactor_A, List<double> vector_B)
        {
            if (vactor_A == null || vector_B == null)
            {
                throw new ArgumentNullException();
            }
            if (vactor_A.Count != vector_B.Count)
            {
                throw new ArgumentException("Range of vectors are not equal!!!");
            }
            List<double> result = new List<double>();
            for (int i = 0; i < vactor_A.Count; i++)
            {
                result.Add(vactor_A[i] + vector_B[i]);
            }
            return result;
        }


        static List<double> SubtractVectors(List<double> vactor_A, List<double> vector_B)
        {
            if (vactor_A == null || vector_B == null)
            {
                throw new ArgumentNullException();
            }
            if (vactor_A.Count != vector_B.Count)
            {
                throw new ArgumentException("Range of vectors are not equal!!!");
            }
            List<double> result = new List<double>();
            for (int i = 0; i < vactor_A.Count; i++)
            {
                result.Add(vactor_A[i] - vector_B[i]);
            }
            return result;
        }

        /// <summary>
        /// Multiply vector on some number
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        static List<double> MultiplyVectorOnValue(List<double> vector, double number) // 
        {
            if (vector == null)
            {
                throw new ArgumentNullException(nameof(vector));
            }
            for (int i = 0; i < vector.Count; i++)
            {
                vector[i] *= number;
            }
            return vector;
        }

        /// <summary>
        /// Gets vector with all zero elements except element on specified index
        /// </summary>
        /// <param name="sizeOfVector"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        static List<double> UnitVector(int sizeOfVector, int index)
        {
            List<double> unit_vector = new List<double>();
            for (int i = 0; i < sizeOfVector; i++)
            {
                if (i == index)
                {
                    unit_vector.Add(1.0d);
                }
                else
                {
                    unit_vector.Add(0.0d);
                }
            }
            return unit_vector;
        }

        /// <summary>
        /// matrix of Hesse
        /// </summary>
        /// <param name="function"></param>
        /// <param name="valuesOfVariables"></param>
        /// <returns></returns>
        static List<List<double>> Hessian(Equation function, Dictionary<char, double> valuesOfVariables) 
        {
            if (function == null)
            {
                throw new ArgumentNullException(nameof(function));
            }
            if (valuesOfVariables == null)
            {
                throw new ArgumentNullException(nameof(valuesOfVariables));
            }
            List<List<double>> hessian = new List<List<double>>(valuesOfVariables.Count);
            for (int i = 0; i < valuesOfVariables.Count; i++)
            {
                hessian.Add(new List<double>(valuesOfVariables.Count));
                for (int j = 0; j < valuesOfVariables.Count; j++)
                {
                    hessian[i].Add(0.0d);
                }
            }

            double step = 0.00001;
            double curFValue = function[valuesOfVariables];

            List<double> fPlus = new List<double>(valuesOfVariables.Count);
            List<double> fMinus = new List<double>(valuesOfVariables.Count);

            List<double> tempList;
            int index = 0;

            for (int i = 0; i < valuesOfVariables.Count; i++)
            {
                index = 0;
                tempList = SumVectors(valuesOfVariables.Values.ToList(), MultiplyVectorOnValue(UnitVector(valuesOfVariables.Count, i), step));
                fPlus.Add(function[valuesOfVariables.Select(x => new KeyValuePair<char, double>(key: x.Key, value: tempList[index++]))]);

                index = 0;
                tempList = SubtractVectors(valuesOfVariables.Values.ToList(), MultiplyVectorOnValue(UnitVector(valuesOfVariables.Count, i), step));
                fMinus.Add(function[valuesOfVariables.Select(x => new KeyValuePair<char, double>(key: x.Key, value: tempList[index++]))]);
                hessian[i][i] = (fPlus[i] - 2 * curFValue + fMinus[i]) / (step * step);
            }

            for (int i = 0; i < valuesOfVariables.Count; i++)
            {
                for (int j = i + 1; j < valuesOfVariables.Count; j++)
                {
                    index = 0;
                    tempList = SumVectors(valuesOfVariables.Values.ToList(), MultiplyVectorOnValue(SumVectors(UnitVector(valuesOfVariables.Count, i), UnitVector(valuesOfVariables.Count, j)), step));
                    double x = function[valuesOfVariables.Select(x1 => new KeyValuePair<char, double>(key: x1.Key, value: tempList[index++]))];
                    hessian[i][j] = (x - fPlus[i] - fPlus[j] + curFValue) / (step * step);
                }
            }

            for (int i = 0; i < valuesOfVariables.Count; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    hessian[i][j] = hessian[j][i];
                }
            }
            return hessian;
        }
    }
}
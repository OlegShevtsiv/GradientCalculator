using System;
using System.Collections.Generic;
using System.Linq;

namespace GradientMethods
{
    public partial class GradientMethod
    {
        static double df(Equation F, Dictionary<char, double> X, char i) // method of finding a partial derivative
        {
            if (X == null)
            {
                throw new Exception("Matrix is empty!!!");
            }
            const double step = 0.000001;
            if (!X.ContainsKey(i))
            {
                throw new KeyNotFoundException();
            }

            Dictionary<char, double> X_h = new Dictionary<char, double>(X);

            X_h[i] += step;

            return (F[X_h] - F[X]) / step;
        }

        static Dictionary<char, double> Gradient(Equation F, Dictionary<char, double> X) // Jacobi matrix
        {
            if (X == null)
            {
                throw new Exception("Matrix is empty!!!");
            }

            Dictionary<char, double> grad = new Dictionary<char, double>();


            foreach (var x in X)
            {
                grad.Add(x.Key, df(F, X, x.Key));
            }

            return grad;
        }

        static List<List<double>> GetInvertibleMatrix(List<List<double>> A)// get invertible matrix 
        {
            if (A == null)
            {
                throw new Exception("Matrix is empty!!!");
            }
            if (A.Count != A[0].Count)
            {
                throw new Exception("Matrix is not square!!!");
            }
            if (A.Count == 1)
            {
                return new List<List<double>> { new List<double> { 1.0 / A[0][0] } };
            }

            int n = A.Count;
            List<double> Koef = GetKoef(A);
            List<List<double>> E = new List<List<double>>(UnitMatrix(n));
            List<List<double>> OA = new List<List<double>>();
            for (int i = 0; i < n; i++)
            {
                OA.Add(new List<double>());
                for (int j = 0; j < n; j++)
                {
                    OA[i].Add(Koef[n - 2] * E[i][j]);
                }
            }

            List<List<double>> A1 = new List<List<double>>(A);

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
                A1 = MultMatrixes(A1, A);
            }

            List<List<double>> CheckResultMutrix = MultMatrixes(OA, A);
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
                throw new Exception("Incorrect calculating!!!");
            }

            return OA;
        }

        static List<List<double>> UnitMatrix(int size)
        {
            List<List<double>> unit_matrix = new List<List<double>>();
            for (int i = 0; i < size; i++)
            {
                unit_matrix.Add(UnitVector(size, i));
            }
            return unit_matrix;
        }

        static List<double> GetKoef(List<List<double>> A) // coefficients of the characteristic polynomial
        {
            if (A == null)
            {
                throw new Exception("Matrix is empty!!!");
            }
            if (A.Count != A[0].Count)
            {
                throw new Exception("Matrix is not square!!!");
            }
            int n = A.Count;
            List<List<double>> AA = new List<List<double>>(A);
            double S;
            List<double> Sk = new List<double>();
            for (int k = 0; k < n; k++)
            {
                S = 0.0d;
                if (k > 0)
                {
                    AA = MultMatrixes(AA, A);
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

        static List<List<double>> MultMatrixes(List<List<double>> A, List<List<double>> B)
        {
            if (A == null)
            {
                throw new Exception("Matrix is empty!!!");
            }
            if (A.Count != A[0].Count)
            {
                throw new Exception("Matrix is not square!!!");
            }
            if (B == null)
            {
                throw new Exception("Matrix is empty!!!");
            }
            if (B.Count != B[0].Count)
            {
                throw new Exception("Matrix is not square!!!");
            }
            if (A[0].Count != B.Count)
            {
                throw new Exception("Impossible to multply two matrixes!!!");
            }
            double S;
            int n = A.Count;
            List<List<double>> Result = new List<List<double>>();
            for (int i = 0; i < n; i++)
            {
                Result.Add(new List<double>());
                for (int j = 0; j < n; j++)
                {
                    S = 0.0d;
                    for (int k = 0; k < n; k++)
                    {
                        S += A[i][k] * B[k][j];
                    }
                    Result[i].Add(S);
                }
            }
            return Result;
        }

        static double MultVec(List<double> A, List<double> B)
        {
            if (A == null || B == null)
            {
                throw new Exception("Vector is empty!!!");
            }
            if (A.Count != B.Count)
            {
                throw new Exception("Range of vectors are not equal!!!");
            }
            int n = A.Count;
            double Sum = 0.0d;
            for (int i = 0; i < n; i++)
            {
                Sum += A[i] * B[i];
            }
            return Sum;
        }

        static List<double> AddVec(List<double> X1, List<double> X2)
        {
            if (X1 == null || X2 == null)
            {
                throw new Exception("Vector is empty!!!");
            }
            if (X1.Count != X2.Count)
            {
                throw new Exception("Range of vectors are not equal!!!");
            }
            List<double> A = new List<double>();
            for (int i = 0; i < X1.Count; i++)
            {
                A.Add(X1[i] + X2[i]);
            }
            return A;
        }

        static List<double> SubVec(List<double> X1, List<double> X2)
        {
            if (X1 == null || X2 == null)
            {
                throw new Exception("Vector is empty!!!");
            }
            if (X1.Count != X2.Count)
            {
                throw new Exception("Range of vectors are not equal!!!");
            }
            List<double> S = new List<double>();
            for (int i = 0; i < X1.Count; i++)
            {
                S.Add(X1[i] - X2[i]);
            }
            return S;
        }

        static List<double> MultVecOnVal(List<double> X, double val) // multiply vector on some value
        {
            if (X == null)
            {
                throw new Exception("Vector is empty!!!");
            }
            for (int i = 0; i < X.Count; i++)
            {
                X[i] *= val;
            }
            return X;
        }

        static List<double> UnitVector(int size, int index)
        {
            List<double> unit_vector = new List<double>();
            for (int i = 0; i < size; i++)
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


        static List<List<double>> Hessian(Equation F, Dictionary<char, double> X) // matrix of Hesse
        {
            if (F == null)
            {
                throw new Exception("Unknown function!!!");
            }
            if (X == null)
            {
                throw new Exception("Vector is empty!!!");
            }
            List<List<double>> hessian = new List<List<double>>(X.Count);
            for (int i = 0; i < X.Count; i++)
            {
                hessian.Add(new List<double>(X.Count));
                for (int j = 0; j < X.Count; j++)
                {
                    hessian[i].Add(0.0d);
                }
            }

            double step = 0.00001;
            double curFValue = F[X];

            List<double> fPlus = new List<double>(X.Count);
            List<double> fMinus = new List<double>(X.Count);

            List<double> tempList;
            int index = 0;

            for (int i = 0; i < X.Count; i++)
            {
                index = 0;
                tempList = AddVec(X.Values.ToList(), MultVecOnVal(UnitVector(X.Count, i), step));
                fPlus.Add(F[X.Select(x => new KeyValuePair<char, double>(key: x.Key, value: tempList[index++]))]);

                index = 0;
                tempList = SubVec(X.Values.ToList(), MultVecOnVal(UnitVector(X.Count, i), step));
                fMinus.Add(F[X.Select(x => new KeyValuePair<char, double>(key: x.Key, value: tempList[index++]))]);
                hessian[i][i] = (fPlus[i] - 2 * curFValue + fMinus[i]) / (step * step);
            }

            for (int i = 0; i < X.Count; i++)
            {
                for (int j = i + 1; j < X.Count; j++)
                {
                    index = 0;
                    tempList = AddVec(X.Values.ToList(), MultVecOnVal(AddVec(UnitVector(X.Count, i), UnitVector(X.Count, j)), step));
                    double x = F[X.Select(x1 => new KeyValuePair<char, double>(key: x1.Key, value: tempList[index++]))];
                    hessian[i][j] = (x - fPlus[i] - fPlus[j] + curFValue) / (step * step);
                }
            }

            for (int i = 0; i < X.Count; i++)
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
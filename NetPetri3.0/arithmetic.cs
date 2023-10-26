using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetPetri3._0
{
   
    public class arithmetic
    {
        public static int[,] subtract_matrix(int[,] matrixA, int[,] matrixB) //Вычитание матриц
        {
            int rows = matrixA.GetLength(0);
            int columns = matrixA.GetLength(1);
            int[,] result = new int[rows, columns];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++) result[i, j] = matrixA[i, j] - matrixB[i, j];
            }
            return result;
        }
        public static int[] sum_vector(int[] v1, int[] v2) //Поэлементная сумма векторов
        {
            int n = v1.Length;
            int[] result = new int[n];
            for (int i = 0; i < n; i++) result[i] = v1[i] + v2[i];
            return result;
        }
        public static bool compare_mark(int[] mark1, int[] mark2)
        {
            for (int i = 0; i < mark1.Length; i++) if (mark1[i] < mark2[i]) return false;
            return true;
        }
        public static int[] multiplication(int[] vector1, int[,] matrix) //перемножение матриц
        {
            int m = vector1.Length;
            int n = matrix.GetLength(1);
            int[] result = new int[m];
            for (int i = 0; i < n; i++) for (int j = 0; j < m; j++) result[i] += vector1[j] * matrix[j, i];
            return result;
        }
        public static double[,] TransposeMatrix(double [,] matrix) //транспонирование
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);
            double [,] transposedMatrix = new double[cols, rows];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    transposedMatrix[j, i] = matrix[i, j];
                }
            }

            return transposedMatrix;
        }
        private static double[,] GaussElimination(double[,] matrix) //приведение матрицы к Гауссовскому виду с исключением нулевой строки
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);

            for (int i = 0; i < rows; i++)
            {
                // Поиск строки с ненулевым первым элементом
                if (matrix[i, i] == 0)
                {
                    // Поиск ненулевой строки ниже текущей
                    for (int j = i + 1; j < rows; j++)
                    {
                        if (matrix[j, i] != 0)
                        {
                            // Обмен строками
                            for (int k = 0; k < cols; k++)
                            {
                                double temp = matrix[i, k];
                                matrix[i, k] = matrix[j, k];
                                matrix[j, k] = temp;
                            }
                            break;
                        }
                    }
                }

                // Приведение текущей строки к виду, в котором первый элемент равен 1
                double divisor = matrix[i, i];
                if (divisor != 0)
                {
                    for (int j = i; j < cols; j++)
                    {
                        matrix[i, j] /= divisor;
                    }
                }

                // Обнуление элементов в текущем столбце под главной диагональю
                for (int j = 0; j < rows; j++)
                {
                    if (j != i)
                    {
                        double factor = matrix[j, i];
                        for (int k = i; k < cols; k++)
                        {
                            matrix[j, k] -= factor * matrix[i, k];
                        }
                    }
                }
            }

            // Удаление нулевых строк
            for (int i = rows - 1; i >= 0; i--)
            {
                bool isZeroRow = true;
                for (int j = 0; j < cols; j++)
                {
                    if (matrix[i, j] != 0)
                    {
                        isZeroRow = false;
                        break;
                    }
                }

                if (isZeroRow)
                {
                    // Сдвиг всех строк выше текущей на одну позицию вниз
                    for (int j = i; j < rows - 1; j++)
                    {
                        for (int k = 0; k < cols; k++)
                        {
                            matrix[j, k] = matrix[j + 1, k];
                        }
                    }

                    rows--;
                }
            }
            double[,] converted_matrix = new double[rows, cols];

            for (int i = 0; i < rows; i++) for (int j = 0; j < cols; j++) converted_matrix[i, j] = matrix[i, j];
            return converted_matrix;
        }
        public static bool solved_SLofE(double[,] m) //вычисления имеет ли СЛАУ решения
        {
            double[,] matrix = GaussElimination(m);
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);

            // Приведение матрицы к ступенчатому виду
            for (int i = 0; i < rows; i++)
            {
                // Поиск строки с ненулевым i-тым элементом
                if (matrix[i, i] == 0) return false;

                // Приведение i-той строки к виду, в котором i-тый элемент равен 1
                double divisor = matrix[i, i];
                for (int j = i; j < cols; j++)
                {
                    matrix[i, j] /= divisor;
                }

                // Обнуление элементов в столбце под i-тым элементом
                for (int j = i + 1; j < rows; j++)
                {
                    double factor = matrix[j, i];
                    for (int k = i; k < cols; k++)
                    {
                        matrix[j, k] -= factor * matrix[i, k];
                    }
                }
            }

            // Обратный ход метода Гаусса (поиск решения)
            double[] solutions = new double[rows];
            for (int i = rows - 1; i >= 0; i--)
            {
                solutions[i] = matrix[i, cols - 1];
                for (int j = i + 1; j < cols - 1; j++)
                {
                    solutions[i] -= matrix[i, j] * solutions[j];
                }
            }
            for (int i = 0; i < rows; i++) if (solutions[i] < 0) return false;
            return true;
        }
    }
}

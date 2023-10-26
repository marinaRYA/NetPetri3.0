using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetPetri3._0
{
    internal class ReachabilityValidator : arithmetic
    {
        
        int[] check_mark;
        int [,] Dmatrix;
        int size_mark;
       
        
        public ReachabilityValidator(List<List<int>> Dplus, List<List<int>> Dminus, List<List<int>> init_m, List<List<int>> fin_m) {
            int m = Dplus.Count;
            int n = Dplus[0].Count;
            int [,] Dminusmatrix = new int[m, n];
            int[,] Dplusmatrix = new int[m, n];
            Dmatrix = new int [m, n];
            check_mark=new int[m];
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    Dminusmatrix[i, j] = Dminus[i][j];
                    Dplusmatrix[i, j] = Dplus[i][j];
                }
            }
            for (int j = 0; j < m; j++) check_mark[j] = fin_m[0][j]-init_m[0][j];
            Dmatrix = subtract_matrix(Dplusmatrix, Dminusmatrix);
            size_mark = m;
        }
        public bool check_reach() //проверка искомой маркировки
        {
            List<int[]> sys_equation = new List<int[]>();
            for (int i = 0; i< Dmatrix.GetLength(1); i++) {
                int[] equation = new int[size_mark];
                for (int j = 0; j < Dmatrix.GetLength(0); j++) equation[j] = Dmatrix[j, i];
                sys_equation.Add(equation);
            }
            int numRows = sys_equation.Count;
            int numCols = sys_equation[0].Length; // добавляем один столбец для свободных членов

            double[,] m = new double[numRows, numCols];
            for (int i = 0; i < numRows; i++)
            {
                for (int j = 0; j < numCols; j++) m[i, j] = sys_equation[i][j];              
            }
            double [,] m1 = TransposeMatrix(m);
            numRows = m1.GetLength(0);
            numCols = m1.GetLength(1) + 1;
            double[,] matrix = new double[numRows, numCols];
            for (int i = 0; i < numRows; i++)
            {
                for (int j = 0; j < numCols - 1; j++)
                {
                    matrix[i, j] = m1[i,j];
                }
                // Последний столбец - свободные члены
                matrix[i, numCols - 1] = check_mark[i];
            }
            return solved_SLofE(matrix);

        }
    }
}

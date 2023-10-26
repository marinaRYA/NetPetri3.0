using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace NetPetri3._0
{
    internal class PetriNetReachabilityTreeBuilder : arithmetic
    {

        int[] init_mark;
        int[,] Dmatrix;
        int[,] Dminusmatrix;
        List<int[]> all_transition;
        Dictionary<int[], int[]> condition_trans = new Dictionary<int[], int[]>();
        Dictionary<int[], int[]> activation_trans = new Dictionary<int[], int[]>();
        List<Data> data = new List<Data>();
        int count_trans;
        
        public PetriNetReachabilityTreeBuilder(List<List<int>> Dplus, List<List<int>> Dminus, List<List<int>> init_m)
        {
            
                int m = Dplus.Count;
                int n = Dplus[0].Count;
                Dminusmatrix = new int[m, n];
                int[,] Dplusmatrix = new int[m, n];
                Dmatrix = new int[m, n];
                init_mark = new int[m];
                for (int i = 0; i < m; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        Dminusmatrix[i, j] = Dminus[i][j];
                        Dplusmatrix[i, j] = Dplus[i][j];
                    }
                }
                for (int j = 0; j < m; j++) init_mark[j] = init_m[0][j];
                Dmatrix = subtract_matrix(Dplusmatrix, Dminusmatrix); //иницилизация матрицы инцидентности

                all_transition = new List<int[]>(); //формирование векторов всех возможных переходов
                for (int i = 0; i < m; i++)
                {
                    int[] active_tran = new int[m];
                    active_tran[i] = 1;
                    all_transition.Add(active_tran);
                }
                count_trans = m;
        }
       
      
        void act_transition() //получения вектора условия перехода и самого перехода
        {
            for (int i = 0; i<count_trans; i++)
            {
                condition_trans[all_transition[i]] = multiplication(all_transition[i],Dminusmatrix);
                activation_trans[all_transition[i]] = multiplication(all_transition[i], Dmatrix);
            }    
        }
        public List<Data> filling_for_tree(int p_depth, int [] mark, string path) 
        {
            if (p_depth == 0) return data;//Глубина обхода дерева
            if (mark != null)
            {
                for (int i = 0; i < count_trans; i++)
                {
                    if (compare_mark(mark, condition_trans[all_transition[i]])) //Проверка выполности перехода
                    {
                        int[] new_mark = sum_vector(mark, activation_trans[all_transition[i]]);
                        path =path + "t" + (i+1).ToString(); //сохранение пути до искомого перехода
                        string mark_str = string.Join("", Array.ConvertAll(new_mark, x => x.ToString()));
                        Data d = new Data(mark_str, path);
                        data.Add(d);
                        filling_for_tree(p_depth - 1, new_mark, path);
                    }
                }
                return data;
            }
            return null;

        }
        public List<Data> build_tree(int deep) 
        {
            act_transition();
            string mark_str = string.Join("", Array.ConvertAll(init_mark, x => x.ToString()));
            Data d = new Data(mark_str, "t0");
            filling_for_tree(deep, init_mark, "");
            return data;
        }
    }
}

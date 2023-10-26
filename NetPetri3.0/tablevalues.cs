using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace NetPetri3._0
{
    internal class tablevalues 
    {
        List<List<string>> values;
        public List<List<int>> number = new List<List<int>>();
        public bool correct_val;
        public tablevalues(DataGrid DG)
        {
            values = DG.ItemsSource as List<List<string>>;       
            if(values != null) correct_val = string_to_int();   
            else correct_val = false;
        }
        private bool string_to_int() //преобразования данных таблицы в целочисленную матрицу
        {
           
            foreach (var row in values)
            {
                int i = 0;
                List<int> intRow = new List<int>();
                foreach (var str in row)
                {
                    if (int.TryParse(str, out int num) && num >= 0)
                    {
                       if (i!=0) intRow.Add(num);
                        i++;
                    }
                    else return false;
                  
                }
                number.Add(intRow);
            }
            return true;
        }
       
    }
}
 
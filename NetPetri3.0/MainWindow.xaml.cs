using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NetPetri3._0
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        
        public void createGrid(DataGrid dataGrid, int sign, int rowCount, int columnCount)
        {
            List<List<string>> matrix = new List<List<string>>();
            dataGrid.Columns.Clear();
            DataGridTextColumn rowHeaderColumn = new DataGridTextColumn();
            if (sign == 1) rowHeaderColumn.Header = "D+";
            else if (sign == 0 ) rowHeaderColumn.Header = "D-";
            else rowHeaderColumn.Header = "-";
            rowHeaderColumn.Binding = new System.Windows.Data.Binding("[0]");
            dataGrid.Columns.Add(rowHeaderColumn);

            for (int i = 1; i <= columnCount; i++) //создаем остальные столбцы помимо названия матрицы
            {
                DataGridTextColumn column = new DataGridTextColumn();
                column.Header = i.ToString();
                column.Binding = new System.Windows.Data.Binding("[" + i + "]");
                dataGrid.Columns.Add(column);
            }

            // Создаем и заполняем матрицу для значений
            for (int i = 0; i < rowCount; i++)
            {
                List<string> row = new List<string>();
                row.Add((i + 1).ToString()); // Номер строки
                for (int j = 1; j <= columnCount; j++) row.Add("0");
                matrix.Add(row);
            }

            dataGrid.ItemsSource = matrix;
            DataGridTextColumn firstColumn = dataGrid.Columns[0] as DataGridTextColumn;
            firstColumn.IsReadOnly = true; // первый стоблик для обозначения номера перехода/позиций делаем его только для чтения
           
        }
        public MainWindow()
        {
            InitializeComponent();
           // InitializeDataGrids();
        }

        private void ApplyD_Click(object sender, RoutedEventArgs e) //кнопка применить
        {

            if (int.TryParse(colnum.Text, out int rowCount) && int.TryParse(linenum.Text, out int columnCount) && (columnCount > 0) && (rowCount > 0) && (columnCount < 11) && (rowCount <11))
            {
                createGrid(Dplus, 1,  columnCount, rowCount );
                createGrid(Dminus, 0, columnCount, rowCount);
                createGrid(init_mark, -1, 1, columnCount);
                createGrid(final_mark, -1, 1, columnCount);
            }
            else MessageBox.Show("Введите корректное количество строк и столбцов.");

        }



        private void Count_Click(object sender, RoutedEventArgs e) //построение всех возможных маркировок
        {
            ApplyCount.IsEnabled = false;

                tablevalues dplus = new tablevalues(Dplus); 
                tablevalues dminus = new tablevalues(Dminus);
                tablevalues in_mark = new tablevalues(init_mark);
                if (!(int.TryParse(Deep.Text, out int deep))) deep = -1;

                if (dplus.correct_val == false || dminus.correct_val == false || in_mark.correct_val == false || deep > 5 || deep < 0 || deep == -1) MessageBox.Show("Введите корректные значения.");
                else
                {
                    PetriNetReachabilityTreeBuilder tree = new PetriNetReachabilityTreeBuilder(dplus.number, dminus.number, in_mark.number);
                    listView.ItemsSource = tree.build_tree(deep);
                    listView.Items.Refresh();
                }
            
            
            ApplyCount.IsEnabled = true;
        }
        private void Check_Click(object sender, RoutedEventArgs e) //проверка возможности искомой маркровки 
        {
           
            tablevalues dplus = new tablevalues(Dplus);
            tablevalues dminus = new tablevalues(Dminus);
            tablevalues in_mark = new tablevalues(init_mark);
            tablevalues fin_mark = new tablevalues(final_mark);
            if (dplus.correct_val == false || dminus.correct_val == false || in_mark.correct_val == false || fin_mark.correct_val == false) MessageBox.Show("Введите корректные значения.");
            else
            {
                ReachabilityValidator validator = new ReachabilityValidator(dplus.number, dminus.number, in_mark.number, fin_mark.number);
               
                if (validator.check_reach()) MessageBox.Show("Искомая маркировка возможна");
                else MessageBox.Show("Искомая маркировка невозможна"); 
            }
        }
    }
}
    
 

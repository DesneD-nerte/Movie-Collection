using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Movie_Collection.View
{
    /// <summary>
    /// Логика взаимодействия для AllGenresView.xaml
    /// </summary>
    public partial class AllGenresView : UserControl
    {
        public AllGenresView()
        {
            InitializeComponent();
        }

        private void MainDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MainDataGrid.SelectedItem != null)
            {
                MainDataGrid.ScrollIntoView(MainDataGrid.SelectedItem);
            }
        }
    }
}

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TyresDb.ViewModels;

namespace TyresDb.Views
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowVm();
        }

        private void tyresGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            if (e.Row != null)
            {
                if (e.Row.GetIndex() % 2 == 0)
                    e.Row.Background = new SolidColorBrush(Colors.Gainsboro);
                else
                    e.Row.Background = new SolidColorBrush(Colors.White);
            }
        }
    }
}

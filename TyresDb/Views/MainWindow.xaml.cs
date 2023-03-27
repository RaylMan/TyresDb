using System.Windows;
using System.Windows.Controls;
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
    }
}

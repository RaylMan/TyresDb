using System;
using System.Windows;
using TyresDb.Model;
using TyresDb.Types;
using TyresDb.ViewModels;

namespace TyresDb.Views
{
    /// <summary>
    /// Логика взаимодействия для ChangeWeightWindow.xaml
    /// </summary>
    public partial class ChangeWeightWindow : Window
    {
        public ChangeWeightWindow(
            WindowType windowType,
            ITyresRepository tyresRepository, 
            Tyre tyre, 
            Action fillTableAction)
        {
            InitializeComponent();
            var vm = new ChangeTyreWindowVm(windowType, tyresRepository, tyre, this, fillTableAction);
            DataContext = vm;
        }
    }
}

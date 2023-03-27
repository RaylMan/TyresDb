using System;
using System.Windows;
using TyresDb.Model;
using TyresDb.ViewModels;

namespace TyresDb.Views
{
    /// <summary>
    /// Логика взаимодействия для ChangeWeightWindow.xaml
    /// </summary>
    public partial class ChangeWeightWindow : Window
    {
        public ChangeWeightWindow(ITyresRepository tyresRepository, Tyre tyre, Action fillTableAction)
        {
            InitializeComponent();
            DataContext = new ChangeWeightWindowVm(tyresRepository, tyre, this, fillTableAction);
        }
    }
}

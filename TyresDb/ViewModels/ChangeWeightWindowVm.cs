using NLog;
using System;
using System.Windows;
using System.Windows.Input;
using TyresDb.Model;
using TyresDb.Views;

namespace TyresDb.ViewModels
{
    internal class ChangeWeightWindowVm : VievModelBase
    {
        #region Fields and Properties
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private readonly ITyresRepository tyresRepository;
        private readonly Tyre tyre;
        private readonly ChangeWeightWindow changeWeightWindow;
        private readonly Action fillTableAction;

        private string weight;
        public string Weight
        {
            get { return weight; }
            set
            {
                if (value.IsTextAllowed())
                    weight = value;

                RaisedPropertyChanged("Weight");
            }
        }
        #endregion

        #region Constructor
        public ChangeWeightWindowVm(
           ITyresRepository tyresRepository,
           Tyre tyre,
           ChangeWeightWindow changeWeightWindow,
           Action fillTableAction)
        {
            this.tyresRepository = tyresRepository;
            this.tyre = tyre;
            this.changeWeightWindow = changeWeightWindow;
            this.fillTableAction = fillTableAction;
            Weight = tyre?.Weight.ToString();
        }
        #endregion

        #region ChangeWeightCommand
        private ICommand changeWeightCommand;
        public ICommand ChangeWeightCommand
        {
            get
            {
                if (changeWeightCommand == null)
                {
                    changeWeightCommand = new RelayCommand(new Action<object>(ChangeWeight));
                }
                return changeWeightCommand;
            }
            set
            {
                changeWeightCommand = value;
                RaisedPropertyChanged("ChangeWeightCommand");
            }
        }

        public void ChangeWeight(object o)
        {
            try
            {
                if (string.IsNullOrEmpty(weight))
                {
                    MessageBox.Show("Необходимо указать вес.", "Ошибка");
                    return;
                }
                Weight = Weight.Replace('.', ',');
                if (double.TryParse(Weight, out double newWeight))
                {
                    tyre.Weight = newWeight;
                    tyresRepository.Save();
                    fillTableAction();
                    changeWeightWindow.Close();
                }
                else
                {
                    MessageBox.Show("Необходимо указать число.", "Ошибка");
                    return;
                }

            }
            catch (Exception e)
            {
                logger.Error(e.ToString());
            }
        }
        #endregion

        #region KeyDownEvent

        private ICommand enterKeyDownCommand;
        public ICommand EnterKeyDownCommand
        {
            get
            {
                if (enterKeyDownCommand == null)
                {
                    enterKeyDownCommand = new RelayCommand(new Action<object>(ChangeWeight));
                }
                return enterKeyDownCommand;
            }
            set
            {
                enterKeyDownCommand = value;
                RaisedPropertyChanged("EnterKeyDownCommand");
            }
        }
        #endregion
    }
}

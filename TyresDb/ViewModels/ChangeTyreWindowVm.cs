using NLog;
using System;
using System.Windows;
using System.Windows.Input;
using TyresDb.Model;
using TyresDb.Types;
using TyresDb.Views;

namespace TyresDb.ViewModels
{
    internal class ChangeTyreWindowVm : VievModelBase
    {
        #region Fields and Properties
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private readonly WindowType windowType;
        private readonly ITyresRepository tyresRepository;
        private readonly Tyre tyre;
        private readonly ChangeWeightWindow changeWeightWindow;
        private readonly Action fillTableAction;

        private string windowName;
        public string WindowName
        {
            get { return windowName; }
            set
            {
                windowName = value;
                RaisedPropertyChanged("WindowName");
            }
        }

        private string weight;
        public string Weight
        {
            get { return weight; }
            set
            {
                if (value.IsDigit())
                    weight = value;

                RaisedPropertyChanged("Weight");
            }
        }

        private string aspectRatio;
        public string AspectRatio
        {
            get { return aspectRatio; }
            set
            {
                if (value.IsDigit())
                    aspectRatio = value;

                RaisedPropertyChanged("AspectRatio");
            }
        }

        private string diameter;
        public string Diameter
        {
            get { return diameter; }
            set
            {
                if (value.IsDigit())
                    diameter = value;
                RaisedPropertyChanged("Diameter");
            }
        }

        private string width;
        public string Width
        {
            get { return width; }
            set
            {
                if (value.IsDigit())
                    width = value;
                RaisedPropertyChanged("Width");
            }
        }

        private string resutDiameter;
        public string ResutDiameter
        {
            get { return resutDiameter; }
            set
            {
                if (value.IsDigit())
                    resutDiameter = value;
                RaisedPropertyChanged("ResutDiameter");
            }
        }

        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                RaisedPropertyChanged("Name");
            }
        }

        private string season;
        public string Season
        {
            get { return season; }
            set
            {
                season = value;
                RaisedPropertyChanged("Season");
            }
        }
        #endregion

        #region Constructor
        public ChangeTyreWindowVm(
           WindowType windowType,
           ITyresRepository tyresRepository,
           Tyre tyre,
           ChangeWeightWindow changeWeightWindow,
           Action fillTableAction)
        {
            this.windowType = windowType;
            this.tyresRepository = tyresRepository;
            this.tyre = tyre;
            this.changeWeightWindow = changeWeightWindow;
            this.fillTableAction = fillTableAction;

            Width = tyre.Width.ToString();
            AspectRatio = tyre.AspectRatio.ToString();
            Diameter = tyre.Diameter.ToString();
            Weight = tyre.Weight.ToString();
            Name = tyre.Name;
            Season = tyre.Season;

            switch (windowType)
            {
                case WindowType.Create:
                    WindowName = "Создать";
                    break;
                case WindowType.Update:
                    WindowName = "Изменить";
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region SaveCommand
        private ICommand saveCommand;
        public ICommand SaveCommand
        {
            get
            {
                if (saveCommand == null)
                {
                    saveCommand = new RelayCommand(new Action<object>(Save));
                }
                return saveCommand;
            }
            set
            {
                saveCommand = value;
                RaisedPropertyChanged("SaveCommand");
            }
        }

        public void Save(object o)
        {
            try
            {
                tyre.Width = Width.GetDoubleFromString();
                tyre.AspectRatio = AspectRatio.GetDoubleFromString();
                tyre.Diameter = Diameter.GetDoubleFromString();
                tyre.Weight = Weight.GetDoubleFromString();
                tyre.Name = Name;
                tyre.Season = Season;

                if(!tyre.Validate(out var error)) 
                {
                    MessageBox.Show(error);
                    return;
                }

                if(windowType == WindowType.Create)
                    tyresRepository.Tyres.Add(tyre);

                tyresRepository.Save();
                fillTableAction();
                changeWeightWindow.Close();
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
                    enterKeyDownCommand = new RelayCommand(new Action<object>(Save));
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

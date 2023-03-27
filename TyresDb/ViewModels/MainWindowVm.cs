using NLog;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using TyresDb.Model;
using TyresDb.Views;

namespace TyresDb.ViewModels
{
    internal class MainWindowVm : VievModelBase
    {
        #region Fields and properties
        private event Action tyrePropertyChanged;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private ITyresRepository tyresRepository;
        private ObservableCollection<Tyre> tyres = new ObservableCollection<Tyre>();
        public ObservableCollection<Tyre> Tyres
        {
            get { return tyres; }

            set
            {
                tyres = value;
                RaisedPropertyChanged("Tyres");
            }
        }



        private string aspectRatio;
        public string AspectRatio
        {
            get { return aspectRatio; }
            set
            {
                if (value.IsTextAllowed())
                    aspectRatio = value;

                tyrePropertyChanged();
                RaisedPropertyChanged("AspectRatio");
            }
        }

        private string diameter;
        public string Diameter
        {
            get { return diameter; }
            set
            {
                if (value.IsTextAllowed())
                    diameter = value;

                tyrePropertyChanged();
                RaisedPropertyChanged("Diameter");
            }
        }

        private string width;
        public string Width
        {
            get { return width; }
            set
            {
                if (value.IsTextAllowed())
                    width = value;

                tyrePropertyChanged();
                RaisedPropertyChanged("Width");
            }
        }

        private string resutDiameter;
        public string ResutDiameter
        {
            get { return resutDiameter; }
            set
            {
                resutDiameter = value;
                RaisedPropertyChanged("ResutDiameter");
            }
        }

        private Tyre selecterdRow;
        public Tyre SelectedRow
        {
            get { return selecterdRow; }
            set
            {
                selecterdRow = value;
                RaisedPropertyChanged("SelectedRow");
            }
        }
        #endregion

        #region Constructor
        public MainWindowVm()
        {
            tyresRepository = TyresRepositoryFactory.Create(out var errors);

            if (!string.IsNullOrWhiteSpace(errors))
            {
                MessageBox.Show($"Ошибка загрузки файла: {errors}", "Ошибка загрузки");
                logger.Error($"Ошибка загрузки БД: {errors}");
            }

            Tyres = tyresRepository.Tyres.ToObservableCollection();

            tyrePropertyChanged += CalculareCustomDiametr;
            tyrePropertyChanged += FilterTyres;
        }
        #endregion

        #region CalculareCustomDiametrCommand
        private ICommand calculareCustomDiametrCommand;
        public ICommand CalculareCustomDiametrCommand
        {
            get
            {
                if (calculareCustomDiametrCommand == null)
                {
                    calculareCustomDiametrCommand = new RelayCommand(new Action<object>(CalculareCustomDiametr));
                }
                return calculareCustomDiametrCommand;
            }
            set
            {
                calculareCustomDiametrCommand = value;
                RaisedPropertyChanged("CalculareCustomDiametrCommand");
            }
        }
        public void CalculareCustomDiametr(object o)
        {
            CalculareCustomDiametr();
        }
        #endregion

        #region ShowAllCommand
        private ICommand showAllCommand;
        public ICommand ShowAllCommand
        {
            get
            {
                if (showAllCommand == null)
                {
                    showAllCommand = new RelayCommand(new Action<object>(ShowAll));
                }
                return showAllCommand;
            }
            set
            {
                showAllCommand = value;
                RaisedPropertyChanged("ShowAllCommand");
            }
        }

        public void ShowAll(object o)
        {
            ShowAll();
        }

        public void ShowAll()
        {
            Tyres = tyresRepository.Tyres.ToObservableCollection();
        }
        #endregion

        #region ShowAllCommand
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
            var win = new ChangeWeightWindow(tyresRepository, (Tyre)o, ShowAll);

            win.ShowDialog();
        }
        #endregion

        #region Private methods
        private void CalculareCustomDiametr()
        {
            var customWidth = Width.GetDoubleFromString();
            var customDiameter = Diameter.GetDoubleFromString();
            var customAspectRatio = AspectRatio.GetDoubleFromString();

            ResutDiameter = TyreHelpers.CalculateTyreDiametr(customDiameter, customWidth, customAspectRatio).ToString();

            FilterTyres();
        }

        private void FilterTyres()
        {
            var customWidth = Width.GetDoubleFromString();
            var customDiameter = Diameter.GetDoubleFromString();
            var customAspectRatio = AspectRatio.GetDoubleFromString();

            var filteredTyres = tyresRepository.Tyres.Where(t =>
                                        (IsZero(customWidth) ? true : t.Width == customWidth)
                                        && (IsZero(customDiameter) ? true : t.Diameter == customDiameter)
                                        && (IsZero(customAspectRatio) ? true : t.AspectRatio == customAspectRatio));

            Tyres = filteredTyres.ToObservableCollection();
        }

        private bool IsZero(double number) => number == 0;
        #endregion
    }
}

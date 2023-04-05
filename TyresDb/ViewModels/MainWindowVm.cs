using NLog;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using TyresDb.Model;
using TyresDb.Types;
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
                if (value.IsDigit())
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
                if (value.IsDigit())
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
                if (value.IsDigit())
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

        public void ShowAll(object parametr)
        {
            Tyres = tyresRepository.Tyres.ToObservableCollection();
        }
        #endregion

        #region ChangeCommand
        private ICommand сhangeCommand;
        public ICommand ChangeCommand
        {
            get
            {
                if (сhangeCommand == null)
                {
                    сhangeCommand = new RelayCommand(new Action<object>(Change));
                }
                return сhangeCommand;
            }
            set
            {
                сhangeCommand = value;
                RaisedPropertyChanged("ChangeCommand");
            }
        }

        public void Change(object parametr)
        {
            if (parametr == null)
                return;

            var win = new ChangeWeightWindow(WindowType.Update, tyresRepository, (Tyre)parametr, tyrePropertyChanged);

            win.ShowDialog();
        }
        #endregion

        #region DuplicateCommand
        private ICommand duplicateCommand;
        public ICommand DuplicateCommand
        {
            get
            {
                if (duplicateCommand == null)
                {
                    duplicateCommand = new RelayCommand(new Action<object>(Duplicate));
                }
                return duplicateCommand;
            }
            set
            {
                duplicateCommand = value;
                RaisedPropertyChanged("DuplicateCommand");
            }
        }

        public void Duplicate(object parametr)
        {
            if (parametr == null)
                return;

            Add(parametr);
        }
        #endregion

        #region AddCommand
        private ICommand addCommand;
        public ICommand AddCommand
        {
            get
            {
                if (addCommand == null)
                {
                    addCommand = new RelayCommand(new Action<object>(Add));
                }
                return addCommand;
            }
            set
            {
                addCommand = value;
                RaisedPropertyChanged("AddCommand");
            }
        }

        public void Add(object parametr)
        {
            var newTyre = new Tyre();
            
            if(parametr != null && parametr is Tyre && ((Tyre)parametr).Validate(out var err))
            {
                newTyre = ((Tyre)parametr).Clone();
            }
            var win = new ChangeWeightWindow(WindowType.Create, tyresRepository, newTyre, tyrePropertyChanged);
            win.ShowDialog();
        }
        #endregion

        #region DeleteCommand
        private ICommand deleteCommand;
        public ICommand DeleteCommand
        {
            get
            {
                if (deleteCommand == null)
                {
                    deleteCommand = new RelayCommand(new Action<object>(Delete));
                }
                return deleteCommand;
            }
            set
            {
                deleteCommand = value;
                RaisedPropertyChanged("DeleteCommand");
            }
        }

        public void Delete(object parametr)
        {
            if (parametr == null)
                return;

            var tyre = (Tyre)parametr;

            tyresRepository.Tyres.Remove(tyre);

            tyresRepository.Save();
            tyrePropertyChanged();
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

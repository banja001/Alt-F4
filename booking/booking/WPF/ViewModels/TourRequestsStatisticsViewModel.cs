using application.UseCases;
using booking.Commands;
using booking.Model;
using booking.View.Guide;
using booking.WPF.ViewModels;
using Domain.DTO;
using Domain.Model;
using LiveCharts.Wpf;
using LiveCharts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using WPF.Views.Guide;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Threading.Tasks;
using System.Threading;

namespace WPF.ViewModels
{
    public class TourRequestsStatisticsViewModel:BaseViewModel, INotifyPropertyChanged
    {
        private string selectedYear;
        private string selectedState;
        private string selectedCity;
        private string selectedMonth;
        private string language;
        private string numberOfTourRequests;
        private bool isLocationSelected;
        private bool isTimeSelected;
        private bool isLanguageSelected;

        private CancellationTokenSource cts = new CancellationTokenSource();

        private bool demoOn;
        public bool DemoOn
        {
            get { return demoOn; }
            set
            {
                demoOn = value;
                OnPropertyChanged(nameof(DemoOn));
            }
        }
        private string demoName;
        public string DemoName
        {
            get { return demoName; }
            set
            {
                if (demoName != value)
                {
                    demoName = value;
                    OnPropertyChanged(nameof(DemoName));
                }
            }
        }
        public bool IsLocationSelected
        {
            get { return isLocationSelected; }
            set
            {
                if (isLocationSelected != value)
                {
                    isLocationSelected = value;
                    OnPropertyChanged(nameof(IsLocationSelected));
                }
            }
        }
        public bool IsTimeSelected
        {
            get { return isTimeSelected; }
            set
            {
                if (isTimeSelected != value)
                {
                    isTimeSelected = value;
                    OnPropertyChanged(nameof(IsTimeSelected));
                }
            }
        }
        public bool IsLanguageSelected
        {
            get { return isLanguageSelected; }
            set
            {
                if (isLanguageSelected != value)
                {
                    isLanguageSelected = value;
                    OnPropertyChanged(nameof(IsLanguageSelected));
                }
            }
        }
        public string SelectedYear
        {
            get { return selectedYear; }
            set
            {
                if (selectedYear != value)
                {
                    selectedYear = value;
                    OnPropertyChanged(nameof(SelectedYear));
                }
            }
        }

        public string SelectedState
        {
            get { return selectedState; }
            set
            {
                if (selectedState != value)
                {
                    selectedState = value;
                    OnPropertyChanged(nameof(SelectedState));
                }
            }
        }

        public string SelectedCity
        {
            get { return selectedCity; }
            set
            {
                if (selectedCity != value)
                {
                    selectedCity = value;
                    OnPropertyChanged(nameof(SelectedCity));
                }
            }
        }

        public string SelectedMonth
        {
            get { return selectedMonth; }
            set
            {
                if (selectedMonth != value)
                {
                    selectedMonth = value;
                    OnPropertyChanged(nameof(SelectedMonth));
                }
            }
        }

        public string Language
        {
            get { return language; }
            set
            {
                if (language != value)
                {
                    language = value;
                    OnPropertyChanged(nameof(Language));
                }
            }
        }
        public string NumberOfTourRequests
        {
            get { return numberOfTourRequests; }
            set
            {
                numberOfTourRequests = value;
                OnPropertyChanged(nameof(NumberOfTourRequests));
            }
        }

        private bool locationTooltip;

        public bool LocationTooltip
        {
            get { return locationTooltip; }
            set
            {
                if (locationTooltip != value)
                {
                    locationTooltip = value;
                    OnPropertyChanged(nameof(LocationTooltip));
                }
            }
        }

        private bool timeTooltip;

        public bool TimeTooltip
        {
            get { return timeTooltip; }
            set
            {
                if (timeTooltip != value)
                {
                    timeTooltip = value;
                    OnPropertyChanged(nameof(TimeTooltip));
                }
            }
        }

        private bool languageTooltip;

        public bool LanguageTooltip
        {
            get { return languageTooltip; }
            set
            {
                if (languageTooltip != value)
                {
                    languageTooltip = value;
                    OnPropertyChanged(nameof(LanguageTooltip));
                }
            }
        }

        private AxesCollection axis;
        public AxesCollection Axis 
        {
            get { return axis; }
            set
            {
                axis = value;
                OnPropertyChanged(nameof(Axis));
            }
        }
        private SeriesCollection series;
        public SeriesCollection Series 
        {
            get { return series; }
            set
            {
                series = value;
                OnPropertyChanged(nameof(Series));
            }
        }
        public ObservableCollection<string> Cities { get; set; }
        public ObservableCollection<string> States { get; set; }
        public ObservableCollection<string> Years { get; set; }
        public ObservableCollection<string> Months { get; set; }
        public ObservableCollection<SimpleRequest> AllRequests { get; set; }

        private LocationService _locationService;
        private SimpleRequestService _simpleRequestService;

        //public ICommand SearchCommand => new RelayCommand(Search);

        public ICommand LocationSelectedCommand => new RelayCommand(LocationSelected);
        public ICommand TimeSelectedCommand => new RelayCommand(TimeSelected);
        public ICommand LanguageSelectedCommand => new RelayCommand(LanguageSelected);
        public ICommand FillCityCBCommand => new RelayCommand(FillCities);
        public ICommand FillMonthsCBCommand => new RelayCommand(FillMonths);
        public ICommand CreateCommand => new RelayCommand(CreateTourWithHelpOfStatistics);
        public ICommand TooltipLocationCommand => new RelayCommand(LocationToolTip);
        public ICommand TooltipTimeCommand => new RelayCommand(TimeToolTip);

        public ICommand TooltipLanguageCommand => new RelayCommand(LanguageToolTip);

        public ICommand DemoCommand => new RelayCommand(StartStopDemo);

        public User Guide { get; set; }
        private NavigationService navigationService;


        private string preDemoState;
        private string preDemoYear;
        private bool preDemoLocation;
        private bool preDemoTime;
        private bool preDemoLanguage;
        public TourRequestsStatisticsViewModel(User guide, NavigationService navigationService) 
        {
            _locationService=new LocationService();
            _simpleRequestService=new SimpleRequestService();
            Cities=new ObservableCollection<string>();
            States= new ObservableCollection<string>(_locationService.InitializeListOfStates());
            Years=new ObservableCollection<string>(_simpleRequestService.GetAllYears());
            Months=new ObservableCollection<string>();
            AllRequests=new ObservableCollection<SimpleRequest>();
            Guide = guide;
            this.navigationService=navigationService;
            isLocationSelected = true;
            IsTimeSelected = false;
            IsLanguageSelected = false;
            LoadRequests();
            CreateStatisticForGraphState();
            DemoName = "Demo";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void LocationToolTip()
        {
            LocationTooltip = !LocationTooltip;
        }
        private void TimeToolTip()
        {
            TimeTooltip = !TimeTooltip;
        }
        private void LanguageToolTip()
        {
            LanguageTooltip = !LanguageTooltip;
        }
        private void LoadRequests()
        {
            AllRequests.Clear();
            foreach (var request in _simpleRequestService.GetAllWithLocation())
            {
                AllRequests.Add(request);
            }
        }
        private void FillCities()
        {
            Cities.Clear();
            foreach (var state in _locationService.FillListWithCities(SelectedState))
            {
                Cities.Add(state);
            }
            CreateStatisticForGraphCity(SelectedState);
        }

        private void FillMonths() 
        {
            Months.Clear();
            foreach (var year in _simpleRequestService.GetAllMonthsForYear(SelectedYear))
            {
                Months.Add(year);
            }
            CreateStatisticForGraphMonths(SelectedYear);
        }
        private void LocationSelected()
        {

            SelectedYear = "";
            IsLocationSelected = true;
            CreateStatisticForGraphState();
            
        }
        private void TimeSelected()
        {
            SelectedState = "";
            IsTimeSelected = true;
            CreateStatisticForGraphYears();   
        }
        private void LanguageSelected()
        {
            SelectedYear = "";
            SelectedState = "";
            IsLanguageSelected = true;
            CreateStatisticForGraphLanguage();
            
        }
        private void CreateTourWithHelpOfStatistics()
        {
            Window window = System.Windows.Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
            if (window != null)
            {
                window.Effect = new BlurEffect();
            }
            ParametarOfStatisticsForTourCreationWindow pwindow = new ParametarOfStatisticsForTourCreationWindow(DemoOn);
            pwindow.ShowDialog();

            if (ParametarOfStatisticsForTourCreationVIewModel.Accept)
            {
                string [] parameters= ParametarOfStatisticsForTourCreationVIewModel.Parameter.Split("|");
                navigationService.Navigate(new AddTourWindow(parameters, Guide));
            }
            window.Effect = null;
        }

        private void CreateStatisticForGraphYears()
        {
            Series = new SeriesCollection();
            Axis = new AxesCollection();
            ColumnSeries YearsColumns = new ColumnSeries() { DataLabels = true, Values = new ChartValues<int>(), LabelPoint = point => point.Y.ToString(), Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#AA96DA")) };
            Axis yearsAxis = new Axis() { Separator = new LiveCharts.Wpf.Separator() { Step = 1, IsEnabled = false } };
            yearsAxis.Labels = new List<string>();
            yearsAxis.Foreground = new SolidColorBrush(Colors.Black);
            yearsAxis.FontSize = 15;
            var yearRequestCountPairs = _simpleRequestService.GetYearsForChart(AllRequests.ToList());
            foreach (var pair in yearRequestCountPairs)
            {
                yearsAxis.Labels.Add(pair.Key);
                YearsColumns.Values.Add(pair.Value);
            }
            Series.Add(YearsColumns);
            Axis.Add(yearsAxis);
        }
        private void CreateStatisticForGraphMonths(string selectedYear)
        {
            Series = new SeriesCollection();
            Axis = new AxesCollection();
            ColumnSeries monthsColumns = new ColumnSeries() { DataLabels = true, Values = new ChartValues<int>(), LabelPoint = point => point.Y.ToString(), Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#AA96DA")) };
            Axis monthsAxis = new Axis() { Separator = new LiveCharts.Wpf.Separator() { Step = 1, IsEnabled = false } };
            monthsAxis.Labels = new List<string>();
            monthsAxis.Foreground = new SolidColorBrush(Colors.Black);
            monthsAxis.FontSize = 15;
            var yearRequestCountPairs = _simpleRequestService.GetMonthsForChart(AllRequests.ToList(), selectedYear);
            foreach (var pair in yearRequestCountPairs)
            {
                monthsAxis.Labels.Add(pair.Key);
                monthsColumns.Values.Add(pair.Value);
            }
            Series.Add(monthsColumns);
            Axis.Add(monthsAxis);
        }
        private void CreateStatisticForGraphState()
        {
            Series = new SeriesCollection();
            Axis = new AxesCollection();
            ColumnSeries stateColumns = new ColumnSeries() { DataLabels = true, Values = new ChartValues<int>(), LabelPoint = point => point.Y.ToString(), Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#AA96DA")) };
            Axis stateAxis = new Axis() { Separator = new LiveCharts.Wpf.Separator() { Step = 1, IsEnabled = false } };
            stateAxis.Labels = new List<string>();
            stateAxis.Foreground = new SolidColorBrush(Colors.Black);
            stateAxis.FontSize = 15;
            var yearRequestCountPairs = _simpleRequestService.GetStatesForChart(AllRequests.ToList());
            foreach (var pair in yearRequestCountPairs)
            {
                stateAxis.Labels.Add(pair.Key);
                stateColumns.Values.Add(pair.Value);
            }
            Series.Add(stateColumns);
            Axis.Add(stateAxis);
        }

        private void CreateStatisticForGraphCity(string selectedState)
        {
            Series = new SeriesCollection();
            Axis = new AxesCollection();
            ColumnSeries cityColumns = new ColumnSeries() { DataLabels = true, Values = new ChartValues<int>(), LabelPoint = point => point.Y.ToString(), Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#AA96DA")) };
            Axis cityAxis = new Axis() { Separator = new LiveCharts.Wpf.Separator() { Step = 1, IsEnabled = false } };
            cityAxis.Labels = new List<string>();
            cityAxis.Foreground = new SolidColorBrush(Colors.Black);
            cityAxis.FontSize = 15;
            var yearRequestCountPairs = _simpleRequestService.GetCitiesForChart(AllRequests.ToList(), selectedState);
            foreach (var pair in yearRequestCountPairs)
            {
                cityAxis.Labels.Add(pair.Key);
                cityColumns.Values.Add(pair.Value);
            }
            Series.Add(cityColumns);
            Axis.Add(cityAxis);
        }

        private void CreateStatisticForGraphLanguage()
        {
            Series = new SeriesCollection();
            Axis = new AxesCollection();
            ColumnSeries languageColumns = new ColumnSeries() { DataLabels = true, Values = new ChartValues<int>(), LabelPoint = point => point.Y.ToString(), Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#AA96DA")) };
            Axis languageAxis = new Axis() { Separator = new LiveCharts.Wpf.Separator() { Step = 1, IsEnabled = false } };
            languageAxis.Labels = new List<string>();
            languageAxis.Foreground = new SolidColorBrush(Colors.Black);
            languageAxis.FontSize = 15;
            var languageRequestCountPairs = _simpleRequestService.GetLanguagesForChart(AllRequests.ToList());
            foreach (var pair in languageRequestCountPairs)
            {
                languageAxis.Labels.Add(pair.Key);
                languageColumns.Values.Add(pair.Value);
            }
            Series.Add(languageColumns);
            Axis.Add(languageAxis);
        }
        private void Search()
        {
            LoadRequests();
            if (!string.IsNullOrEmpty(SelectedState))
                FilterState();
            if (!string.IsNullOrEmpty(SelectedCity))
                FilterCity();
            if (!string.IsNullOrEmpty(Language))
                FilterLanguage();
            if (!string.IsNullOrEmpty(SelectedYear))
                FilterYear();
            if (!string.IsNullOrEmpty(SelectedMonth))
                FilterMonth();
            NumberOfTourRequests=AllRequests.Count().ToString();
        }
        private void FilterState()
        {
            List<SimpleRequest> list = AllRequests.Where(req => req.Location.State == SelectedState).ToList();
            AllRequests = new ObservableCollection<SimpleRequest>(list);
        }
        private void FilterCity()
        {
            List<SimpleRequest> list = AllRequests.Where(req => req.Location.City == SelectedCity).ToList();
            AllRequests = new ObservableCollection<SimpleRequest>(list);
        }
        private void FilterLanguage()
        {
            List<SimpleRequest> list = AllRequests.Where(req => req.Language.ToLower() == Language.ToLower()).ToList();
            AllRequests = new ObservableCollection<SimpleRequest>(list);

        }
        private void FilterYear()
        {
            List<SimpleRequest> list = AllRequests.Where(req => req.DateRange.StartDate.Year == Convert.ToInt32(SelectedYear)).ToList();
            AllRequests = new ObservableCollection<SimpleRequest>(list);
        }
        private void FilterMonth()
        {
            List<SimpleRequest> list = AllRequests.Where(req => req.DateRange.StartDate.Month == MonthNameToMonthNumber(SelectedMonth)).ToList();
            AllRequests = new ObservableCollection<SimpleRequest>(list);
        }
        private void StartStopDemo()
        {
            if (DemoOn)
            {
                IsLanguageSelected = preDemoLanguage;
                IsLocationSelected = preDemoLocation;
                SelectedState = preDemoState;
                IsTimeSelected = preDemoTime;
                SelectedYear = preDemoYear;
                if (IsLanguageSelected)
                    LanguageSelected();
                if (IsLocationSelected)
                    LocationSelected();
                if (IsTimeSelected)
                    TimeSelected();
                if(!String.IsNullOrEmpty(SelectedState))
                    FillCities();
                if (!String.IsNullOrEmpty(SelectedYear))
                    FillMonths();
                cts.Cancel();
                DemoOn = !DemoOn;
                DemoName = "Demo";
                MessageBox.Show("Demo has been stopped!", "Demo message", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                preDemoLanguage = IsLanguageSelected;
                preDemoLocation = IsLocationSelected;
                preDemoState = SelectedState;
                preDemoTime = IsTimeSelected;
                preDemoYear = SelectedYear;
                SelectedState = "";
                SelectedYear = "";
                cts = new CancellationTokenSource();
                DemoOn = !DemoOn;
                DemoIsOn(cts.Token); 
                DemoName = "Stop";
            }
        }
        private async Task DemoIsOn(CancellationToken ct)
        {
            
            if (DemoOn)
            {

                ct.ThrowIfCancellationRequested();
                //MessageBox.Show("Demo has started!", "Demo message", MessageBoxButton.OK, MessageBoxImage.Information);
                ct.ThrowIfCancellationRequested();
                StartDemoWindow sdw = new StartDemoWindow();
                sdw.Show();
                await Task.Delay(2000, ct);
                sdw.Close();
                IsLocationSelected = true;
                await Task.Delay(4000, ct);
                SelectedState = "Nemacka";
                await Task.Delay(4000, ct);
                IsTimeSelected = true;
                await Task.Delay(4000, ct);
                SelectedYear = "2023";
                await Task.Delay(4000, ct);
                IsLanguageSelected = true;
                await Task.Delay(1000, ct);
                CreateTourWithHelpOfStatistics();
                //DemoOn = !DemoOn;
                //DemoName = "Demo";
                //MessageBox.Show("Demo has been finished!","Demo message",MessageBoxButton.OK,MessageBoxImage.Information);
            }
        }




        public int MonthNameToMonthNumber(string monthName)
        {
            int monthNumber;
            switch (monthName)
            {
                case "January":
                    monthNumber = 1;
                    break;
                case "February":
                    monthNumber = 2;
                    break;
                case "March":
                    monthNumber = 3;
                    break;
                case "April":
                    monthNumber = 4;
                    break;
                case "May":
                    monthNumber = 5;
                    break;
                case "June":
                    monthNumber = 6;
                    break;
                case "July":
                    monthNumber = 7;
                    break;
                case "August":
                    monthNumber = 8;
                    break;
                case "September":
                    monthNumber = 9;
                    break;
                case "October":
                    monthNumber = 10;
                    break;
                case "November":
                    monthNumber = 11;
                    break;
                case "December":
                    monthNumber = 12;
                    break;
                default:
                    monthNumber = -1;
                    break;
            }
            return monthNumber;
        }
    }
}

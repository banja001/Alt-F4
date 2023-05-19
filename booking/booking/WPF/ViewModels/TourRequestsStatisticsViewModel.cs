using application.UseCases;
using booking.Commands;
using booking.Model;
using booking.View.Guide;
using booking.WPF.ViewModels;
using Domain.DTO;
using Domain.Model;
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
        public ObservableCollection<string> Cities { get; set; }
        public ObservableCollection<string> States { get; set; }
        public ObservableCollection<string> Years { get; set; }
        public ObservableCollection<string> Months { get; set; }
        public ObservableCollection<SimpleRequest> AllRequests { get; set; }

        private LocationService _locationService;
        private SimpleRequestService _simpleRequestService;

        public ICommand SearchCommand => new RelayCommand(Search);
        public ICommand FillCityCBCommand => new RelayCommand(FillCities);
        public ICommand FillMonthsCBCommand => new RelayCommand(FillMonths);
        public ICommand CreateCommand => new RelayCommand(CreateTourWithHelpOfStatistics);
        public User Guide { get; set; }
        private NavigationService navigationService;
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
            LoadRequests();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
        }

        private void FillMonths() 
        {
            Months.Clear();
            foreach (var year in _simpleRequestService.GetAllMonthsForYear(SelectedYear))
            {
                Months.Add(year);
            }
        }
        private void CreateTourWithHelpOfStatistics()
        {
            ParametarOfStatisticsForTourCreationWindow window = new ParametarOfStatisticsForTourCreationWindow();
            window.ShowDialog();

            if (ParametarOfStatisticsForTourCreationVIewModel.Accept)
            {
                string [] parameters= ParametarOfStatisticsForTourCreationVIewModel.Parameter.Split("|");
                navigationService.Navigate(new AddTourWindow(parameters, Guide));
            }
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

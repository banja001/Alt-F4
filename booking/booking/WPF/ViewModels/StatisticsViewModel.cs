using application.UseCases;
using booking.Commands;
using booking.Model;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Windows.Input;

namespace booking.WPF.ViewModels
{
    public class StatisticsViewModel : BaseViewModel
    {
        public string ApprovedPercentage { get; set; }
        public string InvalidPercentage { get; set; }
        public string AveragePeopleCount { get; set; }
        public AxesCollection LangugeAxis { get; set; }
        public SeriesCollection LanguageSeries { get; set; }
        public AxesCollection LocationAxis { get; set; }
        public SeriesCollection LocationSeries { get; set; }
        public ICommand FilterRequestPercentageCommand => new RelayCommandWithParams(OnFilterRequestPercentage);
        public ICommand FilterAveragePeopleCountCommand => new RelayCommandWithParams(OnFilterAveragePeopleCount);
        public ObservableCollection<String> RequestsYears { get; set; }
        public User User { get; set; }   

        private readonly SimpleRequestService _simpleRequestService;
        public StatisticsViewModel(User user)
        {
            User = user;
            _simpleRequestService = new SimpleRequestService();
            InitializeLabels(user);
            InitializeLocationGraph(user);
            InitializeLanguageGraph(user);
        }
        private void InitializeLocationGraph(User user)
        {
            LocationSeries = new SeriesCollection();
            LocationAxis = new AxesCollection();
            ColumnSeries locationColumns = new ColumnSeries() { DataLabels = true, Values = new ChartValues<int>(), LabelPoint = point => point.Y.ToString() };
            Axis locationAxis = new Axis() { Separator = new Separator() { Step = 1, IsEnabled = false } };
            locationAxis.Labels = new List<string>();
            var locationRequestCountPairs = _simpleRequestService.GetLocationChartByGuest2(user);
            foreach (var pair in locationRequestCountPairs)
            {
                locationAxis.Labels.Add(pair.Key);
                locationColumns.Values.Add(pair.Value);
            }
            LocationSeries.Add(locationColumns);
            LocationAxis.Add(locationAxis);
        }
        private void InitializeLabels(User user)
        {
            RequestsYears = new ObservableCollection<string>(_simpleRequestService.GetAvailableRequestsYears(user).OrderBy(y => y));
            AveragePeopleCount = Math.Round(_simpleRequestService.GetAveragePeopleCountByGuest2(user, new DateTime()), 1).ToString();
            ApprovedPercentage = Math.Round(_simpleRequestService.GetApprovedRequestsRatioByGuest2(user, new DateTime()) * 100, 1).ToString() + "%";
            InvalidPercentage = Math.Round(_simpleRequestService.GetInvalidRequestsRatioByGuest2(user, new DateTime()) * 100, 1).ToString() + "%";
        }

        private void InitializeLanguageGraph(User user)
        {

            LanguageSeries = new SeriesCollection();
            LangugeAxis = new AxesCollection();
            ColumnSeries languageColumns = new ColumnSeries() { DataLabels = true, Values = new ChartValues<int>(), LabelPoint = point => point.Y.ToString() };
            Axis languageAxis = new Axis() { Separator = new Separator() { Step = 1, IsEnabled = false } };
            languageAxis.Labels = new List<string>();
            var languageRequestCountPairs = _simpleRequestService.GetLanguageChartByGuest2(user);
            foreach(var pair in languageRequestCountPairs)
            {
                languageAxis.Labels.Add(pair.Key);
                languageColumns.Values.Add((pair.Value)); 
            }
            LanguageSeries.Add(languageColumns);
            LangugeAxis.Add(languageAxis);
        }

        private void OnFilterAveragePeopleCount(object parameter)
        {
            if(parameter == null)
            {
                AveragePeopleCount = Math.Round(_simpleRequestService.GetAveragePeopleCountByGuest2(User, new DateTime()), 1).ToString();
                OnPropertyChanged(nameof(AveragePeopleCount));
                return;
            }

            int year = (int)Convert.ToInt64(parameter);
            DateTime desiredYear = new DateTime(year, 1, 1);
            AveragePeopleCount = Math.Round(_simpleRequestService.GetAveragePeopleCountByGuest2(User, desiredYear), 1).ToString();
            OnPropertyChanged(nameof(AveragePeopleCount));
        }
        private void OnFilterRequestPercentage(object parameter)
        {
            if (parameter == null)
            {
                ApprovedPercentage = Math.Round(_simpleRequestService.GetApprovedRequestsRatioByGuest2(User, new DateTime()) * 100, 1).ToString() + "%";
                InvalidPercentage = Math.Round(_simpleRequestService.GetInvalidRequestsRatioByGuest2(User, new DateTime()) * 100, 1).ToString() + "%";
                OnPropertyChanged(nameof(InvalidPercentage));
                OnPropertyChanged(nameof(ApprovedPercentage));
                return;
            }
            int year = (int)Convert.ToInt64(parameter);
            DateTime desiredYear = new DateTime(year, 1, 1);
            ApprovedPercentage = (Math.Round(_simpleRequestService.GetApprovedRequestsRatioByGuest2(User, desiredYear), 1)*100).ToString() + "%";
            InvalidPercentage = (Math.Round(_simpleRequestService.GetInvalidRequestsRatioByGuest2(User, desiredYear), 1) * 100).ToString() + "%";
            OnPropertyChanged(nameof(InvalidPercentage));
            OnPropertyChanged(nameof(ApprovedPercentage));
        }
    }
}

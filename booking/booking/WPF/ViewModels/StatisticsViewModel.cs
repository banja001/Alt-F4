using application.UseCases;
using booking.Commands;
using booking.Model;
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
        public ICommand FilterRequestPercentageCommand => new RelayCommandWithParams(OnFilterRequestPercentage);
        public ICommand FilterAveragePeopleCountCommand => new RelayCommandWithParams(OnFilterAveragePeopleCount);
        public ObservableCollection<String> RequestsYears { get; set; }
        public User User { get; set; }   

        private readonly SimpleRequestService _simpleRequestService;
        public StatisticsViewModel(User user)
        {
            User = user;
            _simpleRequestService = new SimpleRequestService();
            RequestsYears = new ObservableCollection<string>(_simpleRequestService.GetAvailableRequestsYears(user).OrderBy(y => y));
            AveragePeopleCount = Math.Round(_simpleRequestService.GetAveragePeopleCountByGuest2(user, new DateTime()), 1).ToString();
            ApprovedPercentage = Math.Round(_simpleRequestService.GetApprovedRequestsRatioByGuest2(user, new DateTime()) * 100, 1).ToString() + "%";
            InvalidPercentage = Math.Round(_simpleRequestService.GetInvalidRequestsRatioByGuest2(user, new DateTime()) * 100, 1).ToString() + "%";
        }
        private void OnFilterAveragePeopleCount(object parameter)
        {
            int year = (int)Convert.ToInt64(parameter);
            DateTime desiredYear = new DateTime(year, 1, 1);
            AveragePeopleCount = Math.Round(_simpleRequestService.GetAveragePeopleCountByGuest2(User, desiredYear), 1).ToString();
            OnPropertyChanged(nameof(AveragePeopleCount));
        }
        private void OnFilterRequestPercentage(object parameter)
        {
            int year = (int)Convert.ToInt64(parameter);
            DateTime desiredYear = new DateTime(year, 1, 1);
            ApprovedPercentage = (Math.Round(_simpleRequestService.GetApprovedRequestsRatioByGuest2(User, desiredYear), 1)*100).ToString() + "%";
            InvalidPercentage = (Math.Round(_simpleRequestService.GetInvalidRequestsRatioByGuest2(User, desiredYear), 1) * 100).ToString() + "%";
            OnPropertyChanged(nameof(InvalidPercentage));
            OnPropertyChanged(nameof(ApprovedPercentage));

        }
    }
}

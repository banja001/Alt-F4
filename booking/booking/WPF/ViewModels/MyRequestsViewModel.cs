using application.UseCases;
using booking.Commands;
using booking.Model;
using booking.WPF.ViewModels;
using Domain.Model;
using Microsoft.VisualBasic;
using Syncfusion.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace booking.WPF.ViewModels
{
    public class MyRequestsViewModel : BaseViewModel
    {
        public string Description { get; set; } 
        public string Language { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int NumberOfGuests { get; set; }
        public ObservableCollection<SimpleRequest> Requests { get; set; }  
        public ICommand SubmitSimpleRequestCommand => new RelayCommand(OnSubmitSimpleRequest);
        public ICommand CancelSimpleRequestCommand => new RelayCommand(OnCancelSimpleRequest);
        private readonly LocationService _locationService;
        private readonly SimpleRequestService _simpleRequestService;

        public MyRequestsViewModel()
        {
            StartDate = new DateTime();
            EndDate = new DateTime();
            _simpleRequestService = new SimpleRequestService();
            _locationService = new LocationService();
            Requests = new ObservableCollection<SimpleRequest>();   
        }
        private void OnSubmitSimpleRequest()
        {
            SimpleRequest simpleRequest = new SimpleRequest();
            AddSimpleRequest(simpleRequest);
            Requests.Add(simpleRequest);
        }
        private void AddSimpleRequest(SimpleRequest simpleRequest)
        {
            Location location = new Location(-1, City, State);

            location.Id = !_locationService.Contains(location) ? _locationService.MakeID() : location.Id = _locationService.GetId(location.State, location.City);
            if (StartDate.ToDateTime().Date <= DateTime.Now.AddDays(2))
                MessageBox.Show("The start date of request should be at least 48h earlier than actual date of tour", "Alert");

            simpleRequest = new SimpleRequest(Description,
                                                        location.Id,
                                                        Language,
                                                        NumberOfGuests,
                                                        StartDate.ToDateTime(),
                                                        EndDate.ToDateTime(),
                                                        SimpleRequestStatus.ON_HOLD);
            _simpleRequestService.Add(simpleRequest);
        }
        private void OnCancelSimpleRequest()
        {
            Description = "";
            Language = "";
            NumberOfGuests = 0;
            City = "";
            State = "";
            OnPropertyChanged(nameof(Description));
            OnPropertyChanged(nameof(Language));
            OnPropertyChanged(nameof(NumberOfGuests));
            OnPropertyChanged(nameof(State));
            OnPropertyChanged(nameof(City));
        }
    }
}

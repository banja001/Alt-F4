using application.UseCases;
using booking.Commands;
using booking.Model;
using booking.WPF.ViewModels;
using Domain.DTO;
using Domain.Model;
using Microsoft.VisualBasic;
using Syncfusion.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

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
        public User User { get; set; }
        public ObservableCollection<SimpleRequestDTO> Requests { get; set; }  
        public ICommand SubmitSimpleRequestCommand => new RelayCommand(OnSubmitSimpleRequest);
        public ICommand CancelSimpleRequestCommand => new RelayCommand(OnCancelSimpleRequest);
        public ICommand PreviewRequestCommand => new RelayCommandWithParams(OnPreviewRequest);
        private readonly LocationService _locationService;
        private readonly SimpleRequestService _simpleRequestService;
        private int dispatcherIncrementer = 0;

        public MyRequestsViewModel() { }

        public MyRequestsViewModel(User user)
        {
            User = user;
            StartDate = new DateTime();
            EndDate = new DateTime();
            _simpleRequestService = new SimpleRequestService();
            _locationService = new LocationService();
            Requests = new ObservableCollection<SimpleRequestDTO>(_simpleRequestService.CreateDTOsByGuest2(user));

            InitializeTimer();
        }
        private void InitializeTimer()
        {
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval = TimeSpan.FromHours(1);
            dispatcherTimer.Tick += DispatcherTicker;
            dispatcherTimer.Start();
        }
        private void DispatcherTicker(object sender, EventArgs e)
        {
            dispatcherIncrementer++;
            _simpleRequestService.CheckApproval();
            Requests = new ObservableCollection<SimpleRequestDTO>(_simpleRequestService.CreateDTOsByGuest2(User));
        }
        private void OnPreviewRequest(object parameter)
        {
            int idx = (Requests.ToList<SimpleRequestDTO>()).FindIndex(r => r.Id == (int)parameter);
            Description = Requests[idx].Description;
            Language = Requests[idx].Language;
            NumberOfGuests = Requests[idx].NumberOfGuests;
            StartDate = Requests[idx].StartDate;
            EndDate = Requests[idx].EndDate;
            State = Requests[idx].Location.State;
            City = Requests[idx].Location.City;

            NotifyPropertiesChanged();
        }
        private void OnSubmitSimpleRequest()
        {
            SimpleRequest simpleRequest = new SimpleRequest();
            AddSimpleRequest(simpleRequest);
            Requests.Add(new SimpleRequestDTO(Requests.Count+1,
                                              Description,
                                              NumberOfGuests,
                                              Language,
                                              StartDate,
                                              EndDate,
                                              simpleRequest.GetStatusUri(),
                                              new Location(-1, City, State)));
            OnPropertyChanged(nameof(Requests));
            OnCancelSimpleRequest();
        }
        private void AddSimpleRequest(SimpleRequest simpleRequest)
        {
            Location location = new Location(-1, City, State);

            if (_locationService.Contains(location))
            {
                location.Id = _locationService.GetId(location.State, location.City);
            }
            else
            {
                location.Id = _locationService.MakeID();
                _locationService.Add(location);
            }
            location.Id = !_locationService.Contains(location) ? _locationService.MakeID() : location.Id = _locationService.GetId(location.State, location.City);
            if (StartDate.ToDateTime().Date <= DateTime.Now.AddDays(2))
                MessageBox.Show("The start date of request should be at least 48h earlier than actual date of tour", "Alert");
            simpleRequest.User.Id = User.Id;
            simpleRequest.Description = Description;
            simpleRequest.Location.Id = location.Id;
            simpleRequest.Language = Language;
            simpleRequest.NumberOfGuests = NumberOfGuests;
            simpleRequest.DateRange.StartDate = StartDate;
            simpleRequest.DateRange.EndDate = EndDate;
            simpleRequest.Status = SimpleRequestStatus.ON_HOLD;
            _simpleRequestService.Add(simpleRequest);
            
        }
        private void OnCancelSimpleRequest()
        {
            Description = "";
            Language = "";
            NumberOfGuests = 0;
            City = "";
            State = "";
            NotifyPropertiesChanged();
        }
        private void NotifyPropertiesChanged()
        {
            OnPropertyChanged(nameof(Description));
            OnPropertyChanged(nameof(Language));
            OnPropertyChanged(nameof(NumberOfGuests));
            OnPropertyChanged(nameof(State));
            OnPropertyChanged(nameof(City));
            OnPropertyChanged(nameof(StartDate));
            OnPropertyChanged(nameof(EndDate));
        }
    }
}

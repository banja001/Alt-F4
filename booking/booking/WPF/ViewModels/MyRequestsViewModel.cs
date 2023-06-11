using application.UseCases;
using booking.Commands;
using booking.Model;
using booking.WPF.ViewModels;
using Domain.DTO;
using Domain.Model;
using Microsoft.VisualBasic;
using Syncfusion.Windows.Controls;
using Syncfusion.Windows.Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Threading;
using WPF.Views.Guest2;

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
        public ObservableCollection<ComplexRequest> ComplexRequests { get; set; }
        public ObservableCollection<SimpleRequestDTO> AddedSimpleRequests { get; set; }
        public ObservableCollection<SimpleRequestDTO> Requests { get; set; }  
        public ICommand SubmitSimpleRequestCommand => new RelayCommand(OnSubmitSimpleRequest);
        public ICommand CancelSimpleRequestCommand => new RelayCommand(OnCancelSimpleRequest);
        public ICommand PreviewRequestCommand => new RelayCommandWithParams(OnPreviewRequest);
        public RelayCommand AddSimpleRequestCommand => new RelayCommand(OnAddSimpleRequest);
        public ICommand PreviewAddedRequestCommand => new RelayCommandWithParams(OnPreviewAddedRequest);
        public ICommand ClearAddedRequestsCommand => new RelayCommand(OnClearAddedRequests, CanClearAddedRequests);
        public ICommand ShowComplexRequestCommand => new RelayCommandWithParams(OnShowComplexRequest);

        public ICommand SubmitComplexRequest => new RelayCommand(OnSubmitComplexRequest);

        private readonly LocationService _locationService;
        private readonly SimpleRequestService _simpleRequestService;
        private readonly ComplexRequestService _complexRequestService;
        private int dispatcherIncrementer = 0;

        public MyRequestsViewModel() { }

        public MyRequestsViewModel(User user)
        {
            User = user;
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
            _simpleRequestService = new SimpleRequestService();
            _locationService = new LocationService();
            Requests = new ObservableCollection<SimpleRequestDTO>(_simpleRequestService.CreateDTOsByGuest2(user));
            AddedSimpleRequests = new ObservableCollection<SimpleRequestDTO>();
            _simpleRequestService.InitializeTimer(DispatcherTicker, dispatcherIncrementer);
            _complexRequestService = new ComplexRequestService();
            ComplexRequests = new ObservableCollection<ComplexRequest>(_complexRequestService.GetAllByGuest2(user));
        }
        private void OnShowComplexRequest(object parameter)
        {
            AddedSimpleRequests.Clear();
            var wantedComplexRequest = _complexRequestService.GetById((int)parameter);
            foreach (var r in wantedComplexRequest.SimpleRequests){
                var request = _simpleRequestService.GetById(r.Id);
                AddedSimpleRequests.Add(new SimpleRequestDTO(AddedSimpleRequests.Count + 1,
                                                             request.Description,
                                                             request.NumberOfGuests,
                                                             request.Language,
                                                             request.DateRange.StartDate,
                                                             request.DateRange.EndDate,
                                                             request.GetStatusUri(),
                                                             _locationService.GetById(request.Location.Id))
                                                            );
            }
            OnPropertyChanged(nameof(AddedSimpleRequests)); 
        }
        private bool CanClearAddedRequests()
        {
            return AddedSimpleRequests.Count > 0;
        }
        private void OnAddSimpleRequest()
        {
            if (!ValidateForm())
            {
                MessageBox.Show("Values you have entered are invalid!", "Error");
                return;
            }
            if(AddedSimpleRequests.Count() > 0)
            {
                var previousSimpleRequest = new SimpleRequest(User, AddedSimpleRequests.First());
                if(previousSimpleRequest.DateRange.StartDate.Date > StartDate.Date)
                {
                    MessageBox.Show("You can't enter date that is before first simple request date!", "Error");
                    return;
                }
            }
                SimpleRequest simpleRequest = new SimpleRequest();
            if (AddSimpleRequest(simpleRequest, "Complex"))
            {
                AddedSimpleRequests.Add(new SimpleRequestDTO(AddedSimpleRequests.Count + 1,
                                                  Description,
                                                  NumberOfGuests,
                                                  Language,
                                                  StartDate,
                                                  EndDate,
                                                  simpleRequest.GetStatusUri(),
                                                  simpleRequest.Location));
            }
            OnPropertyChanged(nameof(AddedSimpleRequests));
            OnCancelSimpleRequest();
        }
        private void OnSubmitComplexRequest()
        {
            var addedSimpleRequests = _simpleRequestService.ConvertByGuest2(AddedSimpleRequests.ToList<SimpleRequestDTO>(), User);
            
            ComplexRequest complexRequest = new ComplexRequest(User.Id, SimpleRequestStatus.ON_HOLD, addedSimpleRequests);
            
            Window window = System.Windows.Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
            if (window != null)
            {
                window.Effect = new BlurEffect();
            }
            var addComplexRequestView = new AddComplexRequestView(User, complexRequest);
            addComplexRequestView.ShowDialog();
            window.Effect = null;

            ComplexRequests.Clear();
            ComplexRequests = new ObservableCollection<ComplexRequest>(_complexRequestService.GetAllByGuest2(User));
            OnPropertyChanged(nameof(ComplexRequests));
        }
        private void OnClearAddedRequests()
        {
            AddedSimpleRequests.Clear();
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
            ShowDetails(idx, Requests.ToList<SimpleRequestDTO>());
            NotifyPropertiesChanged();
        }

        private void ShowDetails(int idx, List<SimpleRequestDTO> simpleRequests)
        {
            Description = simpleRequests[idx].Description;
            Language = simpleRequests[idx].Language;
            NumberOfGuests = simpleRequests[idx].NumberOfGuests;
            StartDate = simpleRequests[idx].StartDate;
            EndDate = simpleRequests[idx].EndDate;
            State = simpleRequests[idx].Location.State;
            City = simpleRequests[idx].Location.City;
        }

        private void OnPreviewAddedRequest(object parameter)
        {
            int idx = (AddedSimpleRequests.ToList<SimpleRequestDTO>()).FindIndex(r => r.Id == (int)parameter);
            ShowDetails(idx, AddedSimpleRequests.ToList<SimpleRequestDTO>());
            NotifyPropertiesChanged();
        }
        private void OnSubmitSimpleRequest()
        {
            if (!ValidateForm())
            {
                MessageBox.Show("Values you have entered are invalid!", "Error");
                return;
            }
            SimpleRequest simpleRequest = new SimpleRequest();
            if (AddSimpleRequest(simpleRequest, "Simple"))
            {
                Requests.Add(new SimpleRequestDTO(Requests.Count + 1,
                                              Description,
                                              NumberOfGuests,
                                              Language,
                                              StartDate,
                                              EndDate,
                                              simpleRequest.GetStatusUri(),
                                              new Location(-1, City, State)));
            }
            OnPropertyChanged(nameof(Requests));
            OnCancelSimpleRequest();
        }
        private bool AddSimpleRequest(SimpleRequest simpleRequest, string type)
        {
            Location location = new Location(-1, City, State);
            location.Id = !_locationService.Contains(location) ? _locationService.MakeID() : _locationService.GetId(location.State, location.City);
            _locationService.Add(location);
            if (StartDate.ToDateTime().Date <= DateTime.Now.AddDays(2))
            {
                MessageBox.Show("The start date of request should be at least 48h earlier than actual date of tour", "Alert");
                return false;
            }

            simpleRequest.User.Id = User.Id;
            simpleRequest.Description = Description;
            simpleRequest.Location.Id = location.Id;
            simpleRequest.Location.City = _locationService.GetById(location.Id).City;
            simpleRequest.Location.State = _locationService.GetById(location.Id).State;
            simpleRequest.Language = Language;
            simpleRequest.NumberOfGuests = NumberOfGuests;
            simpleRequest.DateRange.StartDate = StartDate;
            simpleRequest.DateRange.EndDate = EndDate;
            simpleRequest.Status = SimpleRequestStatus.ON_HOLD;

            if(type.Equals("Simple"))
                _simpleRequestService.Add(simpleRequest);

            return true;
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
        private bool ValidateForm()
        {
            if (Description.IsNullOrWhiteSpace()) return false;
            if(Language.IsNullOrWhiteSpace()) return false;
            if (NumberOfGuests < 0) return false;
            if(City.IsNullOrWhiteSpace()) return false;
            if (State.IsNullOrWhiteSpace()) return false;
            return true;
        }
    }
}

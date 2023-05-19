using application.UseCases;
using booking.Commands;
using booking.Model;
using booking.View.Guide;
using booking.WPF.ViewModels;
using Domain.DTO;
using Domain.Model;
using Syncfusion.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using WPF.Views.Guide;

namespace WPF.ViewModels
{
    class TourRequestAcceptanceViewModel : BaseViewModel,INotifyPropertyChanged
    {
        public User Guide { get; set; }   
        public ObservableCollection<SimpleAndComplexTourRequestsDTO> AllRequests { get; set; }
        private SimpleRequestService _simpleRequestsService { get; set; }
        public SimpleAndComplexTourRequestsDTO SelectedTourRequest { get; set; }
        public ObservableCollection<string> States { get; set; }
        public string SelectedCity { get; set; }
        public string Language { get; set; }
        public string MaxGuests { get; set; }
        public string SelectedState { get; set; }
        public DateTime SelectedStartDate { get; set; }
        private DateTime selectedEndDate;
        public DateTime SelectedEndDate 
        {
            get { return selectedEndDate; }
            set
            {
                selectedEndDate = value;
                OnPropertyChanged(nameof(selectedEndDate));
            }
        }

        private DateTime displayDateStart;
        public DateTime DisplayDateStart
        {
            get { return displayDateStart; }
            set
            {
                displayDateStart = value;
                OnPropertyChanged(nameof(DisplayDateStart));
            }
        }
        public ObservableCollection<string> Cities { get; set; }
        private LocationService _locationService { get; set; }
        public ICommand FillCityCBCommand => new RelayCommand(FillCities);
        public ICommand SearchCommand => new RelayCommand(Search);
        public ICommand CutRangeCommand => new RelayCommand(CutRange);
        public ICommand RejectCommand => new RelayCommand(RejectSimpleTourRequest,CanClick);
        public ICommand AcceptCommand => new RelayCommand(AcceptSimpleTourRequest, CanClick);
        private NavigationService navigationService;
        public TourRequestAcceptanceViewModel(User guide, NavigationService navigationService) 
        {
            Guide = guide;
            AllRequests = new ObservableCollection<SimpleAndComplexTourRequestsDTO>();
            Cities= new ObservableCollection<string>();
            _simpleRequestsService =new SimpleRequestService();
            _locationService =new LocationService();
            States = new ObservableCollection<string>( _locationService.InitializeListOfStates());
            SelectedStartDate= DateTime.Now;
            SelectedEndDate= DateTime.Now;
            displayDateStart = DateTime.Now;
            this.navigationService = navigationService;
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
            foreach (var request in _simpleRequestsService.CreateListOfSimpleRequests()) 
            {
                    AllRequests.Add(request);
            }
        }
        private void FillCities()
        {
            Cities.Clear();
            foreach(var state in _locationService.FillListWithCities(SelectedState))
            {
                Cities.Add(state);
            }
        }
        private void Search()
        {
            LoadRequests();
            if(!string.IsNullOrEmpty(SelectedState))
                FilterState();
            if (!string.IsNullOrEmpty(SelectedCity))
                FilterCity();
            if (!string.IsNullOrEmpty(Language))
                FilterLanguage();
            if (!string.IsNullOrEmpty(MaxGuests))
                FilterNumberOfGuests();
            if (SelectedStartDate.Date.Date>=DateTime.Now.Date)
                FilterStartDate();
            if (SelectedEndDate.Date.Date!= SelectedStartDate.Date.Date)
                FilterEndDate();
            OnPropertyChanged(nameof(AllRequests));
        }
        private void CutRange() 
        {
            DisplayDateStart = SelectedStartDate;
            if(SelectedStartDate>selectedEndDate)
                SelectedEndDate = SelectedStartDate;
            OnPropertyChanged(nameof(DisplayDateStart));
        }
        private void FilterState()
        {
            List<SimpleAndComplexTourRequestsDTO> list = AllRequests.Where(req => req.Location.State == SelectedState).ToList();
            AllRequests = new ObservableCollection<SimpleAndComplexTourRequestsDTO>(list);
        }
        private void FilterCity()
        {
            List<SimpleAndComplexTourRequestsDTO> list = AllRequests.Where(req => req.Location.City == SelectedCity).ToList();
            AllRequests = new ObservableCollection<SimpleAndComplexTourRequestsDTO>(list);
        }
        private void FilterLanguage()
        {
            List<SimpleAndComplexTourRequestsDTO> list = AllRequests.Where(req => req.Language.ToLower() == Language.ToLower()).ToList();
            AllRequests = new ObservableCollection<SimpleAndComplexTourRequestsDTO>(list);

        }
        private void FilterNumberOfGuests()
        {
            List<SimpleAndComplexTourRequestsDTO> list = AllRequests.Where(req => req.NumberOfGuests<=Convert.ToInt32(MaxGuests)).ToList();
            AllRequests = new ObservableCollection<SimpleAndComplexTourRequestsDTO>(list);
        }
        private void FilterStartDate()
        {
            List<SimpleAndComplexTourRequestsDTO> list = AllRequests.Where(req => req.StartDate.Date >= SelectedStartDate).ToList();
            AllRequests = new ObservableCollection<SimpleAndComplexTourRequestsDTO>(list);
        }
        private void FilterEndDate()
        {
            List<SimpleAndComplexTourRequestsDTO> list = AllRequests.Where(req => req.EndDate.Date <= SelectedEndDate).ToList();
            AllRequests = new ObservableCollection<SimpleAndComplexTourRequestsDTO>(list);
        }
        private void RejectSimpleTourRequest()
        {
            SimpleRequest simpleRequest = _simpleRequestsService.GetById(SelectedTourRequest.SimpleRequestId);
            simpleRequest.Status = SimpleRequestStatus.INVALID;
            _simpleRequestsService.Update(simpleRequest);
            if(MessageBox.Show("Are you sure you want to reject this tour?", "Warning", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning)==MessageBoxResult.Yes)
                MessageBox.Show("Tour rejected!", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
            LoadRequests();
            OnPropertyChanged(nameof(AllRequests));
            
        }
        private void AcceptSimpleTourRequest()
        {
            SelectDateForTourRequestWindow window=new SelectDateForTourRequestWindow(SelectedTourRequest);
            window.ShowDialog();

            bool accept = SelectDateForTourRequestViewModel.Accept;
            DateTime SelectedDate = SelectDateForTourRequestViewModel.selectedDate;
            if (accept)
                navigationService.Navigate(new AddTourWindow(SelectedTourRequest, SelectedDate,false));

        }
        private bool CanClick()
        {
            return SelectedTourRequest != null;
        }
    }
}

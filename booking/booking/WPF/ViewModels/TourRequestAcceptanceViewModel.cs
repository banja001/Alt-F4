using application.UseCases;
using booking.application.UseCases;
using booking.Commands;
using booking.Model;
using booking.View.Guide;
using booking.WPF.ViewModels;
using Domain.DTO;
using Domain.Model;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Syncfusion.Windows.Controls;
using Syncfusion.Windows.Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Effects;
using System.Windows.Navigation;
using WPF.Views.Guide;
using System.Threading;
using System.Threading.Tasks;
using Syncfusion.Windows.Tools;
using Org.BouncyCastle.Utilities;

namespace WPF.ViewModels
{
    class TourRequestAcceptanceViewModel : BaseViewModel,INotifyPropertyChanged
    {
        public User Guide { get; set; }   
        public ObservableCollection<SimpleAndComplexTourRequestsDTO> AllRequests { get; set; }
        private SimpleRequestService _simpleRequestsService { get; set; }
        private SimpleAndComplexTourRequestsDTO selectedTourRequest;

        public SimpleAndComplexTourRequestsDTO SelectedTourRequest
        {
            get { return selectedTourRequest; }
            set
            {
                selectedTourRequest = value;
                OnPropertyChanged(nameof(SelectedTourRequest));
            }
        }
        public ObservableCollection<string> States { get; set; }
        private string selectedCity;
        private string language;
        private string maxGuests;
        private string selectedState;

        public string SelectedCity
        {
            get { return selectedCity; }
            set
            {
                selectedCity = value;
                OnPropertyChanged(nameof(SelectedCity));
            }
        }

        public string Language
        {
            get { return language; }
            set
            {
                language = value;
                OnPropertyChanged(nameof(Language));
            }
        }

        public string MaxGuests
        {
            get { return maxGuests; }
            set
            {
                try
                {

                    int maxGuestsTemp = Convert.ToInt32( value);
                    maxGuests = maxGuestsTemp.ToString();
                    OnPropertyChanged(nameof(MaxGuests));
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }

        public string SelectedState
        {
            get { return selectedState; }
            set
            {
                selectedState = value;
                OnPropertyChanged(nameof(SelectedState));
            }
        }


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
        private DateTime selectedStartDate;

        public DateTime SelectedStartDate
        {
            get { return selectedStartDate; }
            set
            {
                selectedStartDate = value;
                OnPropertyChanged(nameof(SelectedStartDate));
            }
        }
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
        public ICommand TooltipSearchCommand => new RelayCommand(SearchToolTipF);

        private bool searchTooltip;

        public bool SearchTooltip
        {
            get { return searchTooltip; }
            set
            {
                if (searchTooltip != value)
                {
                    searchTooltip = value;
                    OnPropertyChanged(nameof(SearchTooltip));
                }
            }
        }
        private void SearchToolTipF()
        {
            SearchTooltip = !SearchTooltip;
        }
        public ICommand FillCityCBCommand => new RelayCommand(FillCities);
        public ICommand SearchCommand => new RelayCommand(Search);
        public ICommand CancelSearchCommand => new RelayCommand(CancelSearch);
        public ICommand CutRangeCommand => new RelayCommand(CutRange);
        public ICommand RejectCommand => new RelayCommand(RejectSimpleTourRequest,CanClick);
        public ICommand AcceptCommand => new RelayCommand(AcceptSimpleTourRequestForButton, CanClick);
        public ICommand GenerateReportCommand => new RelayCommand(OpenReportWindow);
        public ICommand DemoCommand => new RelayCommand(StartStopDemo);

        private string preDemoState;
        private string preDemoCity;
        private string preDemoLanguage;
        private string preDemoNumberOfGuests;
        private DateTime preDemoStartDate;
        private DateTime preDemoEndDate;
        private List<SimpleAndComplexTourRequestsDTO> preDemoRequests;

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
            DemoName = "Demo";
            preDemoRequests = new List<SimpleAndComplexTourRequestsDTO>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void LoadRequests()
        {
            
            AllRequests.Clear();
            foreach (var request in _simpleRequestsService.CreateListOfSimpleRequestsDTO()) 
            {
                if (request.IsPartOfComplex)
                {
                    if (CheckComplexTour(request.SimpleRequestId))
                    {
                        AllRequests.Add(request);
                    }
                }
                else
                {

                    AllRequests.Add(request);
                }
            }
        }
        private bool CheckComplexTour(int id)
        {
            List<ComplexRequest>allComplex=new List<ComplexRequest>();
            foreach (var sr in _simpleRequestsService.FindAllSimpleTourInSimpleRequestsTours(Guide.Id))
            {
                if(_simpleRequestsService.CheckWhichComplexTour(sr.Id)!=null)
                    allComplex.Add(_simpleRequestsService.CheckWhichComplexTour(sr.Id));
            }
            foreach (ComplexRequest cr in allComplex)
            {
                if(cr.SimpleRequests.Find(sr => sr.Id == id)!=null)
                    return false;
            }
            return true;
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
                AllRequests=new ObservableCollection<SimpleAndComplexTourRequestsDTO>( _simpleRequestsService.FilterState(AllRequests.ToList(),SelectedState));
            if (!string.IsNullOrEmpty(SelectedCity))
                AllRequests = new ObservableCollection<SimpleAndComplexTourRequestsDTO>(_simpleRequestsService.FilterCity(AllRequests.ToList(), SelectedCity));
            if (!string.IsNullOrEmpty(Language))
                AllRequests = new ObservableCollection<SimpleAndComplexTourRequestsDTO>(_simpleRequestsService.FilterLanguage(AllRequests.ToList(), Language));
            if (!string.IsNullOrEmpty(MaxGuests))
                AllRequests = new ObservableCollection<SimpleAndComplexTourRequestsDTO>(_simpleRequestsService.FilterNumberOfGuests(AllRequests.ToList(), MaxGuests));
            if (SelectedStartDate.Date.Date>=DateTime.Now.Date)
                AllRequests = new ObservableCollection<SimpleAndComplexTourRequestsDTO>(_simpleRequestsService.FilterStartDate(AllRequests.ToList(), SelectedStartDate));
            if (SelectedEndDate.Date.Date!= SelectedStartDate.Date.Date)
                AllRequests = new ObservableCollection<SimpleAndComplexTourRequestsDTO>(_simpleRequestsService.FilterEndDate(AllRequests.ToList(), SelectedEndDate));
            OnPropertyChanged(nameof(AllRequests));
        }
        private void CancelSearch()
        {
            LoadRequests();
            SelectedState = "";
            SelectedCity = "";
            SelectedStartDate = DateTime.Now;
            Language = "";
            MaxGuests = "";
            SelectedEndDate = selectedStartDate ; 
            OnPropertyChanged(nameof(AllRequests));
        }
        private void CutRange() 
        {
            DisplayDateStart = SelectedStartDate;
            if(SelectedStartDate>selectedEndDate)
                SelectedEndDate = SelectedStartDate;
            OnPropertyChanged(nameof(DisplayDateStart));
        }
      
        private void RejectSimpleTourRequest()
        {
           
            if(MessageBox.Show("Are you sure you want to reject this tour?", "Warning", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning)==MessageBoxResult.Yes)
            {
                SimpleRequest simpleRequest = _simpleRequestsService.GetById(SelectedTourRequest.SimpleRequestId);
                simpleRequest.Status = SimpleRequestStatus.INVALID;
                _simpleRequestsService.Update(simpleRequest);
                MessageBox.Show("Tour rejected!", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
            }
                
            LoadRequests();
            OnPropertyChanged(nameof(AllRequests));
            
        }
        private void AcceptSimpleTourRequestForButton()
        {
            AcceptSimpleTourRequest(false);
        }
        private void AcceptSimpleTourRequest(bool IsDemoOn)
        {
            Window window = System.Windows.Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
            if (window != null)
            {
                window.Effect = new BlurEffect();
            }
            SelectDateForTourRequestWindow selwindow=new SelectDateForTourRequestWindow(SelectedTourRequest,IsDemoOn);
            selwindow.ShowDialog();
            window.Effect = null;
            bool accept = SelectDateForTourRequestViewModel.Accept;
            DateTime SelectedDate = SelectDateForTourRequestViewModel.selectedDate;
            if (accept)
                navigationService.Navigate(new AddTourWindow(SelectedTourRequest, SelectedDate,false,Guide));

        }
        private bool CanClick()
        {
            return SelectedTourRequest != null;
        }
        private void OpenReportWindow()
        {
            Window window = System.Windows.Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
            if (window != null)
            {
                window.Effect = new BlurEffect();
            }
            ReportWindow reportWindow= new ReportWindow(Guide);
            reportWindow.ShowDialog();
            window.Effect = null;
        }
        private void StartStopDemo()
        {
            if (DemoOn)
            {
                SelectedCity = preDemoCity;
                SelectedEndDate = preDemoEndDate;
                Language = preDemoLanguage;
                MaxGuests = preDemoNumberOfGuests;
                SelectedStartDate = preDemoStartDate;
                SelectedState = preDemoState;
                AllRequests.Clear();
                foreach (SimpleAndComplexTourRequestsDTO s in preDemoRequests)
                {
                    AllRequests.Add(s);
                }
                cts.Cancel();
                DemoOn = !DemoOn;
                DemoName = "Demo";
                MessageBox.Show("Demo has been stopped!", "Demo message", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                preDemoCity = SelectedCity;
                preDemoEndDate = SelectedEndDate;
                preDemoLanguage = Language;
                preDemoNumberOfGuests = MaxGuests;
                preDemoStartDate = SelectedStartDate;
                preDemoState = SelectedState;
                preDemoRequests = AllRequests.ToList();
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
                StartDemoWindow sdw=new StartDemoWindow();
                sdw.Show();

                await Task.Delay(2000, ct);
                sdw.Close();
                SelectedState = "Nemacka";

                await Task.Delay(2000, ct);
                SelectedCity = "Berlin";
                await Task.Delay(2000, ct);
                SelectedStartDate = DateTime.Now.AddDays(3);
                await Task.Delay(2000, ct);
                Language = "Engleski";
                await Task.Delay(2000, ct);
                MaxGuests = "30";
                await Task.Delay(2000, ct);
                SelectedEndDate = new DateTime(2023,11,30);
                await Task.Delay(2000, ct);
                Search();
                await Task.Delay(1000, ct);
                SelectedTourRequest = AllRequests[0];
                //MessageBox.Show("New window will open!", "Demo message", MessageBoxButton.OK, MessageBoxImage.Information);

                await Task.Delay(1000, ct);

                AcceptSimpleTourRequest(true);
                DemoOn = !DemoOn;
                DemoName = "Demo";
                
            }
        }

    }
}

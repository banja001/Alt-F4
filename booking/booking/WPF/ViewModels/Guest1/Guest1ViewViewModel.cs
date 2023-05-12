using application.UseCases;
using booking.application.usecases;
using booking.Commands;
using booking.DTO;
using booking.Model;
using booking.View.Guest1;
using booking.WPF.ViewModels;
using Domain.Model;
using Egor92.MvvmNavigation.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WPF.ViewModels.Guest1
{
    public class Guest1ViewViewModel : BaseViewModel, INotifyPropertyChanged
    {
        public static ObservableCollection<AccommodationLocationDTO> AccommodationDTOs { get; set; }
        public static AccommodationLocationDTO SelectedAccommodation { get; set; }
        public SearchedAccomodationDTO SearchedAccommodation { get; set; }
        public ObservableCollection<string> States { get; set; }
        public ObservableCollection<string> Cities { get; set; }

        private bool citiesComboBoxEnabled;
        public bool CitiesComboBoxEnabled
        {
            get { return citiesComboBoxEnabled; }
            set
            {
                if(citiesComboBoxEnabled != value)
                {
                    citiesComboBoxEnabled = value;
                    OnPropertyChanged();
                }
            }
        }

        private int selectedIndexTabControl;
        public int SelectedIndexTabControl 
        {
            get { return selectedIndexTabControl; }
            set
            {
                if(selectedIndexTabControl != value)
                {
                    selectedIndexTabControl = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool ApartmentChecked { get; set; }
        public bool HouseChecked { get; set; }
        public bool CabinChecked { get; set; }

        public string SelectedState { get; set; }
        public string SelectedCity { get; set; }

        private int userId;
        public string UserName { get; set; }
        public string Score { get; set; }

        private List<Guest1Notifications> guest1Notifications;


        private readonly NotificationsService _notificationsService;
        private readonly AccommodationService _accommodationService;
        private readonly UserService _userService;
        private readonly LocationService _locationService;
        private readonly Guest1NotificationsService _guest1NotificationsService;

        public ICommand SearchAccommodationsCommand => new RelayCommand(SearchAccommodations);
        public ICommand ReserveAccommodationsCommand => new RelayCommand(ReserveAccommodations);
        public ICommand OpenImagesCommand => new RelayCommand(OpenImages);
        public ICommand SeeAllCommand => new RelayCommand(SeeAll);
        public ICommand StateSelectionChangedCommand => new RelayCommand(StateSelectionChanged);

        public ICommand OpenFirstTabCommand => new RelayCommand(OpenFirstTab);
        public ICommand OpenSecondTabCommand => new RelayCommand(OpenSecondTab);
        public ICommand OpenThirdTabCommand => new RelayCommand(OpenThirdTab);
        public ICommand OpenFourthTabCommand => new RelayCommand(OpenFourthTab);

        public Guest1ViewViewModel(int userId)
        {
            _notificationsService = new NotificationsService();
            _accommodationService = new AccommodationService();
            _userService = new UserService();
            _locationService = new LocationService();
            _guest1NotificationsService = new Guest1NotificationsService();

            this.userId = userId;

            SelectedIndexTabControl = 0;

            States = new ObservableCollection<string>();
            Cities = new ObservableCollection<string>();

            guest1Notifications = _guest1NotificationsService.GetAllByGuest1Id(userId);

            InitializeStatus();
            InitializeDTOs();
            FillStateComboBox();

            _notificationsService.NotifyGuest1(userId);
        }

        private void InitializeStatus()
        {
            UserName = _userService.GetUserNameById(userId);
            Score = _userService.GetScoreById(userId).ToString();
        }

        private void InitializeDTOs()
        {
            SearchedAccommodation = new SearchedAccomodationDTO();
            AccommodationDTOs = _accommodationService.SortAccommodationDTOs(_accommodationService.CreateAccomodationDTOs());
        }

        private void FillStateComboBox()
        {
            List<Location> locations = _locationService.GetAll();
            foreach (Location location in locations)
            {
                String state = location.State;
                if (!States.Contains(state))
                    States.Add(state);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void SearchAccommodations()
        {
            SetAccommodationTypes();

            SearchedAccommodation.City = (SelectedCity == null) ? "" : SelectedCity;
            SearchedAccommodation.Country = (SelectedState == null) ? "" : SelectedState;

            List<AccommodationLocationDTO> accommodationList = _accommodationService.CreateAccomodationDTOs().ToList();


            while(AccommodationDTOs.Count > 0)
            {
                AccommodationDTOs.RemoveAt(0);
            }

            foreach (AccommodationLocationDTO accommodation in accommodationList)
            {
                AddAccommodationToList(accommodation);
            }

            ObservableCollection<AccommodationLocationDTO> SortedAccommodationDTOs = _accommodationService.SortAccommodationDTOs(AccommodationDTOs);

            while (AccommodationDTOs.Count > 0)
            {
                AccommodationDTOs.RemoveAt(0);
            }

            foreach (var sortedAccommodation in SortedAccommodationDTOs)
            {
                AccommodationDTOs.Add(sortedAccommodation);
            }
        }

        private void SetAccommodationTypes()
        {
            SearchedAccommodation.Type.Clear();
            if (ApartmentChecked)
                SearchedAccommodation.Type.Add("Apartment");
            if (HouseChecked)
                SearchedAccommodation.Type.Add("House");
            if (CabinChecked)
                SearchedAccommodation.Type.Add("Cabin");
        }

        private void AddAccommodationToList(AccommodationLocationDTO accommodation)
        {
            bool matchingType = (SearchedAccommodation.Type.Find(u => u == accommodation.Type) != null) ? true : false;
            bool matchingName = string.IsNullOrEmpty(SearchedAccommodation.Name) || accommodation.Name.ToLower().Contains(SearchedAccommodation.Name.ToLower());
            bool matchingLocation = accommodation.Location.Contains(SearchedAccommodation.City + "," + SearchedAccommodation.Country);
            bool matchingNumOfGuests = SearchedAccommodation.NumOfGuests == 0 || accommodation.MaxCapacity >= SearchedAccommodation.NumOfGuests;
            bool matchingNumOfDays = SearchedAccommodation.NumOfDays == 0 || accommodation.MinDaysToUse <= SearchedAccommodation.NumOfDays;

            if (matchingType && matchingName && matchingLocation && matchingNumOfGuests && matchingNumOfDays)
                AccommodationDTOs.Add(accommodation);
        }

        private void OpenImages()
        {
            ImagesOverview imagesOverview = new ImagesOverview(SelectedAccommodation);
            imagesOverview.Show();
        }

        private void SeeAll()
        {
            while (AccommodationDTOs.Count > 0)
            {
                AccommodationDTOs.RemoveAt(0);
            }

            ObservableCollection<AccommodationLocationDTO> SortedAccommodationDTOs = _accommodationService.SortAccommodationDTOs(_accommodationService.CreateAccomodationDTOs());

            foreach (var sortedAccommodation in SortedAccommodationDTOs)
            {
                AccommodationDTOs.Add(sortedAccommodation);
            }
        }

        private void ReserveAccommodations()
        {
            if (SelectedAccommodation != null)
            {
                ReserveAccommodation reserveAccommodation = new ReserveAccommodation(userId);
                reserveAccommodation.ShowDialog();
            }
            else
            {
                MessageBox.Show("You have to select an accommodation you wish to reserve", "Warning");
            }
        }
        private void StateSelectionChanged()
        {
            List<Location> locations = _locationService.GetAll();

            while(Cities.Count > 0)
            {
                Cities.RemoveAt(0);
            }

            foreach (Location location in locations)
            {
                String city = location.City;
                bool isValid = !Cities.Contains(city) && SelectedState.Equals(location.State);
                if (isValid)
                    Cities.Add(city);
            }

            CitiesComboBoxEnabled = true;
        }

        private void OpenFirstTab()
        {
            SelectedIndexTabControl = 0;
        }

        private void OpenSecondTab()
        {
            SelectedIndexTabControl = 1;
        }

        private void OpenThirdTab()
        {
            SelectedIndexTabControl = 2;
        }

        private void OpenFourthTab()
        {
            SelectedIndexTabControl = 3;
        }
    }
}

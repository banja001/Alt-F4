using application.UseCases;
using booking.application.usecases;
using booking.application.UseCases;
using booking.Commands;
using booking.DTO;
using booking.Model;
using booking.View.Guest1;
using Domain.Model;
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
    public class OverviewViewModel : INotifyPropertyChanged
    {
        public static ObservableCollection<AccommodationLocationDTO> AccommodationDTOs { get; set; }
        public static AccommodationLocationDTO SelectedAccommodation { get; set; }

        private SearchedAccomodationDTO searchedAccommodation;
        public SearchedAccomodationDTO SearchedAccommodation 
        {
            get { return searchedAccommodation; }
            set
            {
                if (searchedAccommodation != value)
                {
                    searchedAccommodation = value;
                    OnPropertyChanged(nameof(SearchedAccommodation));
                }
            } 
        }
        public ObservableCollection<string> States { get; set; }
        public ObservableCollection<string> Cities { get; set; }

        private bool searchButtonEnabled;
        public bool SearchButtonEnabled
        {
            get { return searchButtonEnabled; }
            set
            {
                if(searchButtonEnabled != value)
                {
                    searchButtonEnabled = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool reserveButtonEnabled;
        public bool ReserveButtonEnabled
        {
            get { return reserveButtonEnabled; }
            set
            {
                if (reserveButtonEnabled != value)
                {
                    reserveButtonEnabled = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool citiesComboBoxEnabled;
        public bool CitiesComboBoxEnabled
        {
            get { return citiesComboBoxEnabled; }
            set
            {
                if (citiesComboBoxEnabled != value)
                {
                    citiesComboBoxEnabled = value;
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

        private List<Guest1Notifications> guest1Notifications;

        private Page selectedRadioButton;
        public Page SelectedRadioButton
        {
            get { return selectedRadioButton; }
            set
            {
                if (selectedRadioButton != value)
                {
                    selectedRadioButton = value;
                    OnPropertyChanged();
                }
            }
        }

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
        public ICommand AccommodationSelectionChangedCommand => new RelayCommand(AccommodationSelectionChanged);
        public ICommand NumValueChangedCommand => new RelayCommand(NumValueChanged);

        public OverviewViewModel(int id)
        {
            _notificationsService = new NotificationsService();
            _accommodationService = new AccommodationService();
            _userService = new UserService();
            _locationService = new LocationService();
            _guest1NotificationsService = new Guest1NotificationsService();

            this.userId = id;

            States = new ObservableCollection<string>();
            Cities = new ObservableCollection<string>();

            guest1Notifications = _guest1NotificationsService.GetAllByGuest1Id(userId);
            _notificationsService.NotifyGuest1(userId);

            InitializeDTOs();
            InitializeCheckBoxes();
            FillStateComboBox();
        }

        private void InitializeDTOs()
        {
            RenovationDatesService _renovationService=new RenovationDatesService();
            SearchedAccommodation = new SearchedAccomodationDTO();

            //dodaje (New) ukoliko je skoro renoviran
            ObservableCollection<AccommodationLocationDTO> accList = _accommodationService.CreateAccomodationDTOs();
            foreach(AccommodationLocationDTO acc in accList)
            {
                foreach(RenovationDates renovation in _renovationService.GetAll())
                {
                    if(renovation.EndDate<=DateTime.Now && renovation.EndDate<DateTime.Now.AddYears(1) && renovation.AccommodationId == acc.AccommodationId && !acc.Name.Contains("(New)"))
                    {
                        acc.Name += "(New)";
                    }
                }
            }



            AccommodationDTOs = _accommodationService.SortAccommodationDTOs(accList);
        }

        private void InitializeCheckBoxes()
        {
            ApartmentChecked = true;
            HouseChecked = true;
            CabinChecked = true;
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
            if (SearchedAccommodation.IsValid)
            {
                SetAccommodationTypes();

                SearchedAccommodation.City = (SelectedCity == null) ? "" : SelectedCity;
                SearchedAccommodation.Country = (SelectedState == null) ? "" : SelectedState;

                List<AccommodationLocationDTO> accommodationList = _accommodationService.CreateAccomodationDTOs().ToList();


                while (AccommodationDTOs.Count > 0)
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
            if (SelectedAccommodation != null && ReserveButtonEnabled)
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

            while (Cities.Count > 0)
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

        private void AccommodationSelectionChanged()
        {
            ReserveButtonEnabled = (SelectedAccommodation != null) ? true : false;
        }

        private void NumValueChanged()
        {
            SearchButtonEnabled = SearchedAccommodation.IsValid;
        }
    }
}

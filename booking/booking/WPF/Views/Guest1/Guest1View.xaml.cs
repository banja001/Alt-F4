using booking.Domain.DTO;
using booking.Domain.Model;
using booking.DTO;
using booking.Model;
using booking.Repositories;
using booking.Repository;
using booking.View.Guest1;
using booking.WPF.Views.Guest1;
using Domain.Model;
using Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WPF.ViewModels.Guest1;

namespace booking.View
{
    /// <summary>
    /// Interaction logic for AccomodationOverview.xaml
    /// </summary>
    public partial class Guest1View : Window
    {
        public static ObservableCollection<AccommodationLocationDTO> AccommodationDTOs { get; set; }

        public Image SelectedAddedImages { get; set; }
        public static AccommodationLocationDTO SelectedAccommodation { get; set; }
        
        
        
        public SearchedAccomodationDTO SearchedAccommodation { get; set; }

        private readonly AccommodationRepository _accomodationRepository;
        private readonly LocationRepository _locationRepository;
        private readonly ReservedDatesRepository _reservedDatesRepository;
        private readonly ReservationRequestsRepository _reservationRequestsRepository;
        private readonly OwnerRatingRepository _ownerRatingRepository;
        private readonly OwnerRatingImageRepository _ownerRatingImageRepository;
        private readonly Guest1NotificationsRepository _guest1NotificationsRepository;

        
        private readonly OwnerNotificationRepository _ownerNotificationRepository;
        private readonly UserRepository _userRepository;
        public List<User> users { get; set; }
        
        public string SelectedState { get; set; }
        public string SelectedCity { get; set; }

        public ObservableCollection<string> States { get; set; }

        public SignInForm signInWindow { get; set; }

        private int userId;

        private List<Guest1Notifications> guest1Notifications;

        private ReservationsViewModel _reservationViewModel;
        private RateAccommodationAndOwnerViewModel _rateAccommodationAndOwner;

        public Guest1View(int id,SignInForm sign)
        {
            InitializeComponent();
            userId = id;

            _reservationViewModel = new ReservationsViewModel(userId, this);
            _rateAccommodationAndOwner = new RateAccommodationAndOwnerViewModel(userId, this);

            tabItemOverview.DataContext = this;
            tabItemRate.DataContext = _rateAccommodationAndOwner;
            tabItemReservations.DataContext = _reservationViewModel;
            tabItemForums.DataContext = this;

            signInWindow = sign;
            
            _accomodationRepository = new AccommodationRepository();
            _locationRepository = new LocationRepository();
            _reservedDatesRepository = new ReservedDatesRepository();
            _reservationRequestsRepository = new ReservationRequestsRepository();
            _ownerRatingRepository = new OwnerRatingRepository();
            _ownerRatingImageRepository = new OwnerRatingImageRepository();
            _ownerNotificationRepository = new OwnerNotificationRepository();
            _userRepository = new UserRepository();
            _guest1NotificationsRepository = new Guest1NotificationsRepository();

            users = _userRepository.GetAll();
            SearchedAccommodation = new SearchedAccomodationDTO();

            AccommodationDTOs = CreateAccomodationDTOs(_accomodationRepository.GetAll());
            AccommodationDTOs = SortAccommodationDTOs();
            accommodationData.ItemsSource=AccommodationDTOs;


            States = new ObservableCollection<string>();
            guest1Notifications = _guest1NotificationsRepository.GetAllByGuest1Id(userId);

            InitializeDTOs();
            FillStateComboBox();
            InitializeCheckBoxes();

            Guest1ViewViewModel guest1ViewViewModel = new Guest1ViewViewModel(userId);
        }

        private void InitializeDTOs()
        {
            SearchedAccommodation = new SearchedAccomodationDTO();

            AccommodationDTOs = CreateAccomodationDTOs(_accomodationRepository.GetAll());
        }

        private void InitializeCheckBoxes()
        {
            CheckBoxApartment.IsChecked = true;
            CheckBoxCabin.IsChecked = true;
            CheckBoxHouse.IsChecked = true;
        }

        private void FillStateComboBox()
        {
            List<Location> locations = _locationRepository.GetAll();
            foreach (Location location in locations)
            {
                String state = location.State;
                if (!States.Contains(state))
                    States.Add(state);
            }
        }

        public ObservableCollection<AccommodationLocationDTO> CreateAccomodationDTOs(List<Accommodation> accommodations)
        {
            List<Location> locations = _locationRepository.GetAll();
            ObservableCollection<AccommodationLocationDTO> accommodationLocations = new ObservableCollection<AccommodationLocationDTO>();
            AccommodationLocationDTO accommodationLocation;

            foreach (Accommodation accommodation in accommodations)
            {
                accommodationLocation = CreateAccommodationLocation(locations, accommodation);

                accommodationLocations.Add(accommodationLocation);
            }

            return accommodationLocations;
        }

              
        private static AccommodationLocationDTO CreateAccommodationLocation(List<Location> locations, Accommodation accommodation)
        {
            AccommodationLocationDTO accommodationLocation;
            string locationCity = locations.Find(u => u.Id == accommodation.LocationId).City;
            string locationCountry = locations.Find(u => u.Id == accommodation.LocationId).State;

            accommodationLocation = new AccommodationLocationDTO(accommodation.Id, accommodation.Name, locationCity + "," + locationCountry,
                accommodation.Type, accommodation.MaxCapacity, accommodation.MinDaysToUse, accommodation.MinDaysToCancel,accommodation.Id);
            return accommodationLocation;
        }

        private void SearchAccommodations(object sender, RoutedEventArgs e)
        {
            SetAccommodationTypes();

            SearchedAccommodation.City = CityComboBox.Text;
            SearchedAccommodation.Country = StateComboBox.Text;

            SearchedAccommodation.City = (SearchedAccommodation.City == null) ? "" : SearchedAccommodation.City;
            SearchedAccommodation.Country = (SearchedAccommodation.Country == null) ? "" : SearchedAccommodation.Country;

            List<AccommodationLocationDTO> accommodationList = CreateAccomodationDTOs(_accomodationRepository.GetAll()).ToList();

            AccommodationDTOs.Clear();

            foreach (AccommodationLocationDTO accommodation in accommodationList)
            {
                AddAccommodationToList(accommodation);
            }
            
            ObservableCollection<AccommodationLocationDTO> SortedAccommodationDTOs = SortAccommodationDTOs();
            accommodationData.ItemsSource = SortedAccommodationDTOs;
        }

        public ObservableCollection<AccommodationLocationDTO> SortAccommodationDTOs()
        {
            List<Accommodation> accommodations = _accomodationRepository.GetAll();
            ObservableCollection<AccommodationLocationDTO> SortedAccommodationDTOs = new ObservableCollection<AccommodationLocationDTO>();
            bool flag;
            Accommodation accommodation;
            foreach (var item in AccommodationDTOs)
            {
                accommodation= accommodations.Find(s => s.Id == item.AccommodationId);
                flag = _userRepository.GetAll().Find(s => accommodation.OwnerId == s.Id).Super;
                if (flag)
                {
                    if (!item.Name.Last().Equals("*"))
                        item.Name += "*";
                    SortedAccommodationDTOs.Insert(0, item);
                    
                }
                else
                    SortedAccommodationDTOs.Add(item);
            }
            return SortedAccommodationDTOs;
        }

        private void SetAccommodationTypes()
        {
            SearchedAccommodation.Type.Clear();
            if (CheckBoxApartment.IsChecked == true)
                SearchedAccommodation.Type.Add("Apartment");
            if (CheckBoxCabin.IsChecked == true)
                SearchedAccommodation.Type.Add("Cabin");
            if (CheckBoxHouse.IsChecked == true)
                SearchedAccommodation.Type.Add("House");
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

        private void CheckBoxUnchecked(object sender, RoutedEventArgs e)
        {
            if (CheckBoxApartment.IsChecked == false && CheckBoxCabin.IsChecked == false && CheckBoxHouse.IsChecked == false)
                SearchAccommodationButton.IsEnabled = false;
        }

        private void CheckBoxChecked(object sender, RoutedEventArgs e)
        {
            if (SearchAccommodationButton.IsEnabled == false)
                SearchAccommodationButton.IsEnabled = true;
        }

        private void OpenImages(object sender, RoutedEventArgs e)
        {
            ImagesOverview imagesOverview = new ImagesOverview(SelectedAccommodation);
            imagesOverview.Owner = this;
            imagesOverview.Show();
        }

        private void ReserveAccommodations(object sender, RoutedEventArgs e)
        {
            if (SelectedAccommodation != null)
            {
                ReserveAccommodation reserveAccommodation = new ReserveAccommodation(userId);
                reserveAccommodation.Owner = this;
                reserveAccommodation.ShowDialog();
            }
            else
            {
                MessageBox.Show("You have to select an accommodation you wish to reserve", "Warning");
            }
        }

        private void StateSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CityComboBox.IsEnabled = true;
            List<Location> locations = _locationRepository.GetAll();
            List<string> cities = new List<string>();
            foreach (Location location in locations)
            {
                String city = location.City;
                bool isValid = !cities.Contains(city) && SelectedState.Equals(location.State);
                if (isValid)
                    cities.Add(city);
            }
            CityComboBox.ItemsSource = cities;
        }

        private void ChangedNumbersOf(object sender, TextChangedEventArgs e)
        {
            SearchAccommodationButton.IsEnabled = SearchedAccommodation.IsValid;
        }

        private void SeeAll(object sender, RoutedEventArgs e)
        {
            AccommodationDTOs = CreateAccomodationDTOs(_accomodationRepository.GetAll());

            accommodationData.ItemsSource = AccommodationDTOs;
        }

        public void ResetFormInputs()
        {
            stClean.Value = 0;
            stOwner.Value = 0;
            txtbComment.Text = "";
            bSubmitRate.IsEnabled = false;
        }
        
        private void tbImageUrl_TextChanged(object sender, TextChangedEventArgs e)
        {
            bAddImage.IsEnabled = true;
        }

        public void ClearImgUrlTextBox()
        {
            tbImageUrl.Text = "";
        }

        private void CleanStarsClick(object sender, MouseButtonEventArgs e)
        {
            RateAccommodationAndOwnerViewModel.CleanRating = stClean.Value;
        }
        private void OwnersKindnessStarsClick(object sender, MouseButtonEventArgs e)
        {
            RateAccommodationAndOwnerViewModel.OwnersKindenssRating = stOwner.Value;
        }
    }
}

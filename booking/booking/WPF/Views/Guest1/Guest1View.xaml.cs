using booking.Domain.DTO;
using booking.DTO;
using booking.Model;
using booking.Repository;
using booking.View.Guest1;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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

namespace booking.View
{
    /// <summary>
    /// Interaction logic for AccomodationOverview.xaml
    /// </summary>
    public partial class Guest1View : Window
    {
        public static ObservableCollection<AccommodationLocationDTO> AccommodationDTOs { get; set; }

        public static ObservableCollection<ReservationAccommodationDTO> ReservationAccommodationDTOs { get; set; }

        public static AccommodationLocationDTO SelectedAccommodation { get; set; }

        public SearchedAccomodationDTO SearchedAccommodation { get; set; }

        private readonly AccommodationRepository _accomodationRepository;

        private readonly LocationRepository _locationRepository;

        private readonly ReservedDatesRepository _reservedDatesRepository;

        public string SelectedState { get; set; }
        public string SelectedCity { get; set; }

        public ObservableCollection<string> States { get; set; }

        private int userId;

        public Guest1View(int id)
        {
            InitializeComponent();

            DataContext = this;

            userId = id;

            _accomodationRepository = new AccommodationRepository();
            _locationRepository = new LocationRepository();
            _reservedDatesRepository = new ReservedDatesRepository();

            SearchedAccommodation = new SearchedAccomodationDTO();

            AccommodationDTOs = CreateAccomodationDTOs(_accomodationRepository.GetAll());
            ReservationAccommodationDTOs = CreateReservationAccommodationDTOs(_reservedDatesRepository.GetAll());

            

            States = new ObservableCollection<string>();

            FillStateComboBox();
            InitializeCheckBoxes();

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

        public ObservableCollection<ReservationAccommodationDTO> CreateReservationAccommodationDTOs(List<ReservedDates> reservedDates)
        {
            List<ReservedDates> usersReservedDates = reservedDates.Where(d => d.UserId == userId).ToList();
            ObservableCollection<ReservationAccommodationDTO> reservationAccommodationDTOs = new ObservableCollection<ReservationAccommodationDTO>();
            Accommodation accommodation;
            Location location;

            foreach(var date in usersReservedDates)
            {
                accommodation = _accomodationRepository.GetAll().Where(a => a.Id == date.AccommodationId).ToList()[0];
                location = _locationRepository.GetAll().Where(l => l.Id == accommodation.LocationId).ToList()[0];

                reservationAccommodationDTOs.Add(new ReservationAccommodationDTO(accommodation, location, date.StartDate, date.EndDate));
            }

            return reservationAccommodationDTOs;
        }

        private static AccommodationLocationDTO CreateAccommodationLocation(List<Location> locations, Accommodation accommodation)
        {
            AccommodationLocationDTO accommodationLocation;
            string locationCity = locations.Find(u => u.Id == accommodation.LocationId).City;
            string locationCountry = locations.Find(u => u.Id == accommodation.LocationId).State;

            accommodationLocation = new AccommodationLocationDTO(accommodation.Id, accommodation.Name, locationCity + "," + locationCountry,
                accommodation.Type, accommodation.MaxCapacity, accommodation.MinDaysToUse, accommodation.MinDaysToCancel);
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
    }
}

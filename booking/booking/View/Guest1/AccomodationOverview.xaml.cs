using booking.DAO;
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
    public partial class AccomodationOverview : Window
    {
        public static ObservableCollection<AccommodationLocationDTO> Accommodations { get; set; }

        private AccommodationDAO accomodationDAO;

        public SearchedAccomodationDTO searchedAccommodation { get; set; }

        private readonly AccommodationRepository _accomodationRepository;

        private readonly LocationRepository _locationRepository;

        public AccomodationOverview()
        {
            InitializeComponent();
            DataContext = this;

            _accomodationRepository = new AccommodationRepository();
            _locationRepository = new LocationRepository();

            accomodationDAO = new AccommodationDAO();
            searchedAccommodation = new SearchedAccomodationDTO();

            Accommodations = accomodationDAO.getAll(_accomodationRepository.findAll(), _locationRepository);
        }

        private void SearchAccommodations(object sender, RoutedEventArgs e)
        {
            searchedAccommodation.Type.Clear(); //so that previous values dont stay saved in the list
            if (CheckBoxApartment.IsChecked == true)
                searchedAccommodation.Type.Add("Apartment");
            if (CheckBoxCabin.IsChecked == true)
                searchedAccommodation.Type.Add("Cabin");
            if (CheckBoxHouse.IsChecked == true)
                searchedAccommodation.Type.Add("House");

            searchedAccommodation.City = (searchedAccommodation.City == null) ? "" : searchedAccommodation.City;
            searchedAccommodation.Country = (searchedAccommodation.Country == null) ? "" : searchedAccommodation.Country;

            List<AccommodationLocationDTO> accommodationList = accomodationDAO.getAll(_accomodationRepository.findAll(), _locationRepository).ToList();

            Accommodations.Clear();

            foreach(AccommodationLocationDTO accommodation in accommodationList)
            {
                bool matchingType = (searchedAccommodation.Type.Find(u => u == accommodation.Type) != null) ? true : false;
                bool matchingName = string.IsNullOrEmpty(searchedAccommodation.Name) || accommodation.Name == searchedAccommodation.Name;
                bool matchingLocation = accommodation.Location.Contains(searchedAccommodation.City + "," + searchedAccommodation.Country);
                bool matchingNumOfGuests = searchedAccommodation.NumOfGuests == 0 || accommodation.MaxCapacity >= searchedAccommodation.NumOfGuests;
                bool matchingNumOfDays = searchedAccommodation.NumOfDays == 0 || accommodation.MinDaysToUse <= searchedAccommodation.NumOfDays;

                if(matchingType && matchingName && matchingLocation && matchingNumOfGuests && matchingNumOfDays)
                    Accommodations.Add(accommodation);
            }
        }

        private void CheckBoxUnchecked(object sender, RoutedEventArgs e)
        {
            if (CheckBoxApartment.IsChecked == false && CheckBoxCabin.IsChecked == false && CheckBoxHouse.IsChecked == false)
                SearchAccommodationButton.IsEnabled = false;
        }

        private void CheckBoxChecked(object sender, RoutedEventArgs e)
        {
            SearchAccommodationButton.IsEnabled = true;
        }

        private void OpenImages(object sender, RoutedEventArgs e)
        {
            ImagesOverview imagesOverview = new ImagesOverview();
            imagesOverview.Owner = this;
            imagesOverview.Show();
        }
    }
}

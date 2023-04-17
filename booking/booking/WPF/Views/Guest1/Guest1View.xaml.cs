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

namespace booking.View
{
    /// <summary>
    /// Interaction logic for AccomodationOverview.xaml
    /// </summary>
    public partial class Guest1View : Window
    {
        public static ObservableCollection<AccommodationLocationDTO> AccommodationDTOs { get; set; }
        public static ObservableCollection<ReservationAccommodationDTO> ReservationAccommodationDTOs { get; set; }
        public static ObservableCollection<ReservationsRequestsDTO> ReservationRequestsDTOs { get; set; }

        public static ObservableCollection<ReservationAccommodationDTO> StayedInAccommodations { get; set; }
        public static ObservableCollection<Image> AddedImages { get; set; }

        public Image SelectedAddedImages { get; set; }
        public static AccommodationLocationDTO SelectedAccommodation { get; set; }
        
        public static object SelectedFromList { get; set; }

        public static ReservationAccommodationDTO SelectedStayedInAccommodation { get; set; }

        public static ReservationAccommodationDTO SelectedReservation { get; set; }

        public static ReservationsRequestsDTO SelectedReservationRequestDTO { get; set; }
        public SearchedAccomodationDTO SearchedAccommodation { get; set; }

        private readonly AccommodationRepository _accomodationRepository;
        private readonly LocationRepository _locationRepository;
        private readonly ReservedDatesRepository _reservedDatesRepository;
        private readonly ReservationRequestsRepository _reservationRequestsRepository;
        private readonly OwnerRatingRepository _ownerRatingRepository;
        private readonly OwnerRatingImageRepository _ownerRatingImageRepository;
        private readonly Guest1NotificationsRepository _guest1NotificationsRepository;

        public double CleanRating { get; set; }
        public double OwnersKindenssRating { get; set; }

        public string RatingComment { get; set; }

        public string ImageUrl { get; set; }
        private readonly OwnerNotificationRepository _ownerNotificationRepository;
        private readonly UserRepository _userRepository;
        public List<User> users { get; set; }
        
        public string SelectedState { get; set; }
        public string SelectedCity { get; set; }

        

        public List<OwnerRatingImage> OwnerRatingImages { get; set; }
        public ObservableCollection<string> States { get; set; }

        public SignInForm signInWindow { get; set; }

        private int userId;

        private List<Guest1Notifications> guest1Notifications;

        public Guest1View(int id,SignInForm sign)
        {
            InitializeComponent();

            DataContext = this;

            signInWindow = sign;
            userId = id;
            
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
            AccommodationDTOs=SortAccommodationDTOs();///////////////////////////

            ReservationAccommodationDTOs = CreateReservationAccommodationDTOs(_reservedDatesRepository.GetAll());
            ReservationRequestsDTOs = CreateReservationsRequestsDTOs(_reservationRequestsRepository.GetAll());

            States = new ObservableCollection<string>();
            AddedImages = new ObservableCollection<Image>();
            OwnerRatingImages = new List<OwnerRatingImage>();
            guest1Notifications = _guest1NotificationsRepository.GetAllByGuest1Id(userId);

            InitialzeDTOs();
            FillStateComboBox();
            InitializeCheckBoxes();

            if(guest1Notifications.Count != 0)
            {
                Loaded += NotifyGuest1;
            }
        }
        
        private void NotifyGuest1(object sender, RoutedEventArgs e)
        {
            foreach(var notification in guest1Notifications)
            {
                ReservationRequests reservationRequest = _reservationRequestsRepository.GetById(notification.RequestId);

                MessageBox.Show("Your reservation for " + reservationRequest.NewStartDate.ToString("dd/MM/yyyy") + " - "
                    + reservationRequest.NewEndDate.ToString("dd/MM/yyyy")
                    + "has been " + reservationRequest.isCanceled.ToString());
            }

            _guest1NotificationsRepository.RemoveByGuest1I(userId);
            Loaded -= NotifyGuest1;
        }

        private void InitialzeDTOs()
        {
            SearchedAccommodation = new SearchedAccomodationDTO();

            AccommodationDTOs = CreateAccomodationDTOs(_accomodationRepository.GetAll());
            ReservationAccommodationDTOs = CreateReservationAccommodationDTOs(_reservedDatesRepository.GetAll());
            StayedInAccommodations = new ObservableCollection<ReservationAccommodationDTO>(CreateStayedInAccommodations());
            ReservationRequestsDTOs = CreateReservationsRequestsDTOs(_reservationRequestsRepository.GetAll());
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
            
            foreach(var date in usersReservedDates)
            {
                Accommodation accommodation = _accomodationRepository.GetById(date.AccommodationId);
                Location location = _locationRepository.GetById(accommodation.LocationId);

                reservationAccommodationDTOs.Add(new ReservationAccommodationDTO(accommodation, location, date));
            }

            return reservationAccommodationDTOs;
        }

        public List<ReservationAccommodationDTO> CreateStayedInAccommodations()
        {
            return ReservationAccommodationDTOs.Where(r => !_reservedDatesRepository.GetByID(r.ReservationId).RatedByGuest &&
                DateTime.Now >= r.EndDate && (DateTime.Now - r.EndDate).Days <= 5).ToList();
        }

        public ObservableCollection<ReservationsRequestsDTO> CreateReservationsRequestsDTOs(List<ReservationRequests> reservationRequests)
        {
            ObservableCollection<ReservationsRequestsDTO> reservationRequestsDTOs = new ObservableCollection<ReservationsRequestsDTO>();

            foreach(var request in reservationRequests)
            {
                ReservedDates reservedDate = _reservedDatesRepository.GetByID(request.ReservationId);

                Accommodation accommodation = _accomodationRepository.GetById(reservedDate.AccommodationId);
                Location location = _locationRepository.GetById(accommodation.LocationId);

                reservationRequestsDTOs.Add(new ReservationsRequestsDTO(accommodation, location, "Postpone", request.isCanceled.ToString(), request.Id));
            }

            return reservationRequestsDTOs;
        }


        
        private static AccommodationLocationDTO CreateAccommodationLocation(List<Location> locations, Accommodation accommodation)
        {
            AccommodationLocationDTO accommodationLocation;
            string locationCity = locations.Find(u => u.Id == accommodation.LocationId).City;
            string locationCountry = locations.Find(u => u.Id == accommodation.LocationId).State;

            accommodationLocation = new AccommodationLocationDTO(accommodation.Id, accommodation.Name, locationCity + "," + locationCountry,
                accommodation.Type, accommodation.MaxCapacity, accommodation.MinDaysToUse, accommodation.MinDaysToCancel,accommodation.Id);//dodao acc id
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
            //////////////////////////////////////////////////////////////////////////////////
            ObservableCollection<AccommodationLocationDTO> SortedAccommodationDTOs = SortAccommodationDTOs();
            accommodationData.ItemsSource = SortedAccommodationDTOs;
        }

        private ObservableCollection<AccommodationLocationDTO> SortAccommodationDTOs()
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

        private void Postpone(object sender, RoutedEventArgs e)
        {
            PostponeReservation postponeReservation = new PostponeReservation(_reservedDatesRepository.GetByID(SelectedReservation.ReservationId));
            postponeReservation.Owner = this;
            postponeReservation.ShowDialog();

            UpdateDataGrids();
        }

        private void CancelReservation(object sender, RoutedEventArgs e)
        {
            ReservedDates reservedDate = _reservedDatesRepository.GetByID(SelectedReservation.ReservationId);
            AccommodationLocationDTO accomodation = AccommodationDTOs.Where(a => a.Id == reservedDate.AccommodationId).ToList()[0];

            bool isMoreThan24H = accomodation.MinDaysToCancel == 0 && (SelectedReservation.StartDate - DateTime.Now).Hours >= 24;
            bool isMoreThanMinDays = accomodation.MinDaysToCancel <= (SelectedReservation.StartDate - DateTime.Now).Days;

            if (isMoreThan24H || isMoreThanMinDays)
            {
                _reservedDatesRepository.Delete(reservedDate);
                _reservationRequestsRepository.RemoveAllByReservationId(reservedDate.Id);

                UpdateDataGrids();

                int ownerId = _accomodationRepository.GetById(reservedDate.AccommodationId).OwnerId;

                _ownerNotificationRepository.Add(new OwnerNotification(_ownerNotificationRepository.MakeId(), ownerId, accomodation, reservedDate, _userRepository.GetUserNameById(userId)));

                MessageBox.Show("Your reservation is deleted!");
            }
            else
            {
                MessageBox.Show("You can cancle your reservation only 24h or " + accomodation.MinDaysToCancel + "days before!");
            }
        }

        private void UpdateDataGrids()
        {
            ReservationAccommodationDTOs = CreateReservationAccommodationDTOs(_reservedDatesRepository.GetAll());
            ReservationRequestsDTOs = CreateReservationsRequestsDTOs(_reservationRequestsRepository.GetAll());

            reservationsData.ItemsSource = ReservationAccommodationDTOs;
            reservationRequestsData.ItemsSource = ReservationRequestsDTOs;
        }

        private void CleanStarsClick(object sender, MouseButtonEventArgs e)
        {
            CleanRating = stClean.Value;
        }

        private void SubmitRate(object sender, RoutedEventArgs e)
        {
            ReservedDates reservedDate = _reservedDatesRepository.GetByID(SelectedStayedInAccommodation.ReservationId);
            reservedDate.RatedByGuest = true;
            _reservedDatesRepository.Update(reservedDate);

            Accommodation accommodation = _accomodationRepository.GetById(reservedDate.AccommodationId);

            OwnerRating ownerRating = new OwnerRating(_ownerRatingRepository.MakeId(), accommodation.OwnerId, Convert.ToInt32(CleanRating), Convert.ToInt32(OwnersKindenssRating), RatingComment);

            ownerRating.ReservationId = SelectedStayedInAccommodation.ReservationId;

            _ownerRatingRepository.AddRating(ownerRating);

            foreach (var ownerRatingImage in OwnerRatingImages)
            {
                ownerRatingImage.Id = _ownerRatingImageRepository.MakeId();
                _ownerRatingImageRepository.AddOwnerRatingImage(ownerRatingImage);
            }

            StayedInAccommodations.Remove(StayedInAccommodations.Where(a => a.ReservationId == SelectedStayedInAccommodation.ReservationId).ToList()[0]);
            ResetFormInputs();

            MessageBox.Show("Rating successfully added!");
            signInWindow.RefreshUsers();
            AccommodationDTOs = CreateAccomodationDTOs(_accomodationRepository.GetAll());
            AccommodationDTOs = SortAccommodationDTOs();
            accommodationData.ItemsSource = AccommodationDTOs;
        }

        private void ResetFormInputs()
        {
            OwnerRatingImages.Clear();
            AddedImages.Clear();
            lvAddedImages.ItemsSource = AddedImages;
            lbStayedIn.ItemsSource = StayedInAccommodations;
            stClean.Value = 0;
            stOwner.Value = 0;
            txtbComment.Text = "";
            bSubmitRate.IsEnabled = false;
        }

        private void OwnersKindnessStarsClick(object sender, MouseButtonEventArgs e)
        {
            OwnersKindenssRating = stOwner.Value;
        }

        private void lbStayedIn_Selected(object sender, SelectionChangedEventArgs e)
        {
            bSubmitRate.IsEnabled = true;
            if (SelectedFromList != null)
            {
                string[] parts = SelectedFromList.ToString().Split("|");

                ReservationAccommodationDTO stayedInAccommodation = StayedInAccommodations.Where(a => a.StartDate.ToString("dd/MM/yyyy") == parts[2].Split("-")[0] && a.EndDate.ToString("dd/MM/yyyy") == parts[2].Split("-")[1]
                    && a.Location == parts[1] && a.AccommodationName == parts[0]).ToList()[0];

                SelectedStayedInAccommodation = new ReservationAccommodationDTO(stayedInAccommodation);
            }
        }

        private void AddImage(object sender, RoutedEventArgs e)
        {
            
            AddedImages.Add(CreateImageFromBitMap());
            if (!bAddImage.IsEnabled)
            {
                AddedImages.RemoveAt(AddedImages.Count - 1);
                tbImageUrl.Text = "";
                return;
            }

            if(OwnerRatingImages.Find(i => i.Url == ImageUrl) != null)
            {
                AddedImages.RemoveAt(AddedImages.Count - 1);
                tbImageUrl.Text = "";
                MessageBox.Show("You have already added that image, please choose a different one!", "Warning");
                return;
            }
            OwnerRatingImages.Add(new OwnerRatingImage(-1, ImageUrl, SelectedStayedInAccommodation.ReservationId));
            tbImageUrl.Text = "";
        }

        private Image CreateImageFromBitMap()
        {
            if (bAddImage.IsEnabled)
            {
                Image img = new Image();
                img.Source = CreateBitmapImage();
                img.Width = 100;
                img.Height = 100;

                return img;
            }

            return null;
        }

        private BitmapImage CreateBitmapImage()
        {
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            try
            {
                bitmapImage.UriSource = new Uri(@ImageUrl, UriKind.Absolute);
                bitmapImage.EndInit();
                return bitmapImage;
            }
            catch
            {
                MessageBox.Show("Invalid type of image url");
                bAddImage.IsEnabled = false;
                return null;
            }
            
        }

        private void tbImageUrl_TextChanged(object sender, TextChangedEventArgs e)
        {
            bAddImage.IsEnabled = true;
        }

        private void RemoveImage(object sender, RoutedEventArgs e)
        {
            ObservableCollection<Image> AddedImagesCpy = new ObservableCollection<Image>(AddedImages);
            foreach(var item in lvAddedImages.SelectedItems)
            {
                AddedImagesCpy.Remove((Image)item);
            }

            AddedImages = AddedImagesCpy;
            lvAddedImages.ItemsSource = AddedImages;
        }

        private void ViewComment(object sender, RoutedEventArgs e)
        {
            ReservationRequests reservationRequest =_reservationRequestsRepository.GetById(SelectedReservationRequestDTO.RequestId);

            if(reservationRequest.isCanceled == RequestStatus.Postponed)
            {
                MessageBox.Show("Your request has been confirmed");
                return;
            }
            else 
                if(reservationRequest.isCanceled == RequestStatus.Pending)
                {
                    MessageBox.Show("Your request is still pending");
                    return;
                }

            if (reservationRequest.Comment == "")
                MessageBox.Show("Owner didn't leave a comment", "Owner's comment");
            else MessageBox.Show(reservationRequest.Comment, "Owner's comment");
        }
    }
}

using application.UseCases;
using booking.application.usecases;
using booking.Commands;
using booking.Domain.DTO;
using booking.DTO;
using booking.Model;
using booking.View;
using Microsoft.Expression.Interactivity.Media;
using Overview = WPF.Views.Guest1.Overview;
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
using System.Windows.Media.Imaging;
using WPF.Views.Guest1;
using Domain.DTO;

namespace WPF.ViewModels.Guest1
{
    public class RateAccommodationAndOwnerViewModel : INotifyPropertyChanged
    {
        public List<OwnerRatingImage> OwnerRatingImages { get; set; }
        public static ObservableCollection<ReservationAccommodationDTO> StayedInAccommodations { get; set; }
        public static ObservableCollection<Guest1RatingAccommodationDTO> Guest1RatingAccommodationDTOs { get; set; }
        public List<Guest1Rating> Guest1Ratings { get; set; }

        private ObservableCollection<Image> addedImages;
        public ObservableCollection<Image> AddedImages 
        {
            get { return addedImages; }
            set
            {
                if(addedImages != value)
                {
                    addedImages = value;
                    OnPropertyChanged(nameof(AddedImages));
                }
            }
        }
        public static ReservationAccommodationDTO SelectedStayedInAccommodation { get; set; }

        private bool submitEnabled;
        public bool SubmitEnabled
        {
            get { return submitEnabled; }
            set
            {
                if(submitEnabled != value)
                {
                    submitEnabled = value;
                    OnPropertyChanged();
                }
            }
        }

        public Image SelectedAddedImages { get; set; }

        private bool addImageEnabled;
        public bool AddImageEnabled 
        {
            get { return addImageEnabled; }
            set
            {
                if(addImageEnabled != value)
                {
                    addImageEnabled = value;
                    OnPropertyChanged();
                }
            } 
        }

        private bool removeImageEnabled;
        public bool RemoveImageEnabled
        {
            get { return removeImageEnabled; }
            set
            {
                if(removeImageEnabled != value)
                {
                    removeImageEnabled = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool ratingsIGotEnabled;

        public bool RatingsIGotEnabled
        {
            get { return ratingsIGotEnabled; }
            set
            {
                if(ratingsIGotEnabled != value)
                {
                    ratingsIGotEnabled = value;
                    OnPropertyChanged();
                }
            }
        }

        private static object selectedFromList;
        public object SelectedFromList 
        {
            get
            {
                return selectedFromList;
            }
            set
            {
                selectedFromList = value;
                StayedInSelectionChanged(selectedFromList);
            }
        }

        private double cleanRating;
        public double CleanRating 
        {
            get { return cleanRating; }
            set
            {
                if(cleanRating != value)
                {
                    cleanRating = value;
                    OnPropertyChanged(nameof(CleanRating));

                    SubmitEnabled = OwnersKindenssRating > 0 && SelectedFromList != null;
                }
            }
        }

        private double ownersKidnessRating;
        public double OwnersKindenssRating 
        {
            get { return ownersKidnessRating; }
            set
            {
                if (ownersKidnessRating != value)
                {
                    ownersKidnessRating = value;
                    OnPropertyChanged(nameof(OwnersKindenssRating));

                    SubmitEnabled = CleanRating > 0 && SelectedFromList != null;
                }
            } 
        }
        private string ratingComment;
        public string RatingComment 
        { 
            get { return ratingComment; }
            set
            {
                if(ratingComment != value)
                {
                    ratingComment = value;
                    OnPropertyChanged(nameof(RatingComment));
                }
            }
        }

        private string imageUrl;
        public string ImageUrl 
        {
            get { return imageUrl; }
            set
            {
                if(imageUrl != value)
                {
                    imageUrl = value;
                    OnPropertyChanged(nameof(ImageUrl));

                    AddImageEnabled = true;
                }
            } 
        }

        private string renovationDescription;
        public string RenovationDescription
        {
            get { return renovationDescription; }
            set
            {
                if(renovationDescription != value)
                {
                    renovationDescription = value;
                    OnPropertyChanged(nameof(RenovationDescription));
                }
            }
        }

        private string selectedUrgency;
        public string SelectedUrgency
        {
            get { return selectedUrgency; }
            set
            {
                if(selectedUrgency != value)
                {
                    selectedUrgency = value;
                    OnPropertyChanged(nameof(selectedUrgency));
                }
            }
        }

        private readonly OwnerRatingService _ownerRatingService;
        private readonly OwnerRatingImageService _ownerRatingImageService;
        private readonly OwnerNotificationsService _ownerNotificationsService;
        private readonly ReservedDatesService _reservedDatesService;
        private readonly ReservationService _reservationService;
        private readonly AccommodationService _accommodationService;
        private readonly UserService _userService;
        private readonly Guest1RatingsService _guest1RatingsService;
        private readonly LocationService _locationService;

        public ICommand SubmitRateCommand => new RelayCommand(SubmitRate);
        public ICommand AddImageCommand => new RelayCommand(AddImage);
        public ICommand RemoveImageCommand => new RelayCommand(RemoveImage);
        public ICommand SelectedStayedInChangedCommand => new RelayCommand(SelectedStayedInChanged);
        public ICommand SelectedImageChangedCommand => new RelayCommand(SelectedImageChanged);
        public ICommand OpetRecievedRatingsCommand => new RelayCommand(OpetRecievedRatings);

        private Guest1View guest1ViewWindow;
        private int userId;
        public RateAccommodationAndOwnerViewModel(int id, Guest1View guest1View)
        {
            _ownerNotificationsService = new OwnerNotificationsService();
            _ownerRatingService = new OwnerRatingService();
            _ownerRatingImageService = new OwnerRatingImageService();
            _reservedDatesService = new ReservedDatesService();
            _reservationService = new ReservationService();
            _accommodationService = new AccommodationService();
            _userService = new UserService();
            _guest1RatingsService = new Guest1RatingsService();
            _locationService = new LocationService();

            guest1ViewWindow = guest1View;
            userId = id;

            StayedInAccommodations = new ObservableCollection<ReservationAccommodationDTO>(CreateStayedInAccommodations());
            AddedImages = new ObservableCollection<Image>();
            OwnerRatingImages = new List<OwnerRatingImage>();
            Guest1RatingAccommodationDTOs = new ObservableCollection<Guest1RatingAccommodationDTO>();

            SetRatingsIGotEnabled();   
        }

        public void SetRatingsIGotEnabled()
        {
            List<Guest1Rating> guestsRatings = _guest1RatingsService.GetAllByGuest1Id(userId);

            Guest1Ratings = new List<Guest1Rating>();
            Guest1RatingAccommodationDTOs.Clear();
            List<ReservedDates> guest1RatedDates = _reservedDatesService.GetByGuestId(userId).Where(r => r.RatedByGuest).ToList();

            Guest1Rating rating;
            foreach (ReservedDates dates in guest1RatedDates)
            {
                rating = guestsRatings.Find(r => r.ReservationId == dates.Id);
                if (rating != null)
                {
                    Guest1Ratings.Add(rating);

                    Accommodation accommodation = _accommodationService.GetById(dates.AccommodationId);
                    Location location = _locationService.GetById(accommodation.LocationId);
                    Guest1Rating lastAddedRating = Guest1Ratings[Guest1Ratings.Count() - 1];
                    Guest1RatingAccommodationDTO guest1RatingAccommodationDTO = new Guest1RatingAccommodationDTO(dates.Id, lastAddedRating.CleanRating, lastAddedRating.RulesRating, lastAddedRating.Comment,
                        accommodation.Name, location.State + ", " + location.City, dates.StartDate, dates.EndDate);

                    Guest1RatingAccommodationDTOs.Add(guest1RatingAccommodationDTO);
                }
            }

            RatingsIGotEnabled = Guest1RatingAccommodationDTOs.Count > 0 ? true : false;
        }

        public List<ReservationAccommodationDTO> CreateStayedInAccommodations()
        {
            ObservableCollection<ReservationAccommodationDTO> reservationAccommodations = _reservationService.CreateReservationAccommodationDTOs(userId);

            return reservationAccommodations.Where(r => !_reservedDatesService.GetById(r.ReservationId).RatedByGuest &&
                 DateTime.Now >= r.EndDate && (DateTime.Now - r.EndDate).Days <= 5).ToList();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void SubmitRate()
        {
            if (SubmitEnabled)
            {
                OwnerRating ownerRating = CreateOwnerRating();

                _ownerRatingService.Add(ownerRating);

                foreach (var ownerRatingImage in OwnerRatingImages)
                {
                    ownerRatingImage.Id = _ownerRatingImageService.MakeId();
                    _ownerRatingImageService.Add(ownerRatingImage);
                }

                StayedInAccommodations.Remove(StayedInAccommodations.Where(a => a.ReservationId == SelectedStayedInAccommodation.ReservationId).ToList()[0]);

                ResetLists();
                EmptyForm();

                SetRatingsIGotEnabled();
                Inform();

                InitializeAccommodationDTO();
            }
        }

        private void Inform()
        {
            if (Guest1RatingAccommodationDTOs.Count > 0)
            {
                SuccessfullyRatedView popUpWindow = new SuccessfullyRatedView(userId, Guest1RatingAccommodationDTOs);
                popUpWindow.Show();
            }
            else
            {
                MessageBox.Show("You have successfully rated the accommodation and owner. Once the owner do the same,\n" +
                    "you will be able to see rating you've got!");
            }
        }

        private void EmptyForm()
        {
            RatingComment = "";
            CleanRating = 0;
            OwnersKindenssRating = 0;
        }

        private OwnerRating CreateOwnerRating()
        {
            ReservedDates reservedDate = UpdateReservedDatesGuestRatedFlag();
            Accommodation accommodation = _accommodationService.GetById(reservedDate.AccommodationId);
            string selectedUrgencyLevel = string.IsNullOrEmpty(SelectedUrgency) ? "" : SelectedUrgency.ToString().Split(" - ")[0].Split("System.Windows.Controls.ComboBoxItem: ")[1];

            OwnerRating ownerRating = new OwnerRating(_ownerRatingService.MakeId(), accommodation.OwnerId, Convert.ToInt32(CleanRating), 
                Convert.ToInt32(OwnersKindenssRating), RatingComment, RenovationDescription, selectedUrgencyLevel);
            ownerRating.ReservationId = SelectedStayedInAccommodation.ReservationId;

            return ownerRating;
        }

        private ReservedDates UpdateReservedDatesGuestRatedFlag()
        {
            ReservedDates reservedDate = _reservedDatesService.GetById(SelectedStayedInAccommodation.ReservationId);
            reservedDate.RatedByGuest = true;
            _reservedDatesService.Update(reservedDate);
            return reservedDate;
        }

        private void InitializeAccommodationDTO()
        {
            //guest1ViewWindow.signInWindow.RefreshUsers();

            List<OwnerRating> ownerRatings = _ownerRatingService.GetAll();
            List<User> users = _userService.GetAll().ToList();
            double sum, i;
            double AverageRating = 0;
            foreach (User user in users)
            {
                sum = 0;
                i = 0;
                if (user.Role != "Owner") continue;
                foreach (var rating in ownerRatings)
                {
                    if (rating.OwnerId != user.Id) continue;
                    sum += rating.CleanRating + rating.KindRating;
                    i += 1;
                }
                AverageRating = i == 0 ? 0 : sum / (i * 2);
                _userService.UpdateById(user.Id, AverageRating >= 4.5 && i >= 3);
            }

            while(OverviewViewModel.AccommodationDTOs.Count > 0)
            {
                OverviewViewModel.AccommodationDTOs.RemoveAt(0);
            }

            foreach(var item in _accommodationService.SortAccommodationDTOs(_accommodationService.CreateAccomodationDTOs()))
            {
                OverviewViewModel.AccommodationDTOs.Add(item);
            }
        }

        private void ResetLists()
        {
            while(OwnerRatingImages.Count > 0)
            {
                OwnerRatingImages.RemoveAt(0);
            }

            while(AddedImages.Count > 0)
            {
                AddedImages.RemoveAt(0);
            }
        }

        private void AddImage()
        {
            if (AddImageEnabled)
            {
                AddedImages.Add(CreateImageFromBitMap());
                if (!AddImageEnabled)
                {
                    RemoveLastAddedImage();
                    return;
                }

                if (OwnerRatingImages.Find(i => i.Url == ImageUrl) != null)
                {
                    ImageUrl = "";
                    RemoveLastAddedImage();
                    MessageBox.Show("You have already added that image, please choose a different one!", "Warning");
                    return;
                }

                OwnerRatingImages.Add(new OwnerRatingImage(-1, ImageUrl, SelectedStayedInAccommodation.ReservationId));

                ImageUrl = "";
            }
            else
            {
                MessageBox.Show("You have to select an accommodation you have stayed in before adding an image", "Warning");
            }
        }

        private void RemoveLastAddedImage()
        {
            AddedImages.RemoveAt(AddedImages.Count - 1);
        }

        private Image CreateImageFromBitMap()
        {
            if (AddImageEnabled)
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
                AddImageEnabled = false;
                return null;
            }
        }

        private void RemoveImage()
        {
            if(SelectedAddedImages != null && RemoveImageEnabled)
            {
                
                OwnerRatingImages.RemoveAt(guest1ViewWindow.lvAddedImages.SelectedIndex);
                AddedImages.Remove((Image)guest1ViewWindow.lvAddedImages.SelectedItem);
            }
            else
            {
                MessageBox.Show("You have to select an image you want to remove!");
            }
        }

        private void OpetRecievedRatings()
        {
            ReviewView reviewViewModel = new ReviewView(userId, Guest1RatingAccommodationDTOs);
            reviewViewModel.Show();
        }

        private void StayedInSelectionChanged(object selectedFromList)
        {
            AddImageEnabled = true;
            if (selectedFromList != null)
            {
                string[] parts = selectedFromList.ToString().Split("|");

                ReservationAccommodationDTO stayedInAccommodation = StayedInAccommodations.Where(a => a.StartDate.ToString("dd/MM/yyyy") == parts[2].Split("-")[0] && a.EndDate.ToString("dd/MM/yyyy") == parts[2].Split("-")[1]
                    && a.Location == parts[1] && a.AccommodationName == parts[0]).ToList()[0];

                SelectedStayedInAccommodation = new ReservationAccommodationDTO(stayedInAccommodation);
            }
        }

        private void SelectedStayedInChanged()
        {
            SubmitEnabled = ((selectedFromList != null) ? true : false) && CleanRating > 0 && OwnersKindenssRating > 0;
            AddImageEnabled = (selectedFromList != null) ? true : false;
        }

        private void SelectedImageChanged()
        {
            RemoveImageEnabled = (SelectedAddedImages != null) ? true : false;
        }
    }
}

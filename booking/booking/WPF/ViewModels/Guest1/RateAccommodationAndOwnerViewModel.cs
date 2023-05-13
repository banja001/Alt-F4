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

namespace WPF.ViewModels.Guest1
{
    public class RateAccommodationAndOwnerViewModel : INotifyPropertyChanged
    {
        public List<OwnerRatingImage> OwnerRatingImages { get; set; }
        public static ObservableCollection<ReservationAccommodationDTO> StayedInAccommodations { get; set; }
        public static ObservableCollection<Image> AddedImages { get; set; }
        public static ReservationAccommodationDTO SelectedStayedInAccommodation { get; set; }

        public Image SelectedAddedImages { get; set; }

        public static bool AddImageEnabled { get; set; }

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
                    OnPropertyChanged();
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
                    OnPropertyChanged();
                }
            } 
        }
        public string RatingComment { get; set; }
        public string ImageUrl { get; set; }

        private readonly OwnerRatingService _ownerRatingService;
        private readonly OwnerRatingImageService _ownerRatingImageService;
        private readonly OwnerNotificationsService _ownerNotificationsService;
        private readonly ReservedDatesService _reservedDatesService;
        private readonly ReservationService _reservationService;
        private readonly AccommodationService _accommodationService;
        private readonly UserService _userService;

        public ICommand SubmitRateCommand => new RelayCommand(SubmitRate);
        public ICommand AddImageCommand => new RelayCommand(AddImage);
        public ICommand RemoveImageCommand => new RelayCommand(RemoveImage);

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

            guest1ViewWindow = guest1View;
            userId = id;
            AddImageEnabled = true;

            StayedInAccommodations = new ObservableCollection<ReservationAccommodationDTO>(CreateStayedInAccommodations());
            AddedImages = new ObservableCollection<Image>();
            OwnerRatingImages = new List<OwnerRatingImage>();
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
            OwnerRating ownerRating = CreateOwnerRating();

            _ownerRatingService.Add(ownerRating);

            foreach (var ownerRatingImage in OwnerRatingImages)
            {
                ownerRatingImage.Id = _ownerRatingImageService.MakeId();
                _ownerRatingImageService.Add(ownerRatingImage);
            }

            StayedInAccommodations.Remove(StayedInAccommodations.Where(a => a.ReservationId == SelectedStayedInAccommodation.ReservationId).ToList()[0]);

            ResetLists();
            MessageBox.Show("Rating successfully added!");

            InitializeAccommodationDTO();
        }

        private OwnerRating CreateOwnerRating()
        {
            ReservedDates reservedDate = UpdateReservedDatesGuestRatedFlag();
            Accommodation accommodation = _accommodationService.GetById(reservedDate.AccommodationId);

            OwnerRating ownerRating = new OwnerRating(_ownerRatingService.MakeId(), accommodation.OwnerId, Convert.ToInt32(CleanRating), Convert.ToInt32(OwnersKindenssRating), RatingComment);
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
            /*Guest1View.AccommodationDTOs = guest1ViewWindow.CreateAccomodationDTOs(_accommodationService.GetAll());
            Guest1View.AccommodationDTOs = guest1ViewWindow.SortAccommodationDTOs();
            guest1ViewWindow.accommodationData.ItemsSource = Guest1View.AccommodationDTOs;*/
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
            AddedImages.Add(CreateImageFromBitMap());
            if (!AddImageEnabled)
            {
                RemoveLastAddedImage();
                return;
            }

            if (OwnerRatingImages.Find(i => i.Url == ImageUrl) != null)
            {
                MessageBox.Show("You have already added that image, please choose a different one!", "Warning");
                RemoveLastAddedImage();
                return;
            } 

            OwnerRatingImages.Add(new OwnerRatingImage(-1, ImageUrl, SelectedStayedInAccommodation.ReservationId));
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
            if(SelectedAddedImages != null)
            {
                
                OwnerRatingImages.RemoveAt(guest1ViewWindow.lvAddedImages.SelectedIndex);
                AddedImages.Remove((Image)guest1ViewWindow.lvAddedImages.SelectedItem);
            }
            else
            {
                MessageBox.Show("You have to select an image you want to remove!");
            }
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
    }
}

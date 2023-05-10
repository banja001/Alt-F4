using application.UseCases;
using booking.application.usecases;
using booking.Commands;
using booking.Domain.DTO;
using booking.Model;
using booking.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace WPF.ViewModels.Guest1
{
    public class RateAccommodationAndOwnerViewModel
    {
        public List<OwnerRatingImage> OwnerRatingImages { get; set; }
        public static ObservableCollection<ReservationAccommodationDTO> StayedInAccommodations { get; set; }
        public static ObservableCollection<Image> AddedImages { get; set; }
        public static ReservationAccommodationDTO SelectedStayedInAccommodation { get; set; }

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
        public static double CleanRating { get; set; }
        public static double OwnersKindenssRating { get; set; }
        public string RatingComment { get; set; }
        public string ImageUrl { get; set; }

        private readonly OwnerRatingService _ownerRatingService;
        private readonly OwnerRatingImageService _ownerRatingImageService;
        private readonly OwnerNotificationsService _ownerNotificationsService;
        private readonly ReservedDatesService _reservedDatesService;
        private readonly ReservationService _reservationService;
        private readonly AccommodationService _accommodationService;

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

            guest1ViewWindow = guest1View;
            userId = id;

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
            guest1ViewWindow.signInWindow.RefreshUsers();

            /*Guest1View.AccommodationDTOs = guest1ViewWindow.CreateAccomodationDTOs(_accommodationService.GetAll());
            Guest1View.AccommodationDTOs = guest1ViewWindow.SortAccommodationDTOs();
            guest1ViewWindow.accommodationData.ItemsSource = Guest1View.AccommodationDTOs;*/
        }

        private void ResetLists()
        {
            OwnerRatingImages.Clear();
            AddedImages.Clear();

            guest1ViewWindow.lvAddedImages.ItemsSource = AddedImages;
            guest1ViewWindow.lbStayedIn.ItemsSource = StayedInAccommodations;
            //guest1ViewWindow.ResetFormInputs();
        }

        private void AddImage()
        {
            AddedImages.Add(CreateImageFromBitMap());
            if (!guest1ViewWindow.bAddImage.IsEnabled)
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
            guest1ViewWindow.ClearImgUrlTextBox();
        }

        private void RemoveLastAddedImage()
        {
            AddedImages.RemoveAt(AddedImages.Count - 1);
            guest1ViewWindow.ClearImgUrlTextBox();
        }

        private Image CreateImageFromBitMap()
        {
            if (guest1ViewWindow.bAddImage.IsEnabled)
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
                guest1ViewWindow.bAddImage.IsEnabled = false;
                return null;
            }
        }

        private void RemoveImage()
        {
            OwnerRatingImages.RemoveAt(guest1ViewWindow.lvAddedImages.SelectedIndex);
            AddedImages.Remove((Image)guest1ViewWindow.lvAddedImages.SelectedItem);
           
            guest1ViewWindow.lvAddedImages.ItemsSource = AddedImages;
        }

        private void StayedInSelectionChanged(object selectedFromList)
        {
            guest1ViewWindow.bSubmitRate.IsEnabled = true;
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

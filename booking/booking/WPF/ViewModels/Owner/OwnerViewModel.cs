using application.UseCases;
using booking.Commands;
using booking.Domain.Model;
using booking.DTO;
using booking.Model;
using booking.Repository;
using booking.View;
using booking.View.Owner;
using booking.WPF.ViewModels;
using booking.WPF.Views.Owner;
using Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace WPF.ViewModels.Owner
{
    
    public class OwnerViewModel:BaseViewModel
    {
        public int OwnerId { get; set; }
        public int AverageRating { get; set; }

        public AccommodationService accommodationService;
        public List<Accommodation> accommodations;
        public AccommodationImageService accommodationImageService;
        public List<AccommodationImage> accommodationImages;
        public LocationService locationService;
        public List<Location> locations;
        public ReservedDatesRepository reservedDatesRepository;
        public List<ReservedDates> reservedDates;
        public Guest1RatingsRepository guest1RatingsRepository;
        public List<Guest1Rating> guest1Ratings;
        public UserService userService;
        public List<User> users;
        public OwnerRatingImageRepository OwnerRatingImageRepository;
        public List<OwnerRatingImage> OwnerRatingImages;
        public OwnerRatingRepository OwnerRatingRepository;
        public List<OwnerRating> OwnerRatings;
        public ObservableCollection<Guest1RatingDTO> ListToRate { get; set; }
        public Guest1RatingDTO SelectedItem { get; set; }

        private readonly OwnerNotificationRepository _ownerNotificationRepository;
        public static List<OwnerNotification> Notifications { get; set; }
        public OwnerWindow ownerWindow;
        
        public ICommand AddAccommodationCommand => new RelayCommand(AddAccommodation);
        public ICommand RateGuestsCommand => new RelayCommand(RateGuests_Click);
        public ICommand ReservationChangeCommand => new RelayCommand(Button_Click);
        public ICommand ViewRatingsCommand => new RelayCommand(View_Ratings_Click);
        public OwnerViewModel(int id,OwnerWindow ow)
        {
            OwnerId = id;
            ownerWindow = ow;
            CreateInstances();

            List<ReservedDates> ratingDates = PickDatesForRating();

            List<Guest1RatingDTO> tempList = GetGuestsToRate(ratingDates);
            ListToRate = new ObservableCollection<Guest1RatingDTO>(tempList);

            CalculateAverageRating();



            if (tempList.Count() != 0)
            {
                ownerWindow.Loaded += NotifyUser;
            }

            _ownerNotificationRepository = new OwnerNotificationRepository();
            Notifications = _ownerNotificationRepository.GetAll();

            if (Notifications.Count != 0)
            {
                NotifyOwner();
            }
        }

        public void NotifyOwner()
        {
            foreach (var notification in Notifications)
            {
                if (notification.OwnerId == OwnerId)
                    MessageBox.Show(notification.ToString());
            }

            _ownerNotificationRepository.DeleteAllByOwnerId(OwnerId);
        }

        private void CalculateAverageRating()
        {
            int sum = 0;
            int i = 0;


            foreach (var rating in OwnerRatings)
            {
                if (rating.OwnerId == OwnerId)
                {

                    sum += rating.KindRating + rating.CleanRating;
                    i += 1;
                }

            }
            if (i == 0) AverageRating = 0;
            else AverageRating = sum / (i * 2);


            ownerWindow.AverageLabel.Content = AverageRating;
            if (AverageRating >= 4.5 && i >= 3)
            {
                ownerWindow.SuperOwnerLabel.Content = "Super Owner**";
                userService.UpdateById(OwnerId, true);
            }
        }

        private List<Guest1RatingDTO> GetGuestsToRate(List<ReservedDates> ratingDates)
        {
            List<Guest1RatingDTO> tempList = new List<Guest1RatingDTO>();
            foreach (ReservedDates date in ratingDates)
            {
                Guest1RatingDTO guestsToRate = new Guest1RatingDTO();
                guestsToRate.DateId = date.Id;
                guestsToRate.StartDate = date.StartDate;
                guestsToRate.EndDate = date.EndDate;
                guestsToRate.GuestName = users.Find(u => u.Id == date.UserId).Username;
                guestsToRate.AccommodationName = accommodations.Find(u => u.Id == date.AccommodationId).Name;
                tempList.Add(guestsToRate);
            }
            return tempList;
        }
        private void CreateInstances()
        {
            userService = new UserService();
            users = userService.GetAll();

            accommodationService = new AccommodationService();
            accommodations = accommodationService.GetAll();
            accommodationImageService = new AccommodationImageService();
            accommodationImages = accommodationImageService.GetAll();
            locationService = new LocationService();
            locations = locationService.GetAll();

            reservedDatesRepository = new ReservedDatesRepository();
            reservedDates = reservedDatesRepository.GetAll();
            guest1RatingsRepository = new Guest1RatingsRepository();
            guest1Ratings = guest1RatingsRepository.GetAll();
            OwnerRatingImageRepository = new OwnerRatingImageRepository();
            OwnerRatingImages = OwnerRatingImageRepository.GetAll();
            OwnerRatingRepository = new OwnerRatingRepository();
            OwnerRatings = OwnerRatingRepository.GetAll();
        }

        private void NotifyUser(object sender, RoutedEventArgs e)
        {

            ownerWindow.Loaded -= NotifyUser;
            MessageBox.Show("You have unrated guests", "Message");
        }

        private void AddAccommodation()
        {
            AddAccommodationWindow win = new AddAccommodationWindow(this);
            win.ShowDialog();
        }

        public List<ReservedDates> PickDatesForRating()
        {
            List<ReservedDates> ratingDates = new List<ReservedDates>();
            foreach (ReservedDates reservedDate in reservedDates)
            {
                accommodations.Find(m => m.Id == reservedDate.AccommodationId);
                if (DateTime.Today >= reservedDate.EndDate && DateTime.Today < reservedDate.EndDate.AddDays(5) && reservedDate.RatedByOwner == false
                    && accommodations.Find(m => m.Id == reservedDate.AccommodationId).OwnerId == OwnerId)
                {
                    ratingDates.Add(reservedDate);
                }

            }
            return ratingDates;
        }

        private void RateGuests_Click()
        {
            if (SelectedItem == null)
            {
                MessageBox.Show("No selected items", "Error");
            }
            else
            {

                RateGuestWindow win = new RateGuestWindow(this);
                win.ShowDialog();

            }

        }

        private void View_Ratings_Click()
        {
            RatingViewWindow win = new RatingViewWindow(this);
            win.ShowDialog();
        }

        private void Button_Click()
        {
            ReservationChangeWindow win = new ReservationChangeWindow(this);
            win.ShowDialog();
        }
    }
}

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
using Domain.Model;
using Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using WPF.Views.Owner;

namespace WPF.ViewModels.Owner
{
    
    public class OwnerViewModel:BaseViewModel
    {
        private string averageLabel;
        public string AverageLabel {
            get
            {
                return averageLabel;
            }
            set
            {
                if (value != averageLabel)
                {
                    averageLabel = value;
                    OnPropertyChanged("AverageLabel");
                }
            }
        }
        private string superOwnerLabel;
        public string SuperOwnerLabel
        {
            get
            {
                return superOwnerLabel;
            }
            set
            {
                if (value != superOwnerLabel)
                {
                    superOwnerLabel = value;
                    OnPropertyChanged("AverageLabel");
                }
            }
        }
        public int OwnerId { get; set; }
        public double AverageRating { get; set; }
        public string UserName { get; set; }

        public AccommodationService accommodationService;
        public List<Accommodation> accommodations;
        public AccommodationImageService accommodationImageService;
        public List<AccommodationImage> accommodationImages;
        public LocationService locationService;
        public List<Location> locations;
        public ReservedDatesService reservedDatesService;
        public List<ReservedDates> reservedDates;
        public Guest1RatingsService guest1RatingsService;
        public List<Guest1Rating> guest1Ratings;
        public UserService userService;
        public List<User> users;
        public OwnerRatingImageService OwnerRatingImageService;
        public List<OwnerRatingImage> OwnerRatingImages;
        public OwnerRatingService OwnerRatingService;
        public List<OwnerRating> OwnerRatings;
        public ObservableCollection<Guest1RatingDTO> ListToRate { get; set; }
        public Guest1RatingDTO SelectedItem { get; set; }

        private readonly OwnerNotificationsService _ownerNotificationService;
        public static List<OwnerNotification> Notifications { get; set; }
        public ICommand NotifyUserCommand => new RelayCommand(NotifyUser);

        public bool load;
        public ICommand RateGuestsCommand => new RelayCommand(RateGuests_Click);
        public OwnerViewModel(int id)
        {
            OwnerId = id;
       
            CreateInstances();
            UserName = userService.GetUserNameById(id);
            List<ReservedDates> ratingDates = PickDatesForRating();

            List<Guest1RatingDTO> tempList = GetGuestsToRate(ratingDates);
            ListToRate = new ObservableCollection<Guest1RatingDTO>(tempList);

            CalculateAverageRating();
            load = false;
            if (tempList.Count() != 0)
            {
                load = true;
            }

            _ownerNotificationService = new OwnerNotificationsService();
            Notifications = _ownerNotificationService.GetAll();

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
            _ownerNotificationService.DeleteAllByOwnerId(OwnerId);
        }

        private void CalculateAverageRating()
        {
            double sum = 0;
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
            else AverageRating =sum / (i * 2);
            AverageLabel = AverageRating.ToString();
            if (AverageRating >= 4.5 && i >= 3)
            {
                SuperOwnerLabel = "Super Owner**";
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
                guestsToRate.StartDate = date.StartDate.ToShortDateString();
                guestsToRate.EndDate = date.EndDate.ToShortDateString();
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

            reservedDatesService = new ReservedDatesService();
            reservedDates = reservedDatesService.GetAll();
            guest1RatingsService = new Guest1RatingsService();
            guest1Ratings = guest1RatingsService.GetAll();
            OwnerRatingImageService = new OwnerRatingImageService();
            OwnerRatingImages = OwnerRatingImageService.GetAll();
            OwnerRatingService = new OwnerRatingService();
            OwnerRatings = OwnerRatingService.GetAll();
        }

        private void NotifyUser()
        {
            if (GlobalVariables.a == true && load==true)
            {
                MessageBox.Show("You have unrated guests", "Message");
                GlobalVariables.a = false;
            }
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
                RateGuestWindow win = new RateGuestWindow(this,SelectedItem);
                MainWindow.w.Main.Content = win;
            }

        }
    }
}

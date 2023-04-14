using booking.Repository;
﻿using booking.DTO;
using booking.Model;
using booking.Repository;
using booking.View.Owner;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics.Contracts;
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
using booking.WPF.Views.Owner;

namespace booking.View
{
    /// <summary>
    /// Interaction logic for OwnerWindow.xaml
    /// </summary>
    public partial class OwnerWindow : Window
    {
        public int OwnerId { get; set; }
        public int AverageRating { get; set; }


        public AccommodationRepository accommodationRepository;
        public List<Accommodation> accommodations;
        public AccommodationImageRepository accommodationImageRepository;
        public List<AccommodationImage> accommodationImages;
        public LocationRepository locationRepository;
        public List <Location> locations;
        public ReservedDatesRepository reservedDatesRepository;
        public List<ReservedDates> reservedDates;
        public Guest1RatingsRepository guest1RatingsRepository;
        public List<Guest1Rating> guest1Ratings;
        public UserRepository userRepository;
        public List<User> users;
        public OwnerRatingImageRepository OwnerRatingImageRepository;
        public List<OwnerRatingImage> OwnerRatingImages;
        public OwnerRatingRepository OwnerRatingRepository;
        public List<OwnerRating> OwnerRatings;
        public ObservableCollection<Guest1RatingDTO> ListToRate { get; set; }
        public Guest1RatingDTO SelectedItem { get; set; }
        public OwnerWindow(int id)
        {
            InitializeComponent();
            DataContext = this;
            OwnerId = id;
            CreateInstances();
            
            List<ReservedDates> ratingDates = PickDatesForRating();
            List<Guest1RatingDTO> tempList = GetGuestsToRate(ratingDates);
            ListToRate = new ObservableCollection<Guest1RatingDTO>(tempList);
            int sum = 0;
            int i = 0;
            if (OwnerRatings.Count == 0)
            {
                AverageRating = 0;
            }
            else
            {
                foreach(var rating in OwnerRatings)
                {
                    if (rating.OwnerId == OwnerId)
                    {
                        
                        sum += rating.KindRating + rating.CleanRating;
                        i = i + 2;
                    }
                    
                }
                
                AverageRating = sum / i;
            }
            AverageLabel.Content = AverageRating;


            if (tempList.Count() != 0)
            {
                Loaded += NotifyUser;
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
            userRepository = new UserRepository();
            users = userRepository.GetAll();
            accommodationRepository = new AccommodationRepository();
            accommodations = accommodationRepository.GetAll();
            accommodationImageRepository = new AccommodationImageRepository();
            accommodationImages = accommodationImageRepository.GetAll();
            locationRepository = new LocationRepository();
            locations = locationRepository.GetAll();
            reservedDatesRepository = new ReservedDatesRepository();
            reservedDates = reservedDatesRepository.GetAll();
            guest1RatingsRepository = new Guest1RatingsRepository();
            guest1Ratings = guest1RatingsRepository.GetAll();
            OwnerRatingImageRepository=new OwnerRatingImageRepository();
            OwnerRatingImages=OwnerRatingImageRepository.GetAll();
            OwnerRatingRepository=new OwnerRatingRepository();
            OwnerRatings=OwnerRatingRepository.GetAll();

        }

        private void NotifyUser(object sender, RoutedEventArgs e)
        {
            
            Loaded -= NotifyUser;
            MessageBox.Show("You have unrated guests", "Message");
        }

        private void AddAccommodation(object sender, RoutedEventArgs e)
        {
            AddAccommodationWindow win=new AddAccommodationWindow(this);
            win.ShowDialog();
        }

        public List<ReservedDates> PickDatesForRating()
        { 
            List<ReservedDates> ratingDates = new List<ReservedDates>();
            foreach(ReservedDates reservedDate in reservedDates)
            {
                accommodations.Find(m => m.Id == reservedDate.AccommodationId);
                if (DateTime.Today >= reservedDate.EndDate && DateTime.Today < reservedDate.EndDate.AddDays(5) && reservedDate.Rated==-1
                    && accommodations.Find(m => m.Id == reservedDate.AccommodationId).OwnerId==OwnerId)
                {
                    ratingDates.Add(reservedDate);
                }

            }

            return ratingDates;
        }

        private void RateGuests_Click(object sender, RoutedEventArgs e)
        {
            
            
            if (SelectedItem == null)
            {
                MessageBox.Show("No selected items","Error");
            }
            else
            {
                
                RateGuestWindow win = new RateGuestWindow(this);
                win.ShowDialog();
                
            }
           
        }

        private void View_Ratings_Click(object sender, RoutedEventArgs e)
        {
            RatingView win = new RatingView(this);
            win.ShowDialog();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ReservationChange win = new ReservationChange(this);
            win.ShowDialog();
        }
    }
}

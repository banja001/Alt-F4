using booking.DTO;
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

namespace booking.View
{
    /// <summary>
    /// Interaction logic for OwnerWindow.xaml
    /// </summary>
    public partial class OwnerWindow : Window
    {
        public int OwnerId { get; set; }

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
        public ObservableCollection<Guest1RatingDTO> ListToRate { get; set; }
        public Guest1RatingDTO SelectedItem { get; set; }
        public OwnerWindow(int id)
        {
            InitializeComponent();
            DataContext = this;

            OwnerId = id;


            userRepository = new UserRepository();
            users = userRepository.FindAll();
            accommodationRepository = new AccommodationRepository();
            accommodations=accommodationRepository.FindAll();
            accommodationImageRepository = new AccommodationImageRepository();
            accommodationImages = accommodationImageRepository.FindAll();
            locationRepository = new LocationRepository();
            locations = locationRepository.FindAll();
            reservedDatesRepository = new ReservedDatesRepository();
            reservedDates = reservedDatesRepository.FindAll();
            guest1RatingsRepository = new Guest1RatingsRepository();
            guest1Ratings = guest1RatingsRepository.FindAll();
            

            List<ReservedDates> ratingDates=PickDatesForRating();
            List<Guest1RatingDTO> tempList = new List<Guest1RatingDTO>();
            
            


            foreach(ReservedDates date in ratingDates)
            {
                Guest1RatingDTO guestsToRate = new Guest1RatingDTO();
                guestsToRate.DateId = date.Id;
                guestsToRate.StartDate = date.StartDate;
                guestsToRate.EndDate = date.EndDate;
                guestsToRate.GuestName = users.Find(u => u.Id == date.UserId).Username;
                guestsToRate.AccommodationName = accommodations.Find(u => u.Id == date.AccommodationId).Name;
                tempList.Add(guestsToRate);
            }
            ListToRate = new ObservableCollection<Guest1RatingDTO>(tempList);


            
        }

        private void AddAccommodation(object sender, RoutedEventArgs e)
        {
            AddAccommodationWindow win=new AddAccommodationWindow(this);
            win.ShowDialog();
        }

        public List<ReservedDates> PickDatesForRating()//picks dates and guests that should display in datagrid for owner to rate
        { 
            List<ReservedDates> ratingDates = new List<ReservedDates>();
            foreach(ReservedDates reservedDate in reservedDates)
            {
                accommodations.Find(m => m.Id == reservedDate.AccommodationId);
                if (DateOnly.FromDateTime(DateTime.Today) >= reservedDate.EndDate && DateOnly.FromDateTime(DateTime.Today) < reservedDate.EndDate.AddDays(5) && reservedDate.Rated==-1
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
    }
}

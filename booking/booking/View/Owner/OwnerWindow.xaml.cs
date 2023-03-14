using booking.DTO;
using booking.Model;
using booking.Repository;
using booking.View.Owner;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
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
        private AccommodationRepository accommodationRepository;
        private AccommodationImageRepository accommodationImageRepository;
        private LocationRepository locationRepository;
        private ReservedDatesRepository reservedDatesRepository;
        private Guest1RatingsRepository guest1RatingsRepository;
        private UserRepository userRepository;
        public ObservableCollection<Guest1RatingDTO> ListToRate { get; set; }
        public Guest1RatingDTO SelectedItem { get; set; }
        public OwnerWindow()
        {
            InitializeComponent();
            DataContext = this;
            userRepository = new UserRepository();
            accommodationRepository = new AccommodationRepository();
            accommodationImageRepository = new AccommodationImageRepository();
            locationRepository = new LocationRepository();
            reservedDatesRepository = new ReservedDatesRepository();
            guest1RatingsRepository = new Guest1RatingsRepository();


            List<ReservedDates> ratingDates=PickDatesForRating();
            List<Guest1RatingDTO> tempList = new List<Guest1RatingDTO>();
            List<User> users = userRepository.FindAll();
            List<Accommodation> accommodations=accommodationRepository.FindAll();


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
            AddAccommodationWindow win=new AddAccommodationWindow(accommodationRepository,locationRepository,accommodationImageRepository);
            win.Show();
        }

        public List<ReservedDates> PickDatesForRating()//picks dates and guests that should display in datagrid for owner to rate
        {
            List<ReservedDates> reservedDates = reservedDatesRepository.FindAll();
            
            List<ReservedDates> ratingDates = new List<ReservedDates>();
            foreach(ReservedDates reservedDate in reservedDates)
            {
                if (DateOnly.FromDateTime(DateTime.Today) >= reservedDate.EndDate && DateOnly.FromDateTime(DateTime.Today) < reservedDate.EndDate.AddDays(5) && reservedDate.Rated==-1)
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
                
                RateGuestWindow win = new RateGuestWindow(guest1RatingsRepository,userRepository,reservedDatesRepository,SelectedItem,ListToRate);
                win.ShowDialog();
                
            }
           
        }
    }
}

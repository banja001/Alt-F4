using booking.DTO;
using booking.Model;
using booking.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace booking.View.Guest2
{
    /// <summary>
    /// Interaction logic for BookTourOverview.xaml
    /// </summary>

    public partial class BookTourOverview : Window
    {
        private readonly ReservationTourRepository _reservationTourRepository;
        public int NumberOfGuests { get; set; }
        public int AvailableSpace { get; set; }
        public int AverageGuestAge { get; set; }
        public TourLocationDTO TourForBooking { get; set; }
        public User CurrentUser { get; set; }
        private bool ConfirmButtonFlag { get; set; }
        
        public BookTourOverview(Guest2Overview guest2Overview, User user)
        {
            InitializeComponent();
            this.DataContext = this;
            this.TourForBooking = guest2Overview.SelectedTour;
            _reservationTourRepository = new ReservationTourRepository();
            CurrentUser = user;
            NumberOfGuests = 0;
            int takenSpace = _reservationTourRepository.GetNumberOfGuestsForTourId(TourForBooking.Id);
            AvailableSpace = TourForBooking.MaxGuests - takenSpace - NumberOfGuests;
            ConfirmButtonFlag = false;
            this.ConfirmBookingButton.IsEnabled = false;
            ConfirmButtonFlag = false;
        }

        private void CancelBookingButtonClick(object sender, RoutedEventArgs e)
        {  
            this.Close();
        }

        private void ConfirmBookingButtonClick(object sender, RoutedEventArgs e)
        {
            Guest2Overview parentWindow = new Guest2Overview(CurrentUser);
            if (CheckAvailability())
            {
                ReservationTour reservation = new ReservationTour(_reservationTourRepository.GetNextIndex(),
                                                                  TourForBooking.Id,
                                                                  CurrentUser.Id,
                                                                  NumberOfGuests,
                                                                  AverageGuestAge);
                _reservationTourRepository.Add(reservation);
                MessageBox.Show("Tour reserved successfully!", "Success");

                parentWindow.Show();
                ConfirmButtonFlag=true;
                this.Close();
            }
            else
            {
                var result = MessageBox.Show("There's currently not enough space to reserve the selected tour," +
                                             "do you want to see other options on same location?", "Error message", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    parentWindow.FilterByPeopleCount(NumberOfGuests);
                    parentWindow.FilterByLocation(TourForBooking.Location);
                    parentWindow.tourSelectionTable.ItemsSource = parentWindow.TourLocationDTOs;
                    parentWindow.Show();
                    this.Close();
                }
            }
        }
        private bool CheckAvailability()
        {
            int currentGuestNumber = _reservationTourRepository.GetNumberOfGuestsForTourId(TourForBooking.Id);
            bool isAvailable = currentGuestNumber + NumberOfGuests > TourForBooking.MaxGuests ? false : true;
            return isAvailable;
        }

        private void GuestNumberInputTextChanged(object sender, TextChangedEventArgs e)
        {
            Regex numberOfGuestsRegex = new Regex("^[1-9][0-9]*$");
            var GuestNumberInput = sender as TextBox;
            bool isInvalid = string.IsNullOrEmpty(GuestNumberInput.Text) || GuestNumberInput.Text.Equals("0") || !numberOfGuestsRegex.IsMatch(GuestNumberInput.Text);
            if (isInvalid)
            {
                this.ConfirmBookingButton.IsEnabled = false;
            }
            else
            {
                this.ConfirmBookingButton.IsEnabled = true;
            }
        }
        private void AverageGuestAgeInputTextChanged(object sender, TextChangedEventArgs e)
        {
            Regex averageGuestAgeRegex = new Regex("^[1-9][0-9]*$");
            var AverageGuestAgeInput = sender as TextBox;
            bool isInvalid = string.IsNullOrEmpty(AverageGuestAgeInput.Text) || AverageGuestAgeInput.Text.Equals("0") || !averageGuestAgeRegex.IsMatch(AverageGuestAgeInput.Text);
            if (isInvalid)
            {
                this.ConfirmBookingButton.IsEnabled = false;
            }
            else
            {
                this.ConfirmBookingButton.IsEnabled = true;
            }
        }

        private void BookTourOverviewClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!ConfirmButtonFlag)
            {
                Guest2Overview parentWindow = new Guest2Overview(CurrentUser);
                parentWindow.Show();
            }
        }
    }
}

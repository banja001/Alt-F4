using booking.DTO;
using booking.Model;
using booking.Repository;
using System;
using System.Collections.Generic;
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
        public TourLocationDTO TourForBooking { get; set; }
        public User CurrentUser { get; set; }

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

            this.ConfirmBookingButton.IsEnabled = false;
        }

        private void CancelBookingButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ConfirmBookingButton_Click(object sender, RoutedEventArgs e)
        {
            if (CheckAvailability())
            {
                ReservationTour reservation = new ReservationTour(_reservationTourRepository.GetNextIndex(),
                                                                  TourForBooking.Id,
                                                                  CurrentUser.Id,
                                                                  NumberOfGuests);
                _reservationTourRepository.Add(reservation);
                MessageBox.Show("Rezervisali ste turu uspesno!");
                this.Close();
            }
            else
            {
                MessageBox.Show("Na turi trenutno nema dovoljno mesta za sve goste!");
            }
        }
        private bool CheckAvailability()
        {
            int currentGuestNumber = _reservationTourRepository.GetNumberOfGuestsForTourId(TourForBooking.Id);
            bool isAvailable = currentGuestNumber + NumberOfGuests > TourForBooking.MaxGuests ? false : true;
            return isAvailable;
        }

        private void GuestNumberInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            var GuestNumberInput = sender as TextBox;
            bool isInvalid = string.IsNullOrEmpty(GuestNumberInput.Text) || GuestNumberInput.Text.Equals("0");
            if (isInvalid)
            {
                this.ConfirmBookingButton.IsEnabled = false;
            }
            else
            {
                this.ConfirmBookingButton.IsEnabled = true;
            }
        }
    }
}

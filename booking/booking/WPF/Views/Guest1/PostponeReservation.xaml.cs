using booking.Domain.Model;
using booking.Model;
using booking.Repositories;
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

namespace booking.WPF.Views.Guest1
{
    /// <summary>
    /// Interaction logic for PostponeReservation.xaml
    /// </summary>
    public partial class PostponeReservation : Window
    {
        public ReservedDates NewDate { get; set; }

        private readonly ReservationRequestsRepository _reservationRequestsRepository;
        public PostponeReservation(ReservedDates reservation)
        {
            InitializeComponent();

            DataContext = this;

            NewDate = new ReservedDates(reservation);
            SetCalendarDates();

            _reservationRequestsRepository = new ReservationRequestsRepository();
        }

        private void SetCalendarDates()
        {
            cNewStartDate.DisplayDate = NewDate.StartDate;
            cNewEndDate.DisplayDate = NewDate.EndDate;
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Send(object sender, RoutedEventArgs e)
        {
            int requestId = _reservationRequestsRepository.MakeId();
            _reservationRequestsRepository.Add(new ReservationRequests(requestId, NewDate, "Postpone"));


            MessageBox.Show("Your request has been sent successfully!");


            this.Close();
        }
    }
}

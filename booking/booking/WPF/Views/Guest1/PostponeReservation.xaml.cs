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
using WPF.ViewModels.Guest1;

namespace booking.WPF.Views.Guest1
{
    /// <summary>
    /// Interaction logic for PostponeReservation.xaml
    /// </summary>
    public partial class PostponeReservation : Window
    {
        private PostponeReservationViewModel _viewModel;
        public PostponeReservation(ReservedDates reservation)
        {
            InitializeComponent();
            _viewModel = new PostponeReservationViewModel(reservation);
            DataContext = _viewModel;

            SetCalendarDates(reservation);
        }

        private void SetCalendarDates(ReservedDates reservation)
        {
            cNewStartDate.DisplayDate = reservation.StartDate;
            cNewEndDate.DisplayDate = reservation.EndDate;
        }
    }
}

using booking.Domain.Model;
using booking.View;
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

namespace booking.WPF.Views.Owner
{
    /// <summary>
    /// Interaction logic for LeaveComment.xaml
    /// </summary>
    public partial class LeaveCommentWindow : Window
    {
        public string Comment { get; set; }

        public ReservationChangeWindow resWin;

        private ReservationRequests reservationRequest;
        public LeaveCommentWindow(ReservationChangeWindow win, ReservationRequests reservationRequest)
        {
            InitializeComponent();
            DataContext = this;
            resWin = win;
            this.reservationRequest = reservationRequest;
        }

        private void SaveCommentClick(object sender, RoutedEventArgs e)
        {
            ReservationRequests request=resWin.reservationRequests.Find(s=>resWin.SelectedItem.ReservationId==s.ReservationId);
            resWin.reservationRequestsRepository.UpdateDecline(request, Comment);
            resWin.AddGuest1Notification(reservationRequest);
            resWin.requestsObservable.Remove(resWin.SelectedItem);
            this.Close();

        }
    }
}

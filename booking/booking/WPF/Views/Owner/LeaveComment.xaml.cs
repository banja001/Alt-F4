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
    public partial class LeaveComment : Window
    {
        public string Comment { get; set; }

        public ReservationChange resWin;
        public LeaveComment(ReservationChange win)
        {
            InitializeComponent();
            DataContext = this;
            resWin = win;

        }

        private void SaveCommentClick(object sender, RoutedEventArgs e)
        {
            ReservationRequests r=resWin.requests.Find(s=>resWin.SelectedItem.ReservationChangeId==s.ReservationId);
            resWin.repository.UpdateCancel(r, Comment);
            resWin.requestsObservable.Remove(resWin.SelectedItem);
            this.Close();

        }
    }
}

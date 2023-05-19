using booking.Model;
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
using booking.WPF.Views.Guide;

namespace booking.View.Guide
{
    /// <summary>
    /// Interaction logic for GuideWindow.xaml
    /// </summary>
    public partial class GuideWindow : Window
    {
        public User Guide = new User();
        public GuideWindow(User user)
        {
            InitializeComponent();
            Guide= user;
        }

        private void AddTour(object sender, RoutedEventArgs e)
        {
            //AddTourWindow addTourWindow = new AddTourWindow();
           // addTourWindow.ShowDialog();
        }

        private void LiveTrackingTour(object sender, RoutedEventArgs e)
        {
            
            LiveTrackingWindow liveTrackingWindow=new LiveTrackingWindow(Guide);
            //liveTrackingWindow.ShowDialog();
        }

        private void ShowReviews_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                ShowReviews showReviews = new ShowReviews(Guide);
                //showReviews.ShowDialog();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }

        private void ShowUnfinishedTours_Click(object sender, RoutedEventArgs e)
        {
            TourCancellation tourCancellation=new TourCancellation(Guide);
            //tourCancellation.ShowDialog();
        }
    }
}

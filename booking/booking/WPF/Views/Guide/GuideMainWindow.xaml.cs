using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using booking.Model;
using booking.View.Guide;
using booking.WPF.Views.Guide;

namespace WPF.Views.Guide
{
    /// <summary>
    /// Interaction logic for GuideMainWindow.xaml
    /// </summary>
    public partial class GuideMainWindow : Window
    {
        public User Guide { get; set; }

        public GuideMainWindow(User guide)
        {
            InitializeComponent();
            Guide = guide;
            DataContext=this;
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Content.NavigationService.Navigate(new AddTourWindow());
        }
        private void Border1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Content.NavigationService.Navigate(new LiveTrackingWindow(Guide));
        }
        private void Border2_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Content.NavigationService.Navigate(new TourCancellation(Guide));
        }
        private void Border3_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Content.NavigationService.Navigate(new ShowReviews(Guide));
        }
        private void Border4_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Content.NavigationService.Navigate(new AddTourWindow());
        }
        private void Border5_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Content.NavigationService.Navigate(new AddTourWindow());
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Content.NavigationService.Navigate(new ProfilePage(Guide));
        }

        private void Image_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            Content.NavigationService.Navigate(new ProfilePage(Guide));

        }
    }
}

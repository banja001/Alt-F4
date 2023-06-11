using booking.View;
using booking.View.Owner;
using booking.WPF.Views.Owner;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPF.ViewModels.Owner;

namespace WPF.Views.Owner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {



        public int OwnerId;
        public OwnerWindow OwnerWindow;
        public OwnerViewModel OwnerModel;
        public static MainWindow w;

        public MainWindow(int id)
        { 
            InitializeComponent();
            OwnerId = id;
            OwnerWindow = new OwnerWindow(id);
            OwnerModel=OwnerWindow.OwnerModel;



            Main.Content = OwnerWindow;

            

            w = this;
        }
        private void BackClick(object sender, RoutedEventArgs e)
        {
            if (Main.NavigationService.CanGoBack)
            {
                Main.NavigationService.GoBack();
            }
        }

        private void ForwardClick(object sender, RoutedEventArgs e)
        {
            if (Main.NavigationService.CanGoForward)
            {
                Main.NavigationService.GoForward();
            }
        }
        private void OwnerDropdownClick(object sender, RoutedEventArgs e)
        {
            Main.Content=OwnerWindow;
            dropdownMenu.IsOpen = false;

        }
        private void AddAccommodationDropdownClick(object sender, RoutedEventArgs e)
        {
            Main.Content=new AddAccommodationWindow(OwnerModel);
            dropdownMenu.IsOpen = false;
        }

        private void ViewRatingsDropdownClick(object sender, RoutedEventArgs e)
        {
            Main.Content = new RatingViewWindow(OwnerModel);
            dropdownMenu.IsOpen = false;
        }

        private void ManageReservationsDropdownClick(object sender, RoutedEventArgs e)
        {
            Main.Content = new ReservationChangeWindow(OwnerModel);
            dropdownMenu.IsOpen = false;
        }
        private void AccommodationStatsClick(object sender, RoutedEventArgs e)
        {
            Main.Content = new AccommodationStats(OwnerModel);
            dropdownMenu.IsOpen = false;
        }
        private void ScheduleRenovationClick(object sender, RoutedEventArgs e)
        {
            Main.Content = new ScheduleRenovation(OwnerModel);
            dropdownMenu.IsOpen = false;
        }
        private void ViewRenovationClick(object sender, RoutedEventArgs e)
        {
            Main.Content = new RenovationView(OwnerModel);
            dropdownMenu.IsOpen = false;
        }
        public void ViewForumsClick(object sender, RoutedEventArgs e)
        {
            Main.Content = new ForumSelect(OwnerId);
            dropdownMenu.IsOpen = false;
        }

    }
}

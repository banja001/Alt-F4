using booking.View;
using booking.View.Owner;
using booking.WPF.Views.Owner;
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

        public MainWindow(int id)
        {
            InitializeComponent();
            OwnerId = id;
            OwnerWindow = new OwnerWindow(id,this);
            OwnerModel=OwnerWindow.OwnerModel;
            Main.Content = OwnerWindow;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Main.Content=OwnerWindow;   
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Main.Content=new AddAccommodationWindow(OwnerModel);
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

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Main.Content = new RatingViewWindow(OwnerModel);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Main.Content = new ReservationChangeWindow(OwnerModel);
        }
    }
}

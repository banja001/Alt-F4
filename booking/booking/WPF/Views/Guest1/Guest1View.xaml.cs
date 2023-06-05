using booking.Commands;
using booking.Domain.DTO;
using booking.Domain.Model;
using booking.DTO;
using booking.Model;
using booking.Repositories;
using booking.Repository;
using booking.View.Guest1;
using booking.WPF.Views.Guest1;
using Domain.Model;
using Repositories;
using Syncfusion.Windows.Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Security.Policy;
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
using WPF.Views;
using WPF.Views.Guest1;
using Overview = WPF.Views.Guest1.Overview;

namespace booking.View
{
    /// <summary>
    /// Interaction logic for AccomodationOverview.xaml
    /// </summary>
    public partial class Guest1View : Window
    {
        private ReservationsViewModel _reservationViewModel;
        private RateAccommodationAndOwnerViewModel _rateAccommodationAndOwner;
        private Guest1ViewViewModel _guest1ViewViewModel;
        private Overview _overview;
        private AnytimeAnywhereView _anytimeAnywhereView;
        private ForumViewModel _forumViewModel;

        public Guest1View(int id,SignInForm sign)
        {
            InitializeComponent();

            _reservationViewModel = new ReservationsViewModel(id, this);
            _rateAccommodationAndOwner = new RateAccommodationAndOwnerViewModel(id, this);
            _guest1ViewViewModel = new Guest1ViewViewModel(id);
            _overview = new Overview(id);
            _anytimeAnywhereView = new AnytimeAnywhereView(id);
            _forumViewModel = new ForumViewModel(id);


            fOverviewAnywhere.Content = _overview;

            this.DataContext = _guest1ViewViewModel;
            fOverviewAnywhere.DataContext = _overview;
            tabItemRate.DataContext = _rateAccommodationAndOwner;
            tabItemReservations.DataContext = _reservationViewModel;
            tabItemForums.DataContext = _forumViewModel;   
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Escape)
                this.Close();
        }

        private void RadioButton_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                if (sender.ToString().Contains("Overview") && rbAnywhereAnytime.IsChecked == true)
                {
                    rbOverview.IsChecked = true;
                    rbAnywhereAnytime.IsChecked = false;

                    fOverviewAnywhere.DataContext = _overview;
                    fOverviewAnywhere.Content = _overview;
                    lbHeader.Content = "Accommodation overview";
                }
                else if (sender.ToString().Contains("Anywhere, anytime") && rbOverview.IsChecked == true)
                {
                    rbOverview.IsChecked = false;
                    rbAnywhereAnytime.IsChecked = true;

                  
                    fOverviewAnywhere.DataContext = _anytimeAnywhereView;
                    fOverviewAnywhere.Content = _anytimeAnywhereView;
                    lbHeader.Content = "Anywhere, anytime!";
                }
        }

        private void rbOverview_Click(object sender, RoutedEventArgs e)
        {
            rbOverview.IsChecked = true;
            rbAnywhereAnytime.IsChecked = false;

            fOverviewAnywhere.DataContext = _overview;
            fOverviewAnywhere.Content = _overview;
            lbHeader.Content = "Accommodation overview";
        }

        private void rbAnywhereAnytime_Checked(object sender, RoutedEventArgs e)
        {
            rbOverview.IsChecked = false;
            rbAnywhereAnytime.IsChecked = true;


            fOverviewAnywhere.DataContext = _anytimeAnywhereView;
            fOverviewAnywhere.Content = _anytimeAnywhereView;
            lbHeader.Content = "Anywhere, anytime!";
        }
    }
}

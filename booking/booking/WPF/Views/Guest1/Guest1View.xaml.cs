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

namespace booking.View
{
    /// <summary>
    /// Interaction logic for AccomodationOverview.xaml
    /// </summary>
    public partial class Guest1View : Window
    {

        public Image SelectedAddedImages { get; set; }
        public SignInForm signInWindow { get; set; }

        

        private ReservationsViewModel _reservationViewModel;
        private RateAccommodationAndOwnerViewModel _rateAccommodationAndOwner;
        private Guest1ViewViewModel _guest1ViewViewModel;

        public Guest1View(int id,SignInForm sign)
        {
            InitializeComponent();

            _reservationViewModel = new ReservationsViewModel(id, this);
            _rateAccommodationAndOwner = new RateAccommodationAndOwnerViewModel(id, this);
            _guest1ViewViewModel = new Guest1ViewViewModel(id);

            this.DataContext = _guest1ViewViewModel;
            tabItemOverview.DataContext = _guest1ViewViewModel;
            tabItemRate.DataContext = _rateAccommodationAndOwner;
            tabItemReservations.DataContext = _reservationViewModel;
            tabItemForums.DataContext = this;

            signInWindow = sign;
            
            InitializeCheckBoxes();

            Guest1ViewViewModel guest1ViewViewModel = new Guest1ViewViewModel(id);
        }

        private void InitializeCheckBoxes()
        {
            CheckBoxApartment.IsChecked = true;
            CheckBoxCabin.IsChecked = true;
            CheckBoxHouse.IsChecked = true;
        }
        
        private void tbImageUrl_TextChanged(object sender, TextChangedEventArgs e)
        {
            bAddImage.IsEnabled = true;
        }

        public void ClearImgUrlTextBox()
        {
            tbImageUrl.Text = "";
        }

        private void CleanStarsClick(object sender, MouseButtonEventArgs e)
        {
            RateAccommodationAndOwnerViewModel.CleanRating = stClean.Value;
        }
        private void OwnersKindnessStarsClick(object sender, MouseButtonEventArgs e)
        {
            RateAccommodationAndOwnerViewModel.OwnersKindenssRating = stOwner.Value;
        }
    }
}

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
            
            InitializeCheckBoxes();
        }

        private void InitializeCheckBoxes()
        {
            CheckBoxApartment.IsChecked = true;
            CheckBoxCabin.IsChecked = true;
            CheckBoxHouse.IsChecked = true;
        }

        private void CheckBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                if (sender.ToString().Contains("Apartment"))
                    CheckBoxApartment.IsChecked = !CheckBoxApartment.IsChecked;
                else if (sender.ToString().Contains("House"))
                    CheckBoxHouse.IsChecked = !CheckBoxHouse.IsChecked;
                else
                    CheckBoxCabin.IsChecked = !CheckBoxCabin.IsChecked;
        }

        private void RadioButton_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                if (sender.ToString().Contains("Overview") && rbAnywhereAnytime.IsChecked == true)
                {
                    rbOverview.IsChecked = true;
                    rbAnywhereAnytime.IsChecked = false;
                }
                else if (sender.ToString().Contains("Anywhere, anytime") && rbOverview.IsChecked == true)
                {
                    rbOverview.IsChecked = false;
                    rbAnywhereAnytime.IsChecked = true;
                }
            else if(e.Key == Key.Down && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                FocusManager.SetFocusedElement(CheckBoxApartment.Parent, CheckBoxApartment);
            } 
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Escape)
                this.Close();
        }

        private void GroupBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Down && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                FocusManager.SetFocusedElement(accommodationData.Parent, accommodationData);
            }
            else if (e.Key == Key.W && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                FocusManager.SetFocusedElement(rbOverview.Parent, rbOverview);
            }
        }

        private void accommodationData_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                FocusManager.SetFocusedElement(ReserveAccommodationButton.Parent, ReserveAccommodationButton);
            }
        }
    }
}

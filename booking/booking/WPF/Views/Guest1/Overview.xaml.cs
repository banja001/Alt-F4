using application.UseCases;
using booking.application.usecases;
using booking.Commands;
using booking.DTO;
using booking.Model;
using booking.View.Guest1;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using WPF.ViewModels.Guest1;

namespace WPF.Views.Guest1
{
    /// <summary>
    /// Interaction logic for Overview.xaml
    /// </summary>
    public partial class Overview : Page
    {
        private int userId;

        private readonly OverviewViewModel _overviewViewModel;

        public Overview(int id)
        {
            InitializeComponent();

            this.userId = id;

            _overviewViewModel = new OverviewViewModel(id);

            this.DataContext = _overviewViewModel;
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

        private void GroupBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Down && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                FocusManager.SetFocusedElement(accommodationData.Parent, accommodationData);
            }
        }

        private void accommodationData_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.H && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                FocusManager.SetFocusedElement(ReserveAccommodationButton.Parent, ReserveAccommodationButton);
            }
        }
    }
}

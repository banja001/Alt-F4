using booking.DTO;
using booking.Model;
using booking.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using booking.Converter;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using WPF.ViewModels.Guest1;

namespace booking.View.Guest1
{
    /// <summary>
    /// Interaction logic for ReserveAccommodation.xaml
    /// </summary>
    public partial class ReserveAccommodation : Window
    {
        ReserveAccommodationViewModel reserveAccommodationViewModel;

        public ReserveAccommodation(int userId)
        {
            InitializeComponent();

            reserveAccommodationViewModel = new ReserveAccommodationViewModel(userId);
            DataContext = reserveAccommodationViewModel;

        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                this.Close();
        }

        private void accommodationData_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Down && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                FocusManager.SetFocusedElement(udNumOfGuests.Parent, udNumOfGuests);
            }
            else if(e.Key == Key.Up && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                FocusManager.SetFocusedElement(cStartDate.Parent, cStartDate);
            }
        }

        private void udNumOfGuests_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.W && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                FocusManager.SetFocusedElement(cStartDate.Parent, cStartDate);
            }
        }
    }
}

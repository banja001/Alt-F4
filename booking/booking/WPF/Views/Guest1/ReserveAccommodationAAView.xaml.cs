using booking.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WPF.ViewModels.Guest1;

namespace WPF.Views.Guest1
{
    /// <summary>
    /// Interaction logic for ReserveAccommodationAAView.xaml
    /// </summary>
    public partial class ReserveAccommodationAAView : Window
    {
        public ReserveAccommodationAAView(ObservableCollection<ReservedDates> dates, int accommodationId, int userId, int numOfGuests)
        {
            InitializeComponent();
            DataContext = new ReserveAccommodationAAViewModel(dates, accommodationId, userId, numOfGuests);
        }
    }
}

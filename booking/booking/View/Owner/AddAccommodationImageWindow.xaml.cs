using booking.Manager;
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

namespace booking.View.Owner
{
    /// <summary>
    /// Interaction logic for AddAccommodationImageWindow.xaml
    /// </summary>
    public partial class AddAccommodationImageWindow : Window
    {
        private AccommodationImageRepository accommodationImageRepository;
        public AddAccommodationImageWindow(AccommodationImageRepository acci)
        {
            InitializeComponent();
            DataContext = this;
            accommodationImageRepository = acci;
        }

        public void ConfirmImageClick(object sender, RoutedEventArgs e)
        {

        }

        public void CancelImageClick(object sender, RoutedEventArgs e)
        {

        }
    }
}

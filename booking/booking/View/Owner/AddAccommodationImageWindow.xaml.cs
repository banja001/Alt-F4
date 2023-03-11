using booking.Manager;
using booking.Model;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace booking.View.Owner
{
    /// <summary>
    /// Interaction logic for AddAccommodationImageWindow.xaml
    /// </summary>
    public partial class AddAccommodationImageWindow : Window
    {
        private AccommodationImageRepository accommodationImageRepository;
        private List<string> accommodationImages;
        public AddAccommodationImageWindow(AccommodationImageRepository acci,List<string> images)
        {
            InitializeComponent();
            DataContext = this;
            accommodationImageRepository = acci;
            accommodationImages = images;  

        }

        public void ConfirmImageClick(object sender, RoutedEventArgs e)
        {
            accommodationImages.Add(UrlTextBox.Text);

        }

        public void CancelImageClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

using booking.Model;
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

namespace booking.View.Guest2
{
    /// <summary>
    /// Interaction logic for MoreDetailsOverview.xaml
    /// </summary>
    public partial class MoreDetailsOverview : Window
    {
        private List<TourImage> TourImages;

        private int currentImageIndex;
        private String Description { get; set; }
        public MoreDetailsOverview(Guest2Overview guest2Overview)
        {
            InitializeComponent();
            this.DataContext = this;
            currentImageIndex = 0;
            showInitalDetails(guest2Overview);
        }

        private void SwipeLeftButtonClick(object sender, RoutedEventArgs e)
        {
            if (currentImageIndex == 0)
            {
                currentImageIndex = TourImages.Count - 1;
                changePresentImage();
            }
            else
            {
                currentImageIndex--;
                changePresentImage();
            }

        }

        private void SwipeRightButtonClick(object sender, RoutedEventArgs e)
        {
            if (currentImageIndex == TourImages.Count - 1)
            {
                currentImageIndex = 0;
                changePresentImage();
            }
            else
            {
                currentImageIndex++;
                changePresentImage();
            }
        }
        private void showInitalDetails(Guest2Overview guest2Overview)
        {
            Description = guest2Overview.SelectedTour.Description;
            DescriptionTextBlock.Text = Description;
            TourImages = guest2Overview.SelectedTour.Images.ToList();
            if (TourImages.Count() != 0)
            {
                changePresentImage();
            }
            else if(TourImages.Count() == 0)
            {
                swipeLeftButton.IsEnabled = false;
                swipeRightButton.IsEnabled = false;
                changePresentImage();
            }
        }

        private void changePresentImage()
        {
            if (TourImages.Count != 0)
            {
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.UriSource = new Uri(@TourImages[currentImageIndex].Url, UriKind.Absolute);
                bitmapimage.EndInit();
                PresentTourImage.Source = bitmapimage;
            }
        }
    }
}

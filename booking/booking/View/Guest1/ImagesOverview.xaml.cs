using booking.DTO;
using booking.Model;
using booking.Repository;
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
using static System.Net.Mime.MediaTypeNames;

namespace booking.View.Guest1
{
    /// <summary>
    /// Interaction logic for ImagesOverview.xaml
    /// </summary>
    public partial class ImagesOverview : Window
    {
        AccommodationLocationDTO accommodation;

        private readonly AccommodationImageRepository _repository;
        public List<AccommodationImage> AccommodationImages { get; set; }

        private int activeImageIndx;

        public ImagesOverview()
        {
            InitializeComponent();
        }

        public ImagesOverview(AccommodationLocationDTO accommodation)
        {
            InitializeComponent();
            DataContext = this;

            this.accommodation = accommodation;

            _repository = new AccommodationImageRepository();


            AccommodationImages = _repository.Get(accommodation);

            ShowImage();
        }

        public void SetImageSource(string url)
        {
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(@url, UriKind.Absolute);
            bitmapImage.EndInit();
            AccommodationImage.Source = bitmapImage;
        }

        public void ShowImage()
        {
            if (AccommodationImages.Count == 0)
            {
                NoImagesLabel.Content = "No images for display";
                return;
            }

            activeImageIndx = 0;
            SetImageSource(AccommodationImages[activeImageIndx].Url);
            CheckIndexScope();
        }

        public void NextPictureClick(object sender, RoutedEventArgs e)
        {
            SetImageSource(AccommodationImages[++activeImageIndx].Url);
            CheckIndexScope();
        }

        private void PrevImageButtonClick(object sender, RoutedEventArgs e)
        {
            SetImageSource(AccommodationImages[--activeImageIndx].Url);
            CheckIndexScope();
        }

        public void CheckIndexScope()
        {
            NextImageButton.IsEnabled = activeImageIndx + 1 == AccommodationImages.Count ? false : true;
            PrevImageButton.IsEnabled = activeImageIndx == 0 ? false : true;
        }
    }
}

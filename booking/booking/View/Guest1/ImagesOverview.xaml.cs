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

namespace booking.View.Guest1
{
    /// <summary>
    /// Interaction logic for ImagesOverview.xaml
    /// </summary>
    public partial class ImagesOverview : Window
    {
        AccommodationLocationDTO accommodation;

        private readonly AccommodationImageRepository _repository;
        private List<AccommodationImage> images;
        public List<AccommodationImage> AccommodationImages { get; set; }

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

            images = _repository.GetAllImages();
            AccommodationImages = new List<AccommodationImage>();

            GetAccommodationImages();
            ShowImage();
        }

        public void GetAccommodationImages()
        {
            foreach(AccommodationImage image in images)
            {
                if(image.AccomodationId == accommodation.Id)
                {
                    AccommodationImages.Add(image);
                }
            }
        }

        public void ShowImage()
        {
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(AccommodationImages[0].Url, UriKind.Relative);
            bitmapImage.EndInit();

            AccommodationImage.Source = bitmapImage;
        }
    }
}

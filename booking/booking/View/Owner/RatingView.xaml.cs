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

namespace booking.View.Owner
{
    /// <summary>
    /// Interaction logic for RatingView.xaml
    /// </summary>
    public partial class RatingView : Window
    {
        

        private int ActiveImageIndx;

        private OwnerWindow win;
        
        public ObservableCollection<OwnerRatingDTO> OwnerRatings { get; set; }
        public OwnerRatingDTO SelectedItem { get; set; }
        public RatingView(OwnerWindow win)
        {
            InitializeComponent();
            DataContext = this;
            NextImageButton.IsEnabled = false;
            PrevImageButton.IsEnabled = false;
            this.win = win;
            

           
            

            OwnerRatings = new ObservableCollection<OwnerRatingDTO>();
            foreach(OwnerRating OwnerRating in win.OwnerRatings)
            {
                ReservedDates res=win.reservedDates.Find(s => s.Id == OwnerRating.ReservationId);
                if (res == null) continue;
                else if (res.Rated == 1 && OwnerRating.OwnerId==win.OwnerId)
                {
                    OwnerRatingDTO ow=new OwnerRatingDTO(win.users.Find(s=>s.Id==res.UserId).Username,OwnerRating.CleanRating,OwnerRating.KindRating,OwnerRating.Comment,OwnerRating.ReservationId);
                    


                    OwnerRatings.Add(ow);
                }


            }
            
        }

        public void DatagridSelectionChange(object sender, RoutedEventArgs e)
        {
            ActiveImageIndx = 0;
            int a = SelectedItem.ReservationId;
            win.OwnerRatingImages = win.OwnerRatingImageRepository.Get(a);
            ShowImage();
        }

        public void SetImageSource(string url)
        {

            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(@url, UriKind.Absolute);
            bitmapImage.EndInit();
            OwnerImage.Source = bitmapImage;

        }
        public void ShowImage()
        {
            CheckIndexScope();
            if (win.OwnerRatingImages.Count == 0)
            {
                NoImagesLabel.Content = "No images for display";
                return;
            }

            ActiveImageIndx = 0;
            SetImageSource(win.OwnerRatingImages[ActiveImageIndx].Url);
            
        }

        public void NextPictureClick(object sender, RoutedEventArgs e)
        {
            SetImageSource(win.OwnerRatingImages[++ActiveImageIndx].Url);
            CheckIndexScope();
        }

        private void PrevImageButtonClick(object sender, RoutedEventArgs e)
        {
            SetImageSource(win.OwnerRatingImages[--ActiveImageIndx].Url);
            CheckIndexScope();
        }
        public void CheckIndexScope()
        {
            if (ActiveImageIndx + 1 >= win.OwnerRatingImages.Count)
            {
                NextImageButton.IsEnabled = false;
            }
            else
            {
                NextImageButton.IsEnabled = true;
            }

            if (ActiveImageIndx <= 0)
            {
                PrevImageButton.IsEnabled = false;
            }
            else
            {
                PrevImageButton.IsEnabled = true;
            }
        }
    }
}

using booking.DTO;
using booking.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Windows.Input;
using booking.Commands;
using Syncfusion.Windows.Shared;
using booking.WPF.ViewModels;

namespace WPF.ViewModels
{
    public class MoreDetailsViewModel : BaseViewModel
    {
        private List<TourImage> TourImages;
        private TourLocationDTO SelectedTour;
        public BitmapSource ImageSource { get; set; }
        private int currentImageIndex;
        public String Description { get; set; }
        public ICommand SwipeLeftCommand => new RelayCommand(OnSwipeLeftButtonClick, CanSwipeLeftButtonClick);
        public ICommand SwipeRightCommand => new RelayCommand(OnSwipeRightButtonClick, CanSwipeRightButtonClick);
        public ICommand CloseWindowCommand => new RelayCommand(OnCloseButtonClick);

        public MoreDetailsViewModel(TourLocationDTO selectedTour)
        {
            SelectedTour = selectedTour;
            currentImageIndex = 0;
            showInitalDetails(SelectedTour);
        }
        private void OnCloseButtonClick()
        {
            this.CloseCurrentWindow();
        }

        private void OnSwipeLeftButtonClick()
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
        private bool CanSwipeLeftButtonClick()
        {
            return TourImages.Count() > 1;
        }
        private bool CanSwipeRightButtonClick()
        {
            return (TourImages.Count() > 1);
        }

        private void OnSwipeRightButtonClick()
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
        private void showInitalDetails(TourLocationDTO selectedTour)
        {
            Description = selectedTour.Description;
            TourImages = selectedTour.Images.ToList();
            if (TourImages.Count() != 0)
            {
                changePresentImage();
            }
            else if (TourImages.Count() == 0)
            {
                changePresentImage();
            }
        }

        private void changePresentImage()
        {
            if (TourImages.Count != 0)
            {
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                if (TourImages[currentImageIndex].Url == "")
                {
                    string url = "https://www.freeiconspng.com/img/23483";
                    bitmapimage.UriSource = new Uri(@url, UriKind.Absolute);
                    bitmapimage.EndInit();
                    ImageSource = bitmapimage;
                    OnPropertyChanged(nameof(ImageSource));
                    return;
                }
                bitmapimage.UriSource = new Uri(@TourImages[currentImageIndex].Url, UriKind.Absolute);
                bitmapimage.EndInit();
                ImageSource = bitmapimage;
                OnPropertyChanged(nameof(ImageSource));
            }
        }
    }
}

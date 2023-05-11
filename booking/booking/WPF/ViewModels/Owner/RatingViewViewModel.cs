using booking.DTO;
using booking.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Collections.ObjectModel;
using booking.Model;
using booking.View.Owner;
using booking.Commands;
using System.Windows.Input;
using System.Windows.Media;

namespace WPF.ViewModels.Owner
{
    public class RatingViewViewModel:BaseViewModel
    {
        private bool nextButtonEnabled;

        public bool NextButtonEnabled
        {
            get
            {
                return nextButtonEnabled;
            }
            set
            {
                if (value != nextButtonEnabled)
                {
                    nextButtonEnabled = value;
                    OnPropertyChanged("NextButtonEnabled");
                }
            }
        }

        private bool prevButtonEnabled;

        public bool PrevButtonEnabled
        {
            get
            {
                return prevButtonEnabled;
            }
            set
            {
                if (value != prevButtonEnabled)
                {
                    prevButtonEnabled = value;
                    OnPropertyChanged("PrevButtonEnabled");
                }
            }
        }

        private BitmapImage imageSource;

        public BitmapImage ImageSource
        {
            get
            {
                return imageSource;
            }
            set
            {
                if (value != imageSource)
                {
                    imageSource = value;
                    OnPropertyChanged("ImageSource");
                }
            }
        }

        private int ActiveImageIndx;
        public string Comment { get; set; }
        private OwnerViewModel win;
        //public RatingViewWindow viewWindow;
        public ObservableCollection<OwnerRatingDTO> OwnerRatings { get; set; }
        private OwnerRatingDTO selectedItem;
        public OwnerRatingDTO SelectedItem
        {
            get
            {
                return selectedItem;
            }
            set
            {
                selectedItem = value;
                OnPropertyChanged("SelectedItem");
                DatagridSelectionChange();
            }
        
        }
        
        public ICommand NextCommand => new RelayCommand(NextPictureClick);
        public ICommand PrevCommand => new RelayCommand(PrevImageButtonClick);

        public RatingViewViewModel(OwnerViewModel win)
        {
            this.win = win;
            NextButtonEnabled = false;
            PrevButtonEnabled = false;
            //this.viewWindow = viewWindow;

            OwnerRatings = new ObservableCollection<OwnerRatingDTO>();
            AddRatingsToView();
        }

        private void AddRatingsToView()
        {
            foreach (OwnerRating OwnerRating in win.OwnerRatings)
            {
                ReservedDates res = win.reservedDates.Find(s => s.Id == OwnerRating.ReservationId);
                if (res == null) continue;
                else if (res.RatedByOwner == true && res.RatedByGuest == true && OwnerRating.OwnerId == win.OwnerId)
                {
                    OwnerRatingDTO ow = new OwnerRatingDTO(win.users.Find(s => s.Id == res.UserId).Username, OwnerRating.CleanRating, OwnerRating.KindRating, OwnerRating.Comment, OwnerRating.ReservationId);
                    OwnerRatings.Add(ow);
                }
            }
        }

        public void DatagridSelectionChange()
        {
            ActiveImageIndx = 0;
            int a = SelectedItem.ReservationId;
            win.OwnerRatingImages = win.OwnerRatingImageService.GetByReservedDatesId(a);
            ShowImage();
        }
        public void SetImageSource(string url)
        {
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(@url, UriKind.Absolute);
            bitmapImage.EndInit();
            ImageSource = bitmapImage;
        }
        public void ShowImage()
        {
            CheckIndexScope();
            if (win.OwnerRatingImages.Count == 0)
            {
                ImageSource = null;
                //viewWindow.NoImagesLabel.Content = "No images for display";
                return;
            }

            ActiveImageIndx = 0;
            SetImageSource(win.OwnerRatingImages[ActiveImageIndx].Url);

        }

        public void NextPictureClick()
        {
            SetImageSource(win.OwnerRatingImages[++ActiveImageIndx].Url);
            CheckIndexScope();
        }

        private void PrevImageButtonClick()
        {
            SetImageSource(win.OwnerRatingImages[--ActiveImageIndx].Url);
            CheckIndexScope();
        }
        public void CheckIndexScope()
        {
            if (ActiveImageIndx + 1 >= win.OwnerRatingImages.Count)
            {
                NextButtonEnabled = false;
            }
            else
            {
                NextButtonEnabled = true;
            }

            if (ActiveImageIndx <= 0)
            {
                PrevButtonEnabled = false;
            }
            else
            {
                PrevButtonEnabled = true;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using booking.Commands;
using System.Windows.Input;
using booking.Domain.DTO;
using booking.Domain.Model;
using booking.Model;
using booking.Repositories;
using booking.WPF.ViewModels;
using System.ComponentModel;

namespace WPF.ViewModels
{
    public class SelectedCommentViewModel: BaseViewModel,INotifyPropertyChanged
    {
        public User Guide { get; set; }
        private readonly GuideRatingRepository _guideRatingRepository;
        public TourRatingDTO Comment { get; set; }
        private bool firstStar;

        public bool FirstStar
        {
            get { return firstStar; }
            set
            {
                if (firstStar != value)
                {
                    firstStar = value;
                    OnPropertyChanged(nameof(FirstStar));
                }
            }
        }

        private bool secondStar;

        public bool SecondStar
        {
            get { return secondStar; }
            set
            {
                if (secondStar != value)
                {
                    secondStar = value;
                    OnPropertyChanged(nameof(SecondStar));
                }
            }
        }

        private bool thirdStar;

        public bool ThirdStar
        {
            get { return thirdStar; }
            set
            {
                if (thirdStar != value)
                {
                    thirdStar = value;
                    OnPropertyChanged(nameof(ThirdStar));
                }
            }
        }

        private bool fourthStar;

        public bool FourthStar
        {
            get { return fourthStar; }
            set
            {
                if (fourthStar != value)
                {
                    fourthStar = value;
                    OnPropertyChanged(nameof(FourthStar));
                }
            }
        }

        private bool fifthStar;

        public bool FifthStar
        {
            get { return fifthStar; }
            set
            {
                if (fifthStar != value)
                {
                    fifthStar = value;
                    OnPropertyChanged(nameof(FifthStar));
                }
            }
        }

        public SelectedCommentViewModel(User guide, TourRatingDTO rating)
        {
            Guide = guide;
            Comment = rating;
            _guideRatingRepository= new GuideRatingRepository();
            FifthStar = true;
            ThirdStar = true;
            FourthStar = true;
            FirstStar = true;
            SecondStar = true;
            RatingStars();
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public ICommand ReportCommand => new RelayCommand(ReportReview,CanReport);
        public ICommand ExitWindowCommand => new RelayCommand(ExitWindow);

        public void ReportReview()
        {
            if (!Comment.Rating.IsValid)
                MessageBox.Show("This review is already reported!", "Warning",
                    MessageBoxButton.OK);
            else
            {
                if (MessageBox.Show("Are you sure you want to report this review?", "Warning",
                        MessageBoxButton.YesNo,MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    Comment.Rating.IsValid = false;
                    _guideRatingRepository.Update(Comment.Rating);
                    ExitWindow();
                }
            }
            
        }

        public bool CanReport()
        {
            return Comment.Rating.IsValid;
        }
        private void ExitWindow()
        {
            this.CloseCurrentWindow();
        }
        private void RatingStars()
        {
            if(Comment.AverageRating<5)
                FifthStar = false;
            if (Comment.AverageRating < 4)
                FourthStar = false;
            if (Comment.AverageRating < 3)
                ThirdStar = false;
            if (Comment.AverageRating < 2)
                SecondStar = false;
            if (Comment.AverageRating < 1)
                FirstStar = false;
        }
    }
}

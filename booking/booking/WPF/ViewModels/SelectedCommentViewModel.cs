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
using System.Windows.Media;
using System.Threading.Tasks;
using System.Threading;

namespace WPF.ViewModels
{
    public class SelectedCommentViewModel: BaseViewModel,INotifyPropertyChanged
    {
        public User Guide { get; set; }
        private readonly GuideRatingRepository _guideRatingRepository;
        public TourRatingDTO Comment { get; set; }
        private Brush firstStarColor;

        public Brush FirstStarColor
        {
            get { return firstStarColor; }
            set
            {
                if (firstStarColor != value)
                {
                    firstStarColor = value;
                    OnPropertyChanged(nameof(FirstStarColor));
                }
            }
        }

        private Brush secondStarColor;

        public Brush SecondStarColor
        {
            get { return secondStarColor; }
            set
            {
                if (secondStarColor != value)
                {
                    secondStarColor = value;
                    OnPropertyChanged(nameof(SecondStarColor));
                }
            }
        }

        private Brush thirdStarColor;

        public Brush ThirdStarColor
        {
            get { return thirdStarColor; }
            set
            {
                if (thirdStarColor != value)
                {
                    thirdStarColor = value;
                    OnPropertyChanged(nameof(ThirdStarColor));
                }
            }
        }

        private Brush fourthStarColor;

        public Brush FourthStarColor
        {
            get { return fourthStarColor; }
            set
            {
                if (fourthStarColor != value)
                {
                    fourthStarColor = value;
                    OnPropertyChanged(nameof(FourthStarColor));
                }
            }
        }

        private Brush fifthStarColor;

        public Brush FifthStarColor
        {
            get { return fifthStarColor; }
            set
            {
                if (fifthStarColor != value)
                {
                    fifthStarColor = value;
                    OnPropertyChanged(nameof(FifthStarColor));
                }
            }
        }
        public Brush FilledStar { get; set; }
        public Brush EmptyStar { get; set; }
        public SelectedCommentViewModel(User guide, TourRatingDTO rating,bool demoOn)
        {
            Guide = guide;
            Comment = rating;
            _guideRatingRepository= new GuideRatingRepository();
            FilledStar = new SolidColorBrush(Colors.Gold);
            EmptyStar = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FEFFDE"));
            FifthStarColor = FilledStar;
            ThirdStarColor = FilledStar;
            FourthStarColor = FilledStar;
            FirstStarColor = FilledStar;
            SecondStarColor = FilledStar;
            RatingStars();
            if (demoOn)
                DemoIsOn(new CancellationToken());
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
                FifthStarColor = EmptyStar;
            if (Comment.AverageRating < 4)
                FourthStarColor = EmptyStar;
            if (Comment.AverageRating < 3)
                ThirdStarColor = EmptyStar;
            if (Comment.AverageRating < 2)
                SecondStarColor = EmptyStar;
            if (Comment.AverageRating < 1)
                FirstStarColor = EmptyStar;
        }
        private async Task DemoIsOn(CancellationToken ct)
        {

            ct.ThrowIfCancellationRequested();
            await Task.Delay(2000, ct);
            ReportReview();
            await Task.Delay(2000, ct);
            ExitWindow();
        }
    }
}

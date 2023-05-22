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

namespace WPF.ViewModels
{
    public class SelectedCommentViewModel: BaseViewModel
    {
        public User Guide { get; set; }
        private readonly GuideRatingRepository _guideRatingRepository;
        public TourRatingDTO Comment { get; set; }
        public SelectedCommentViewModel(User guide, TourRatingDTO rating)
        {
            Guide = guide;
            Comment = rating;
            _guideRatingRepository= new GuideRatingRepository();
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
    }
}

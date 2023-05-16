using booking.Commands;
using booking.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using WPF.Views.Guest1;

namespace WPF.ViewModels.Guest1
{
    public class SuccessfullyRatedViewModel : BaseViewModel
    {
        private int userId;

        public ICommand CloseCommand => new RelayCommand(Close);
        public ICommand ViewReviewCommand => new RelayCommand(ViewReview);
        public SuccessfullyRatedViewModel(int userId)
        {
            this.userId = userId;
        }

        private void Close()
        {
            CloseCurrentWindow();
        }

        private void ViewReview()
        {
            Close();

            ReviewView review = new ReviewView(userId);
            review.Show();
        }
    }
}

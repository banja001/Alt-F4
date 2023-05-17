using booking.Commands;
using booking.Model;
using booking.WPF.ViewModels;
using Domain.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using WPF.Views.Guest1;

namespace WPF.ViewModels.Guest1
{
    public class SuccessfullyRatedViewModel : BaseViewModel
    {
        public ObservableCollection<Guest1RatingAccommodationDTO> Guest1RatingAccommodationDTOs { get; set; }
        private int userId;

        public ICommand CloseCommand => new RelayCommand(Close);
        public ICommand ViewReviewCommand => new RelayCommand(ViewReview);
        public SuccessfullyRatedViewModel(int userId, ObservableCollection<Guest1RatingAccommodationDTO> guest1RatingAccommodationDTOs)
        {
            this.userId = userId;
            Guest1RatingAccommodationDTOs = guest1RatingAccommodationDTOs; 
        }

        private void Close()
        {
            CloseCurrentWindow();
        }

        private void ViewReview()
        {
            Close();

            ReviewView review = new ReviewView(userId, Guest1RatingAccommodationDTOs);
            review.Show();
        }
    }
}

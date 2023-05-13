using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using booking.application.UseCases;
using booking.Commands;
using booking.Domain.DTO;
using booking.Domain.Model;
using booking.Model;
using booking.Repository;
using booking.WPF.Views.Guide;
using WPF.Views.Guide;

namespace booking.WPF.ViewModels
{
    class ShowReviewsViewModel:BaseViewModel
    {
        public ObservableCollection<GuideRating> AllCommentsForThatTour { get; set; }
        private readonly AppointmentService _appointmentService;
        public TourRatingDTO SelectedComment { get; set; }
        public ObservableCollection<TourRatingDTO> AllComments { get; set; }
        public ICommand ShowCommand => new RelayCommand(Show);
        public ICommand ExitWindowCommand => new RelayCommand(ExitWindow);
        public User Guide { get; set; }

        public ShowReviewsViewModel(User guide, AppointmentGuestsDTO appointment)
        {
            _appointmentService = new AppointmentService();
            AllCommentsForThatTour=new ObservableCollection<GuideRating>(_appointmentService.FindComments(_appointmentService.FindAppointment(appointment.AppointmentId)));
            AllComments =
                new ObservableCollection<TourRatingDTO>(
                    _appointmentService.MakeTourRatings(AllCommentsForThatTour, appointment));
            SelectedComment=new TourRatingDTO();
            Guide = guide;
        }

        public void Show()
        {
            try
            {
                if (SelectedComment != null && !string.IsNullOrEmpty(SelectedComment.TourName))
                {
                    SelectedCommentWindow showComment = new SelectedCommentWindow(SelectedComment, Guide);
                    showComment.ShowDialog();
                    //ExitWindow();
                }
                else
                    MessageBox.Show("Select tour!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }
        private void ExitWindow()
        {
            this.CloseCurrentWindow();
        }
    }
}

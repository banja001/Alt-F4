using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using booking.application.UseCases;
using booking.Domain.DTO;
using booking.Domain.Model;
using booking.Model;
using booking.Repository;

namespace booking.WPF.ViewModels
{
    class ShowReviewsViewModel
    {
        public ObservableCollection<GuideRating> AllCommentsForThatTour { get; set; }
        private readonly AppointmentService _appointmentService;
        public ObservableCollection<TourRatingDTO> AllComments { get; set; }

        public ShowReviewsViewModel(User guide, AppointmentGuestsDTO appointment)
        {
            MessageBox.Show(appointment.Name);
            _appointmentService = new AppointmentService();
            AllCommentsForThatTour=new ObservableCollection<GuideRating>(_appointmentService.FindComments(_appointmentService.FindAppointment(appointment.AppointmentId)));
            AllComments =
                new ObservableCollection<TourRatingDTO>(
                    _appointmentService.MakeTourRatings(AllCommentsForThatTour, appointment));
        }

    }
}

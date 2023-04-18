using application.UseCases;
using booking.application.UseCases;
using booking.application.UseCases.Guest2;
using booking.Commands;
using booking.DTO;
using booking.Model;
using booking.Repository;
using booking.View.Owner;
using booking.WPF.Views.Guest2;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace booking.WPF.ViewModels
{
    public class MyToursViewModel : BaseViewModel
    {
        public ICommand RateGuideCommand => new RelayCommand(RateGuide);
        public ICommand JoinActiveTourCommand => new RelayCommand(JoinActiveTour);
        public ObservableCollection<Voucher> Vouchers { get; set; }
        public ObservableCollection<Appointment> CompletedTours { get; set; }
        public ObservableCollection<TourLocationDTO> ActiveTour { get; set; }
        public AppointmentCheckPoint CurrentCheckpoint { get; set; }
        public Appointment SelectedTour { get; set; }
        public Appointment ActiveAppointment { get; set; }
        private User User { get; set; }

        private readonly AppointmentService _appointmentService;
        private readonly VoucherService _voucherService;
        private readonly AppointmentCheckpointService _appointmentCheckpointService;
        private readonly TourAttendanceService _tourAttendanceService;
        public MyToursViewModel(User user) 
        {
            User = user;
            _appointmentService = new AppointmentService();
            _tourAttendanceService = new TourAttendanceService();
            _voucherService = new VoucherService();
            _appointmentCheckpointService = new AppointmentCheckpointService();
            _voucherService.GenerateNewVouchersByGuest2(user);
            SelectedTour = new Appointment();
            CompletedTours = new ObservableCollection<Appointment>(_appointmentService.GetCompletedAppointmentByGuest2(User));
            Vouchers = new ObservableCollection<Voucher>(_voucherService.GetUsableVouchersByGuest2(user));
            var activeTour = _appointmentService.GetActiveAppointmentByGuest2(user).ToList();
            if (activeTour.Count() > 0)
            {
                ActiveAppointment = activeTour[0];
                ActiveTour = new ObservableCollection<TourLocationDTO>(_appointmentService.MakeToursFrom(activeTour));
                CurrentCheckpoint = _appointmentCheckpointService.GetCurrentCheckpointFor(ActiveAppointment);
            }
        }

        private void RateGuide()
        {
            if(SelectedTour.Start.Time == null) 
            {
                MessageBox.Show("You need to select a tour!", "Alert", MessageBoxButton.OK);
            }
            else 
            {
                var rateGuideWindow = new RateGuideView(SelectedTour, User);
                rateGuideWindow.Show();
            }
            
        }
        private void JoinActiveTour()
        {
            var tourAttendance = new TourAttendance(-1, User.Id, CurrentCheckpoint.Id, true);
            _tourAttendanceService.Add(tourAttendance, ActiveAppointment);
            MessageBox.Show("Successfully joined a tour!", "Success", MessageBoxButton.OK);
        }
    }
}

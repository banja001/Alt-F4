using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using booking.application.UseCases;
using booking.Commands;
using booking.Domain.DTO;
using booking.WPF.ViewModels;
using Domain.DTO;

namespace WPF.ViewModels
{
    public class ShowStatisticsViewModel: BaseViewModel
    {
        public AppointmentGuestsDTO AppointmentGuests { get; set; }
        public AppointmentStatisticsDTO SelectedAppointmentStatistics { get; set; }
        private readonly AppointmentService _appointmentService;
        public ICommand ExitWindowCommand => new RelayCommand(ExitWindow);
        public ShowStatisticsViewModel(AppointmentGuestsDTO appointmentGuests)
        {
            AppointmentGuests=appointmentGuests;
            _appointmentService = new AppointmentService();
            SelectedAppointmentStatistics =
                _appointmentService.MakeAppointmentStatisticsDTO(AppointmentGuests.AppointmentId);
        }
        private void ExitWindow()
        {
            this.CloseCurrentWindow();
        }
    }
}

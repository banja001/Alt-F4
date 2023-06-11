using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using booking.application.UseCases;
using booking.Commands;
using booking.Domain.DTO;
using booking.WPF.ViewModels;
using Domain.DTO;
using WPF.Views.Guide;

namespace WPF.ViewModels
{
    public class ShowStatisticsViewModel: BaseViewModel
    {
        public AppointmentGuestsDTO AppointmentGuests { get; set; }
        public AppointmentStatisticsDTO SelectedAppointmentStatistics { get; set; }
        private readonly AppointmentService _appointmentService;
        public ICommand ExitWindowCommand => new RelayCommand(ExitWindow);
        public ShowStatisticsViewModel(AppointmentGuestsDTO appointmentGuests,bool demoOn)
        {
            AppointmentGuests=appointmentGuests;
            _appointmentService = new AppointmentService();
            SelectedAppointmentStatistics =
                _appointmentService.MakeAppointmentStatisticsDTO(AppointmentGuests.AppointmentId);
            if (demoOn)
                DemoIsOn(new CancellationToken());
        }
        
        private void ExitWindow()
        {
            this.CloseCurrentWindow();
        }
        private async Task DemoIsOn(CancellationToken ct)
        {


            ct.ThrowIfCancellationRequested();
            //MessageBox.Show("Demo has started!", "Demo message", MessageBoxButton.OK, MessageBoxImage.Information);
            await Task.Delay(2000, ct);
            ExitWindow();
        }
    }
}

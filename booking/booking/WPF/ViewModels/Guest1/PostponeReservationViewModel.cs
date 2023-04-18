using booking.application.usecases;
using booking.Commands;
using booking.Model;
using booking.WPF.ViewModels;
using booking.WPF.Views.Guest1;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace WPF.ViewModels.Guest1
{
    public class PostponeReservationViewModel : BaseViewModel
    {
        public ReservedDates NewDate { get; set; }

        private readonly ReservationRequestsService _reservationRequestsService;
        public ICommand SendPostponeRequestCommand => new RelayCommand(SendPostponeRequest);
        public ICommand CloseCommand => new RelayCommand(Close);

        public PostponeReservationViewModel(ReservedDates reservation)
        {
            this.NewDate = new ReservedDates(reservation);
            _reservationRequestsService = new ReservationRequestsService();
        }

        private void SendPostponeRequest()
        {
            _reservationRequestsService.SendPostponeRequest(NewDate);
            MessageBox.Show("Your request has been sent successfully!");
            Close();
        }
        private void Close()
        {
            this.CloseCurrentWindow();
        }
    }
}

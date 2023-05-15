using booking.application.usecases;
using booking.Commands;
using booking.Model;
using booking.WPF.ViewModels;
using booking.WPF.Views.Guest1;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace WPF.ViewModels.Guest1
{
    public class PostponeReservationViewModel : BaseViewModel, INotifyPropertyChanged
    {
        public ReservedDates NewDate { get; set; }

        private bool postponeButtonEnabled;
        public bool PostponeButtonEnabled
        {
            get { return postponeButtonEnabled; }
            set
            {
                if (postponeButtonEnabled != value)
                {
                    postponeButtonEnabled = value;
                    OnPropertyChanged();
                }
            }
        }

        private readonly ReservationRequestsService _reservationRequestsService;
        public ICommand SendPostponeRequestCommand => new RelayCommand(SendPostponeRequest);
        public ICommand CloseCommand => new RelayCommand(Close);
        public ICommand SelectedDateChangedCommand => new RelayCommand(SelectedDateChanged);

        public PostponeReservationViewModel(ReservedDates reservation)
        {
            this.NewDate = new ReservedDates(reservation);
            _reservationRequestsService = new ReservationRequestsService();
        }

        private void SendPostponeRequest()
        {
            if (PostponeButtonEnabled)
            {
                _reservationRequestsService.SendPostponeRequest(NewDate);
                MessageBox.Show("Your request has been sent successfully!");
                Close();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void SelectedDateChanged()
        {
            PostponeButtonEnabled = NewDate.IsValid;
        }

        private void Close()
        {
            this.CloseCurrentWindow();
        }
    }
}

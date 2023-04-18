﻿using application.UseCases;
using booking.application.UseCases;
using booking.application.UseCases.Guest2;
using booking.Commands;
using booking.Model;
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
        public ObservableCollection<Voucher> Vouchers { get; set; }
        public ObservableCollection<Appointment> CompletedTours { get; set; }
        public Appointment SelectedTour { get; set; }
        private User User { get; set; }

        private readonly AppointmentService _appointmentService;
        private readonly VoucherService _voucherService;
        public MyToursViewModel(User user) 
        {
            User = user;
            _appointmentService = new AppointmentService();
            _voucherService = new VoucherService();
            _voucherService.GenerateNewVouchersByGuest2(user);
            SelectedTour = new Appointment();
            CompletedTours = new ObservableCollection<Appointment>(_appointmentService.GetCompletedAppointmentByGuest2(User));
            Vouchers = new ObservableCollection<Voucher>(_voucherService.GetUsableVouchersByGuest2(user));
        }

        private void RateGuide()
        {
            if(SelectedTour.Start.Time == null) // ???
            {
                MessageBox.Show("You need to select a tour!", "Alert", MessageBoxButton.OK);
            }
            else 
            {
                var rateGuideWindow = new RateGuideView(SelectedTour, User);
                rateGuideWindow.Show();
            }
            
        }
    }
}
using booking.application.UseCases;
using booking.application.UseCases.Guest2;
using booking.Commands;
using booking.Model;
using booking.View.Owner;
using booking.WPF.Views.Guest2;
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

        public ObservableCollection<Appointment> CompletedTours { get; set; }
        public Appointment SelectedTour { get; set; }
        private User User { get; set; }

        private readonly AppointmentService _appointmentService;
        public MyToursViewModel(User user) 
        {
            User = user;
            _appointmentService = new AppointmentService();
            SelectedTour = new Appointment();
            CompletedTours = new ObservableCollection<Appointment>(_appointmentService.GetCompletedAppointmentByGuest2(User));
        }

        private void RateGuide()
        {
            if(SelectedTour.Start.Time == null) // ???
            {
                MessageBox.Show("You need to select a tour!", "Alert", MessageBoxButton.OK);
            }
            else 
            {
                var rateGuideWindow = new RateGuideView(SelectedTour);
                rateGuideWindow.Show();
            }
            
        }
    }
}

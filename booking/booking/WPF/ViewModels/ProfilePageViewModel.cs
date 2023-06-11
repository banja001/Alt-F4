using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using booking.Commands;
using booking.Model;
using booking.View;
using booking.WPF.ViewModels;
using booking.application.UseCases;
using booking.Repository;
using application.UseCases;
using Domain.RepositoryInterfaces;
using booking.Injector;

namespace WPF.ViewModels
{
    class ProfilePageViewModel:BaseViewModel,INotifyPropertyChanged
    {
        public User Guide { get; set; }
        public string Super { get; set; }
        public ICommand SignOutCommand => new RelayCommand(SignOut);
        public ICommand TooltipQuitCommand => new RelayCommand(QuitJobToolTip);
        public ICommand QuitJobCommand => new RelayCommand(QuitJob);
        private AppointmentService _appiontmentService;
        private readonly TourService _tourService;

        private readonly UserService _userService; 
        private bool quitTooltip;

        public bool QuitTooltip
        {
            get { return quitTooltip; }
            set
            {
                if (quitTooltip != value)
                {
                    quitTooltip = value;
                    OnPropertyChanged(nameof(QuitTooltip));
                }
            }
        }
        public ProfilePageViewModel(User guide)
        {
            Guide = guide;
            _appiontmentService = new AppointmentService();
            _userService=new UserService();
            _tourService =new TourService();
            Super = Guide.IsSuper( IsGuideSuper());

        }

        public void QuitJobToolTip()
        {
            QuitTooltip=!QuitTooltip;
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public void SignOut()
        {
            if (MessageBox.Show("Are you sure you want to sign out?", "Warning", MessageBoxButton.YesNo,MessageBoxImage.Warning) ==
                MessageBoxResult.Yes)
            {
                
                SignInForm signIn = new SignInForm();
                this.CloseCurrentWindow();
                signIn.Show();
            }
        }
        public bool IsGuideSuper()
        {
            List<Appointment>appointments= _appiontmentService.FindAllAppointmentsByGuide(Guide.Id);
            if (appointments.Count > 0)
            {
                string bestLanguage = _appiontmentService.GroupByLanguage(appointments);
                if (_appiontmentService.IsAbove20(appointments, bestLanguage))
                    _userService.UpdateSuperGuide(Guide.Id, true, bestLanguage);
                else
                    _userService.UpdateSuperGuide(Guide.Id, false, "");
                return _appiontmentService.IsAbove20(appointments, bestLanguage);
            }
           else
                return false;
        }
        public void QuitJob()
        {
            if (MessageBox.Show("Are you sure you want to quit job?", "Warning", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {

                _userService.QuitJob(Guide.Id);
                _tourService.GiveVouchersBecauseGuideQuitted(Guide.Id);
                MessageBox.Show("You quitted job!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
       
    }
}

using booking.Commands;
using booking.Model;
using booking.View;
using booking.View.Guest2;
using booking.View.Guest2.Windows;
using booking.WPF.Views.Guest2;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Utilities;
using WPF.ViewModels;

namespace booking.WPF.ViewModels
{
    public class MainGuest2ViewModel : BaseViewModel
    {

        private App app;
        private const string SRB = "sr-Latn-RS";
        private const string ENG = "en-US";
        public ICommand ExitButtonCommand => new RelayCommand(ExitWindow);
        public ICommand LogOutButtonCommand => new RelayCommand(LogOut);
        public ICommand NavigateWindowsCommand => new RelayCommandWithParams(NavigateWindows);
        public RelayCommand ChangeLanguageCommand => new RelayCommand(ChangeLanguage);
        public RelayCommand ChangeThemeCommand => new RelayCommand(ChangeTheme);
        public BaseViewModel UserControlInstance { get; set; }
        public User User { get; set; }
        public String HeaderMessage { get; set; }
        public MainGuest2ViewModel(User user) 
        {
            app = (App)Application.Current;
            app.ChangeLanguage(SRB);
            this.User = user;
            UserControlInstance = new HomeViewModel(User);
            OnPropertyChanged(nameof(UserControlInstance));
            HeaderMessage = " Welcome " + User.Username.ToString() + " ";
            OnPropertyChanged(nameof(HeaderMessage));
        }
        private void ChangeTheme()
        {
            if(ThemesController.CurrentTheme == ThemesController.ThemeTypes.Light)
                ThemesController.SetTheme(ThemesController.ThemeTypes.Dark);
            else
                ThemesController.SetTheme(ThemesController.ThemeTypes.Light);
        }
        private void ChangeLanguage()
        {
            if (app.getCultureInfo().Equals(SRB))
            {
                app.ChangeLanguage(ENG);
                return;
            }
            app.ChangeLanguage(SRB);
        }

        private void NavigateWindows(object parameter)
        {
            if (parameter != null)
            {
                switch (parameter.ToString())
                {
                    case "Home":
                        UserControlInstance = new HomeViewModel(User);
                        OnPropertyChanged(nameof(UserControlInstance));
                        HeaderMessage = " Welcome " + User.Username.ToString() + " ";
                        OnPropertyChanged(nameof(HeaderMessage));
                        break;
                    case "MyTours":
                        UserControlInstance = new MyToursViewModel(User);
                        OnPropertyChanged(nameof(UserControlInstance));
                        HeaderMessage = " My Tours ";
                        OnPropertyChanged(nameof(HeaderMessage));
                        break;
                    case "MyRequests":
                        UserControlInstance = new MyRequestsViewModel(User);
                        OnPropertyChanged(nameof(UserControlInstance));
                        HeaderMessage = " My Requests ";
                        OnPropertyChanged(nameof(HeaderMessage));
                        break;
                    case "Statistics":
                        UserControlInstance = new StatisticsViewModel(User);
                        OnPropertyChanged(nameof(UserControlInstance));
                        HeaderMessage = " Requests Statistics ";
                        OnPropertyChanged(nameof(HeaderMessage));
                        break;
                    default:
                        break;
                }
            }
        }

        private void ExitWindow()
        {
            this.CloseCurrentWindow();
        }
        private void LogOut()
        {
            var singInForm = new SignInForm();
            this.CloseCurrentWindow();
            singInForm.Show();
        }
    }
}

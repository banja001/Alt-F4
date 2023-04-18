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

namespace booking.WPF.ViewModels
{
    public class MainGuest2ViewModel : BaseViewModel
    {
        public ICommand ExitButtonCommand => new RelayCommand(ExitWindow);
        public ICommand LogOutButtonCommand => new RelayCommand(LogOut);
        public ICommand NavigateWindowsCommand => new RelayCommandWithParams(NavigateWindows);
        public FrameworkElement UserControlInstance { get; set; }
        public User User { get; set; }
        public MainGuest2ViewModel(User user) 
        {
            this.User = user;
            UserControlInstance = new MyToursView(User);
        }

        private void NavigateWindows(object parameter)
        {
            if (parameter != null)
            {
                switch (parameter.ToString())
                {
                    case "MyTours":
                        var tourWindow = new Guest2Overview(User);
                        tourWindow.Show();
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

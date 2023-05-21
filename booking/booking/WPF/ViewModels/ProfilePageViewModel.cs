using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;
using booking.Commands;
using booking.Model;
using booking.View;
using booking.WPF.ViewModels;

namespace WPF.ViewModels
{
    class ProfilePageViewModel:BaseViewModel
    {
        public User Guide { get; set; }
        public string Super { get; set; }
        public ICommand SignOutCommand => new RelayCommand(SignOut);

        public ProfilePageViewModel(User guide)
        {
            Guide = guide;
            Super = Guide.IsSuper();
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
    }
}

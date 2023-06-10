using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Input;
using booking.Commands;
using booking.Model;
using booking.View;
using booking.WPF.ViewModels;

namespace WPF.ViewModels
{
    class ProfilePageViewModel:BaseViewModel,INotifyPropertyChanged
    {
        public User Guide { get; set; }
        public string Super { get; set; }
        public ICommand SignOutCommand => new RelayCommand(SignOut);
        public ICommand TooltipQuitCommand => new RelayCommand(QuitJobToolTip);

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
            Super = Guide.IsSuper();
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
    }
}

using booking.Commands;
using booking.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace booking.WPF.ViewModels
{
    public class MainGuest2ViewModel : BaseViewModel
    {
        public ICommand ExitButtonCommand => new RelayCommand(ExitWindow);
        public ICommand LogOutButtonCommand => new RelayCommand(LogOut);
        public MainGuest2ViewModel() 
        { 
        }

        private void ExitWindow()
        {
            this.CloseCurrentWindow();
        }
        private void LogOut()
        {
            this.CloseCurrentWindow();
            var singInForm = new SignInForm();
            singInForm.Show();
        }
    }
}

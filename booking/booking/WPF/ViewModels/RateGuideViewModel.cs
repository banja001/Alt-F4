using booking.Commands;
using booking.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace booking.WPF.ViewModels
{
    public class RateGuideViewModel : BaseViewModel
    {
        public ICommand ExitWindowCommand => new RelayCommand(ExitWindow);
        public ICommand AddPhotoCommand => new RelayCommand(AddPhoto);
        public ICommand SubmitCommand => new RelayCommand(Submit);
        public Appointment SelectedTour { get; set; }
        public string ImageUrl { get; set; }
        public RateGuideViewModel(Appointment selectedTour) 
        {
            SelectedTour = selectedTour;
        }
        private void Submit()
        {

        }
        private void AddPhoto()
        {
            
        }
        private void ExitWindow()
        {
            this.CloseCurrentWindow();
        }
    }
}

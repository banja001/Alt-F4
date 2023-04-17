using booking.Commands;
using booking.Model;
using booking.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace WPF.ViewModels.Owner
{
    public class AddAccommodationImageViewModel: BaseViewModel
    {
        private List<string> accommodationImages;
        public string Url{get;set;}

        public ICommand CloseWindowCommand => new RelayCommand(CancelImageClick);
        public ICommand ConfirmImageCommand => new RelayCommand(ConfirmImageClick);
        public AddAccommodationImageViewModel(List<string> images)
        {
            accommodationImages = images;
        }

        public void ConfirmImageClick()
        {
            if (string.IsNullOrEmpty(Url))
            {
                MessageBox.Show("Please fill all of the textboxes");
                return;
            }
            accommodationImages.Add(Url);
            this.CloseCurrentWindow();

        }

        public void CancelImageClick()
        {
            this.CloseCurrentWindow();
        }


    }
}

using application.UseCases;
using booking.Commands;
using booking.Model;
using booking.WPF.ViewModels;
using Domain.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace WPF.ViewModels.Guest1
{
    public class ReviewViewModel : BaseViewModel
    {
        public ObservableCollection<Guest1RatingAccommodationDTO> Reviews { get; set; }

        public ICommand CloseCommand => new RelayCommand(Close);
        public ReviewViewModel(int guest1Id, ObservableCollection<Guest1RatingAccommodationDTO> Guest1RatingAccommodationDTOs)
        {
            Reviews = new ObservableCollection<Guest1RatingAccommodationDTO>(Guest1RatingAccommodationDTOs);
        }

        private void Close()
        {
            CloseCurrentWindow();
        }
    }
}

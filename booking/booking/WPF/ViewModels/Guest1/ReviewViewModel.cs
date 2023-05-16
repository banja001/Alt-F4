using application.UseCases;
using booking.Commands;
using booking.Model;
using booking.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace WPF.ViewModels.Guest1
{
    public class ReviewViewModel : BaseViewModel
    {
        public ObservableCollection<Guest1Rating> Reviews { get; set; }

        private readonly Guest1RatingsService _guest1RatingsService;

        public ICommand CloseCommand => new RelayCommand(Close);
        public ReviewViewModel(int guest1Id)
        {
            _guest1RatingsService = new Guest1RatingsService();

            Reviews = new ObservableCollection<Guest1Rating>(_guest1RatingsService.GetAllByGuest1Id(guest1Id));
        }

        private void Close()
        {
            CloseCurrentWindow();
        }
    }
}

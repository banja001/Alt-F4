using application.UseCases;
using booking.Commands;
using booking.Model;
using booking.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace WPF.ViewModels.Guest1
{
    public class ReserveAccommodationAAViewModel : BaseViewModel
    {
        public ObservableCollection<ReservedDates> FreeDates { get; set; }
        public ReservedDates SelectedDate { get; set; }

        private int accommodationId;
        private int userId;
        private int numOfGuests;

        private readonly ReservedDatesService _reservedDatesService;

        public ICommand CloseWindowCommand => new RelayCommand(CloseWindow);
        public ICommand ReserveAccommodationClickCommand => new RelayCommand(ReserveAccommodationClick);
        public ReserveAccommodationAAViewModel(ObservableCollection<ReservedDates> dates, int accommodationId, int userId, int numOfGuests)
        {
            _reservedDatesService = new ReservedDatesService();

            FreeDates = dates;
            this.accommodationId = accommodationId;
            this.userId = userId;
            this.numOfGuests = numOfGuests;
        }

        public void ReserveAccommodationClick()
        {
            if (SelectedDate != null)
            {
                SetSelectedDatesParameters();
                _reservedDatesService.Add(new ReservedDates(SelectedDate.StartDate, SelectedDate.EndDate, accommodationId, userId, false, SelectedDate.Id, numOfGuests, false));
                MessageBox.Show("Your reservation has been successfully made!");
                CloseWindow();
            }
            else
                MessageBox.Show("You have to pick a date before making a reservation!", "Warning");
        }

        private void SetSelectedDatesParameters()
        {
            SelectedDate.Id = _reservedDatesService.MakeId();
            SelectedDate.NumOfGuests = numOfGuests;
            SelectedDate.UserId = userId;
            SelectedDate.DateOfReserving = DateTime.Now;
        }

        private void CloseWindow()
        {
            this.CloseCurrentWindow();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using booking.Commands;
using System.Windows.Input;
using application.UseCases;
using booking.application.UseCases;
using booking.Model;
using booking.Repository;
using booking.WPF.ViewModels;
using Microsoft.Web.WebView2.Core.Raw;

namespace WPF.ViewModels
{
    public class TourCancellationViewModel:BaseViewModel
    {
        public ObservableCollection<Tour> UpcomingTours { get; set; }
        public Tour SelectedTour { get; set; }
        private readonly TourService _tourService;
        private readonly ReservationTourService _reservationService;
        public User Guide { get; set; }
        public TourCancellationViewModel(User guide)
        {
            _tourService = new TourService();
            _reservationService= new ReservationTourService();
            UpcomingTours = _tourService.FindUpcomingTours();
            SelectedTour = new Tour();
            Guide = guide;
        }
        public ICommand AbandonTourCommand => new RelayCommand(AbandonTour);
        public ICommand ExitCommand => new RelayCommand(ExitWindow);
        public void AbandonTour()
        {
            if (SelectedTour.Id > 0)
            {
                if (MessageBox.Show("Are you sure?", "Warning", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    _reservationService.GiveVouchers(SelectedTour,Guide);
                    _tourService.DeleteTour(SelectedTour);
                    ExitWindow();
                }
            }
            else
            {
                MessageBox.Show("Select tour!", "Warning", MessageBoxButton.OK);
            }
        }
        private void ExitWindow()
        {
            this.CloseCurrentWindow();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
using System.ComponentModel;

namespace WPF.ViewModels
{
    public class TourCancellationViewModel:BaseViewModel,INotifyPropertyChanged
    {
        public ObservableCollection<Tour> UpcomingTours { get; set; }
        public Tour SelectedTour { get; set; }
        private readonly TourService _tourService;
        private readonly ReservationTourService _reservationService;
        public User Guide { get; set; }

        private bool toursTooltip;

        public bool ToursTooltip
        {
            get { return toursTooltip; }
            set
            {
                if (toursTooltip != value)
                {
                    toursTooltip = value;
                    OnPropertyChanged(nameof(ToursTooltip));
                }
            }
        }
        public TourCancellationViewModel(User guide)
        {
            _tourService = new TourService();
            _reservationService= new ReservationTourService();
            UpcomingTours = _tourService.FindUpcomingTours();
            //SelectedTour = new Tour();
            Guide = guide;
        }
        public ICommand AbandonTourCommand => new RelayCommand(AbandonTour,CanAbandon);
        public ICommand ExitCommand => new RelayCommand(ExitWindow);

        public ICommand TooltipToursCommand => new RelayCommand(ToolTipTourShow);
        public void ToolTipTourShow()
        {
            ToursTooltip = !ToursTooltip;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public bool CanAbandon()
        {
            return SelectedTour != null;
        }
        public void AbandonTour()
        {
            if (SelectedTour.Id > 0)
            {
                if (MessageBox.Show("Are you sure?", "Warning", MessageBoxButton.YesNo,MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    _reservationService.GiveVouchers(SelectedTour,Guide);
                    _tourService.DeleteTour(SelectedTour);
                    UpcomingTours.Remove(SelectedTour);
                    //ExitWindow();
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

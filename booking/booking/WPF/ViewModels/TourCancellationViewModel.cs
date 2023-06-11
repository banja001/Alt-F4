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
using System.Threading;
using Domain.DTO;
using System.Threading.Tasks;
using WPF.Views.Guide;

namespace WPF.ViewModels
{
    public class TourCancellationViewModel:BaseViewModel,INotifyPropertyChanged
    {
        public RelayCommand AbandonTourCommand => new RelayCommand(AbandonTour, CanAbandon);
        public ObservableCollection<Tour> UpcomingTours { get; set; }
        private Tour selectedTour;
        public Tour SelectedTour
        {
            get { return selectedTour; }
            set
            {
                if (selectedTour != value)
                {
                    selectedTour = value;
                    OnPropertyChanged(nameof(SelectedTour));
                }
            }
        }
        private CancellationTokenSource cts = new CancellationTokenSource();

        private bool demoOn;
        public bool DemoOn
        {
            get { return demoOn; }
            set
            {
                demoOn = value;
                OnPropertyChanged(nameof(DemoOn));
            }
        }
        private string demoName;
        public string DemoName
        {
            get { return demoName; }
            set
            {
                if (demoName != value)
                {
                    demoName = value;
                    OnPropertyChanged(nameof(DemoName));
                }
            }
        }
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
            DemoName = "Demo";
        }
        
        public ICommand ExitCommand => new RelayCommand(ExitWindow);
        public ICommand DemoCommand => new RelayCommand(StartStopDemo);
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
                if (MessageBox.Show("Are you sure?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    if (!DemoOn)
                    { 
                        _reservationService.GiveVouchers(SelectedTour, Guide);
                        _tourService.DeleteTour(SelectedTour);
                        UpcomingTours.Remove(SelectedTour);
                    }   
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
        private void StartStopDemo()
        {
            if (DemoOn)
            {
                cts.Cancel();
                DemoOn = !DemoOn;
                DemoName = "Demo";
                MessageBox.Show("Demo has been stopped!", "Demo message", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                cts = new CancellationTokenSource();
                DemoOn = !DemoOn;
                DemoIsOn(cts.Token);
                DemoName = "Stop";
            }
        }
        private async Task DemoIsOn(CancellationToken ct)
        {

            if (DemoOn)
            {

                ct.ThrowIfCancellationRequested();
                StartDemoWindow sdw = new StartDemoWindow();
                sdw.Show();

                await Task.Delay(2000, ct);
                sdw.Close();
                SelectedTour = UpcomingTours[0];

                await Task.Delay(2000, ct);
                AbandonTour();
                
                await Task.Delay(1000, ct);
                FinishedDemoWindow fdw = new FinishedDemoWindow();
                fdw.Show();

                await Task.Delay(2000, ct);
                fdw.Close();
                DemoOn = !DemoOn;
                DemoName = "Demo";

            }
        }
    }
}

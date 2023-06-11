using booking.application.UseCases;
using booking.Commands;
using booking.WPF.ViewModels;
using Domain.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using WPF.Views.Guide;
using System.ComponentModel;

namespace WPF.ViewModels
{
    public class SelectDateForTourRequestViewModel:BaseViewModel,INotifyPropertyChanged
    {
        private DateTime displayDateEnd;
        private DateTime displayDateStart;
        private DateTime selectedDates;


        public DateTime DisplayDateEnd
        {
            get { return displayDateEnd; }
            set
            {
                displayDateEnd = value;
                OnPropertyChanged(nameof(DisplayDateEnd));
            }
        }

        public DateTime DisplayDateStart
        {
            get { return displayDateStart; }
            set
            {
                displayDateStart = value;
                OnPropertyChanged(nameof(DisplayDateStart));
            }
        }

        public DateTime SelectedDate
        {
            get { return selectedDates; }
            set
            {
                selectedDates = value;
                OnPropertyChanged(nameof(SelectedDate));
            }
        }
        public bool IsDemoOn { get; set; }
        public static bool Accept;
        public static DateTime selectedDate;
        public ICommand CreateTourCommand => new RelayCommand(CreateTour);
        private TourService _tourService;
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public SelectDateForTourRequestViewModel(SimpleAndComplexTourRequestsDTO selectedTour,bool isDemoOn) 
        {
            DisplayDateEnd = selectedTour.EndDate.Date;
            DisplayDateStart = selectedTour.StartDate.Date;
            SelectedDate= selectedTour.StartDate.Date;
            _tourService=new TourService();
            IsDemoOn = isDemoOn;
            if (IsDemoOn)
            {
                DemoIsOn(new CancellationToken());
            }
        }
        private void CreateTour()
        {
            if (_tourService.CheckAvailability(SelectedDate))
            {
                selectedDate = SelectedDate;
                Accept = true;
                CloseCurrentWindow();
            }
            else
                MessageBox.Show("You have other tours in that time!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        private async Task DemoIsOn(CancellationToken ct)
        {


                ct.ThrowIfCancellationRequested();
                //MessageBox.Show("Demo has started!", "Demo message", MessageBoxButton.OK, MessageBoxImage.Information);
                await Task.Delay(2000, ct);
                SelectedDate = DisplayDateStart.AddDays(2);
                await Task.Delay(2000, ct);
                
                CreateTour();
                await Task.Delay(2000, ct);
                FinishedDemoWindow fdw= new FinishedDemoWindow();
                fdw.Show();

            await Task.Delay(2000, ct);
            fdw.Close();
        }
    }
}

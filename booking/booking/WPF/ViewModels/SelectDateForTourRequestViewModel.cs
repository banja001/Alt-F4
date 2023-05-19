using booking.Commands;
using booking.WPF.ViewModels;
using Domain.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using WPF.Views.Guide;

namespace WPF.ViewModels
{
    public class SelectDateForTourRequestViewModel:BaseViewModel
    {
        public DateTime DisplayDateEnd{ get; set; }
        public DateTime DisplayDateStart{ get; set; }
        public DateTime SelectedDate { get; set; }
        public static bool Accept;
        public static DateTime selectedDate;
        public ICommand CreateTourCommand => new RelayCommand(CreateTour);
        public SelectDateForTourRequestViewModel(SimpleAndComplexTourRequestsDTO selectedTour) 
        {
            DisplayDateEnd = selectedTour.EndDate.Date;
            DisplayDateStart = selectedTour.StartDate.Date;
            SelectedDate= selectedTour.StartDate.Date;
        }
        private void CreateTour() 
        {
            selectedDate = SelectedDate;
            Accept = true;
            CloseCurrentWindow();
        }
    }
}

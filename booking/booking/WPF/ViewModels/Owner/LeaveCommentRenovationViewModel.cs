using booking.application.UseCases;
using booking.Commands;
using booking.Domain.Model;
using booking.Model;
using booking.WPF.ViewModels;
using Domain.DTO;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;
using WPF.Views.Owner;

namespace WPF.ViewModels.Owner
{
    public class LeaveCommentRenovationViewModel : BaseViewModel
    {
        public string Comment { get; set; }
        private RenovationDatesService repository;
        private DateIntervalDTO selectedInterval;
        private int accommodationId;
        public ICommand SaveCommentCommand => new RelayCommand(SaveCommentClick);
        public LeaveCommentRenovationViewModel(DateIntervalDTO s,int accid,RenovationDatesService ren)
        {
            selectedInterval = s;
            accommodationId=accid;
            repository = ren;//moze biti prob
        }


        private void SaveCommentClick()
        {
            RenovationDates ren = new RenovationDates(repository.MakeId(), selectedInterval.StartDate, selectedInterval.EndDate, accommodationId,Comment);
            repository.Add(ren);
            MessageBox.Show("Renovation scheduled!");
            MainWindow.w.Main.Navigate(MainWindow.w.OwnerWindow);
            
            this.CloseCurrentWindow();
        }
    }
}

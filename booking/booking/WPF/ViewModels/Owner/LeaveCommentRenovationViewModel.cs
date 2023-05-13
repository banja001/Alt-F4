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
using System.Windows.Input;

namespace WPF.ViewModels.Owner
{
    public class LeaveCommentRenovationViewModel : BaseViewModel
    {
        public string Comment { get; set; }
        private RenovationDatesService repository;
        private DateIntervalDTO selectedInterval;
        private int accommodationId;
        public ICommand SaveCommentCommand => new RelayCommand(SaveCommentClick);
        public LeaveCommentRenovationViewModel(DateIntervalDTO s,int accid)
        {
            selectedInterval = s;
            accommodationId=accid;
            repository = new RenovationDatesService();
        }


        private void SaveCommentClick()
        {
            RenovationDates ren = new RenovationDates(repository.MakeId(), selectedInterval.StartDate, selectedInterval.EndDate, accommodationId,Comment);
            repository.Add(ren);
            this.CloseCurrentWindow();
        }
    }
}

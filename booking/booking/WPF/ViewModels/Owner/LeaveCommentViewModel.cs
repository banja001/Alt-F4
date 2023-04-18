using booking.Commands;
using booking.Domain.Model;
using booking.WPF.ViewModels;
using booking.WPF.Views.Owner;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;

namespace WPF.ViewModels.Owner
{
    public class LeaveCommentViewModel:BaseViewModel
    {


        public string Comment { get; set; }

        public ReservationChangeViewModel resVM { get; set; }
        public ICommand SaveCommentCommand => new RelayCommand(SaveCommentClick);
        public LeaveCommentViewModel(ReservationChangeViewModel res)
        {
            this.resVM = res;
        }

        private void SaveCommentClick()
        {
            ReservationRequests request = resVM.reservationRequests.Find(s => resVM.SelectedItem.ReservationId == s.ReservationId);
            resVM.reservationRequestsRepository.UpdateDecline(request, Comment);
            resVM.requestsObservable.Remove(resVM.SelectedItem);
            this.CloseCurrentWindow();

        }
    }
}

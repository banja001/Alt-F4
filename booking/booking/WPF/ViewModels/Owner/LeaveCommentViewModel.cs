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
        private ReservationRequests reservationRequest;
        public ICommand SaveCommentCommand => new RelayCommand(SaveCommentClick);
        public LeaveCommentViewModel(ReservationChangeViewModel res,ReservationRequests r)
        {
            this.resVM = res;
            this.reservationRequest = r;
        }
        

        private void SaveCommentClick()
        {
            ReservationRequests request = resVM.reservationRequests.Find(s => resVM.SelectedItem.ReservationId == s.ReservationId);
            resVM.reservationRequestsService.UpdateDecline(request, Comment);
            resVM.AddGuest1Notification(reservationRequest);
            resVM.requestsObservable.Remove(resVM.SelectedItem);
            this.CloseCurrentWindow();

        }
    }
}

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

        public ReservationChangeWindow resWin;

        public ICommand SaveCommentCommand => new RelayCommand(SaveCommentClick);
        public LeaveCommentViewModel(ReservationChangeWindow resWin)
        {
            this.resWin = resWin;
        }

        private void SaveCommentClick()
        {
            ReservationRequests request = resWin.reservationRequests.Find(s => resWin.SelectedItem.ReservationId == s.ReservationId);
            resWin.reservationRequestsRepository.UpdateDecline(request, Comment);
            resWin.requestsObservable.Remove(resWin.SelectedItem);
            this.CloseCurrentWindow();

        }
    }
}

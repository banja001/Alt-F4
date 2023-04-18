using booking.Commands;
using booking.Domain.DTO;
using booking.Domain.Model;
using booking.Model;
using booking.Repositories;
using booking.View;
using booking.WPF.ViewModels;
using booking.WPF.Views.Owner;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace WPF.ViewModels.Owner
{
    public class ReservationChangeViewModel:BaseViewModel
    {
        public List<ReservationRequests> reservationRequests;
        
        public OwnerWindow ownerWindow;

        public ReservationRequestsRepository reservationRequestsRepository;
        public ObservableCollection<ReservationChangeDTO> requestsObservable { get; set; }
        public ReservationChangeDTO SelectedItem { get; set; }

        public ReservationChangeWindow reservationChangeWindow;
        public ICommand AllowCommand => new RelayCommand(AllowClick);

        public ICommand DeclineCommand => new RelayCommand(DeclineClick);
        public ReservationChangeViewModel(OwnerWindow ownerWindow,ReservationChangeWindow res)
        {
            this.ownerWindow = ownerWindow;
            this.reservationChangeWindow = res;
            reservationRequestsRepository = new ReservationRequestsRepository();
            requestsObservable = new ObservableCollection<ReservationChangeDTO>();
            UpdateObservable();
        }

        private void UpdateObservable()
        {
            reservationRequests = reservationRequestsRepository.GetPostpone();//Treba get false
            requestsObservable.Clear();
            foreach (ReservationRequests resRequest in reservationRequests)
            {
                ReservationChangeDTO resTemp = new ReservationChangeDTO();

                ReservedDates reservedDate = ownerWindow.reservedDates.Find(s => resRequest.ReservationId == s.Id);
                Accommodation reservedAccommodation = ownerWindow.accommodations.Find(s => reservedDate.AccommodationId == s.Id);

                if (reservedAccommodation.OwnerId != ownerWindow.OwnerId || resRequest.isCanceled != RequestStatus.Pending) continue;
                resTemp.RequestId = resRequest.Id;
                resTemp.ReservationId = resRequest.ReservationId;
                resTemp.AccommodationName = reservedAccommodation.Name;
                resTemp.OldStartDate = reservedDate.StartDate;
                resTemp.OldEndDate = reservedDate.EndDate;
                resTemp.NewStartDate = resRequest.NewStartDate;
                resTemp.NewEndDate = resRequest.NewEndDate;

                ReservedDates rr = ownerWindow.reservedDates.Find(s => !(s.EndDate < resRequest.NewStartDate) && !(s.StartDate > resRequest.NewEndDate) && (s.AccommodationId == reservedAccommodation.Id) && (resTemp.OldStartDate != s.StartDate) && (resTemp.OldEndDate != s.StartDate));
                //ovo testirat
                resTemp.IsTaken = rr == null ? Taken.No : Taken.Yes;

                requestsObservable.Add(resTemp);
            }

        }

        private void AllowClick()
        {
            if (SelectedItem == null) return;

            ReservedDates reservation = ownerWindow.reservedDates.Find(s => s.Id == SelectedItem.ReservationId);
            Accommodation accommodation = ownerWindow.accommodations.Find(s => s.Id == reservation.AccommodationId);

            List<ReservedDates> reservedDatesForDeletion = ownerWindow.reservedDates.FindAll(s => !(s.EndDate < SelectedItem.NewStartDate) && !(s.StartDate > SelectedItem.NewEndDate) && (s.AccommodationId == accommodation.Id)
            && (SelectedItem.ReservationId != s.Id));
            //(SelectedItem.OldStartDate != s.StartDate) && (SelectedItem.OldEndDate != s.StartDate)

            reservation.StartDate = SelectedItem.NewStartDate;
            reservation.EndDate = SelectedItem.NewEndDate;
            ownerWindow.reservedDatesRepository.Update(reservation);

            foreach (ReservedDates res in reservedDatesForDeletion)
            {

                ownerWindow.reservedDatesRepository.Remove(res);
                List<ReservationRequests> requestsToDelete = reservationRequests.FindAll(s => res.Id == s.ReservationId);
                foreach (var request in requestsToDelete)
                    reservationRequestsRepository.Remove(request);


            }


            reservationRequestsRepository.UpdateAllow(reservationRequests.Find(s => SelectedItem.RequestId == s.Id));


            UpdateObservable();

        }

        private void DeclineClick()
        {
            if (SelectedItem == null) return;

            LeaveCommentWindow win = new LeaveCommentWindow(this);
            win.ShowDialog();

        }
    }
}

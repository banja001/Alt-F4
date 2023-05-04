using booking.Domain.Model;
using booking.Injector;
using booking.Model;
using booking.Repositories;
using booking.Repository;
using Domain.Model;
using Domain.RepositoryInterfaces;
using Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace booking.application.usecases
{
    public class NotificationsService
    {
        private readonly IGuest1NotificationsRepository _guest1NotificationsRepository;
        private readonly IReservedDatesRepository _reservedDatesRepository;
        private readonly IReservationRequestsRepository _reservationRequestsRepository;

        public NotificationsService()
        {
            _guest1NotificationsRepository = Injector.Injector.CreateInstance<IGuest1NotificationsRepository>();
            _reservedDatesRepository = Injector.Injector.CreateInstance<IReservedDatesRepository>();
            _reservationRequestsRepository = Injector.Injector.CreateInstance <IReservationRequestsRepository>();
        }

        public void AddGuest1Notification(ReservationRequests reservationRequst)
        {
            int id = _guest1NotificationsRepository.MakeId();
            _guest1NotificationsRepository.Add(new Guest1Notifications(id, _reservedDatesRepository.GetById(reservationRequst.ReservationId).UserId, reservationRequst.Id));
        }

        public void NotifyGuest1(int userId)
        {
            List<Guest1Notifications> notifications = _guest1NotificationsRepository.GetAllByGuest1Id(userId);
            foreach (var notification in notifications)
            {
                ReservationRequests reservationRequest = _reservationRequestsRepository.GetById(notification.RequestId);

                MessageBox.Show("Your reservation for " + reservationRequest.NewStartDate.ToString("dd/MM/yyyy") + " - "
                    + reservationRequest.NewEndDate.ToString("dd/MM/yyyy")
                    + "has been " + reservationRequest.isCanceled.ToString());
            }

            if(notifications.Count != 0)
                _guest1NotificationsRepository.RemoveByGuest1Id(userId);
        }
    }
}

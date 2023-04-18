using booking.Domain.Model;
using booking.Injector;
using booking.Repository;
using Domain.Model;
using Domain.RepositoryInterfaces;
using Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace booking.application.usecases
{
    public class NotificationsService
    {
        private readonly IGuest1NotificationsRepository _guest1NotificationsRepository;
        private readonly IReservedDatesRepository _reservedDatesRepository;

        public NotificationsService()
        {
            _guest1NotificationsRepository = Injector.Injector.CreateInstance<IGuest1NotificationsRepository>();
            _reservedDatesRepository = Injector.Injector.CreateInstance<IReservedDatesRepository>();
        }

        public void AddGuest1Notification(ReservationRequests reservationRequst)
        {
            int id = _guest1NotificationsRepository.MakeId();
            _guest1NotificationsRepository.Add(new Guest1Notifications(id, _reservedDatesRepository.GetById(reservationRequst.ReservationId).UserId, reservationRequst.Id));
        }
    }
}

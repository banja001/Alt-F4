using booking.Domain.Model;
using booking.Model;
using Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace booking.application.usecases
{
    public class ReservationRequestsService
    {
        private readonly IReservationRequestsRepository _reservationRequestsRepository;

        public ReservationRequestsService()
        {
            _reservationRequestsRepository = Injector.Injector.CreateInstance<IReservationRequestsRepository>();
        }

        public void SendPostponeRequest(ReservedDates NewDate)
        {
            int requestId = _reservationRequestsRepository.MakeId();
            _reservationRequestsRepository.Add(new ReservationRequests(requestId, NewDate, "Pending"));
        }
    }
}

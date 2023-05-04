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

        public List<ReservationRequests> GetAll()
        {
            return _reservationRequestsRepository.GetAll();
        }

        public ReservationRequests GetById(int id)
        {
            return _reservationRequestsRepository.GetById(id);
        }

        public void Remove(ReservationRequests r)
        {
            _reservationRequestsRepository.Remove(r);
        }

        public void RemoveAllByReservationId(int id)
        {
            _reservationRequestsRepository.RemoveAllByReservationId(id);
        }

        public List<ReservationRequests> GetPending()
        {
            return _reservationRequestsRepository.GetPending();
        }
        public void UpdateDecline(ReservationRequests r, string comment)
        {
            _reservationRequestsRepository.UpdateDecline(r, comment);
        }
        public void UpdateAllow(ReservationRequests r)
        {
            _reservationRequestsRepository.UpdateAllow(r);
        }
        public void Add(ReservationRequests reservationRequest)
        {
            _reservationRequestsRepository.Add(reservationRequest);
        }
        public void Save()
        {
            _reservationRequestsRepository.Save();
        }
        public int MakeId()
        {
            return _reservationRequestsRepository.MakeId();
        }

        public void SendPostponeRequest(ReservedDates NewDate)
        {
            int requestId = _reservationRequestsRepository.MakeId();
            _reservationRequestsRepository.Add(new ReservationRequests(requestId, NewDate, "Pending"));
        }

    }
}

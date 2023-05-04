using booking.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.RepositoryInterfaces
{
    public interface IReservationRequestsRepository
    {
        public void Load();
        public List<ReservationRequests> GetAll();
        public ReservationRequests GetById(int id);
        public void Remove(ReservationRequests r);
        public void RemoveAllByReservationId(int id);
        public List<ReservationRequests> GetPending();
        public void UpdateDecline(ReservationRequests r, string comment);
        public void UpdateAllow(ReservationRequests r);
        public void Add(ReservationRequests reservationRequest);
        public void Save();
        public int MakeId();
    }
}

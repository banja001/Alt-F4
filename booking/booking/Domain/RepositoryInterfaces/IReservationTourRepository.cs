using booking.Model;
using booking.Serializer;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.RepositoryInterfaces
{
    public interface IReservationTourRepository
    {
        public List<ReservationTour> GetAll();
        public int GetNextIndex();
        public List<ReservationTour> GetAllByTourId(int tourId);
        public void Add(ReservationTour reservedTour);

        public int GetNumberOfGuestsForTourId(int tourId);
        public ReservationTour GetById(int id);
        public List<ReservationTour> GetByUserId(int id);

        public void Delete(ReservationTour reservation);
    }
}

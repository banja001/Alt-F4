using booking.DTO;
using booking.Model;
using booking.Serializer;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace booking.Repository
{
    public class ReservationTourRepository
    {
        private List<ReservationTour> reservations;
        private Serializer<ReservationTour> serializer;

        private readonly string fileName = "../../../Resources/Data/reservationTour.csv";

        public ReservationTourRepository()
        {
            serializer = new Serializer<ReservationTour>();
            reservations = serializer.FromCSV(fileName);
        }

        public List<ReservationTour> GetAll()
        {
            return reservations;
        }
        public int GetNextIndex()
        {
            return reservations.Count() + 1;
        }
        public List<ReservationTour>  GetAllByTourId(int tourId)
        {
            List<ReservationTour> foundReservations = new List<ReservationTour>();
            foreach (ReservationTour reservation in reservations)
            {
                if (reservation.Tour.Id == tourId)
                    foundReservations.Add(reservation);
            }
            return foundReservations;
        }
        public void Add(ReservationTour reservedTour)
        {
            reservations.Add(reservedTour);
            serializer.ToCSV(fileName, reservations);
        }

        public int GetNumberOfGuestsForTourId(int tourId)
        {
            List<ReservationTour> filteredReservations = this.GetAllByTourId(tourId);
            int numberOfGuests = 0;
            foreach (ReservationTour reservation in filteredReservations)
            {
                numberOfGuests += reservation.NumberOfGuests;
            }
            return numberOfGuests;
        }
        public ReservationTour GetById(int id)
        {
            foreach (ReservationTour reservation in reservations)
            {
                if (reservation.Id == id)
                    return reservation;
            }
            return null;
        }
    }
}

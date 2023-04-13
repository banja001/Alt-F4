using booking.Domain.Model;
using booking.Serializer;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace booking.Repositories
{
    public class ReservationRequestsRepository
    {
        private List<ReservationRequests> reservationRequests;
        private Serializer<ReservationRequests> serializer;
        private readonly string fileName = "../../../Resources/Data/reservationRequests.csv";

        public ReservationRequestsRepository()
        {
            serializer = new Serializer<ReservationRequests>();
            reservationRequests = serializer.FromCSV(fileName);
        }

        public List<ReservationRequests> GetAll()
        {
            return reservationRequests;
        }

        public void Add(ReservationRequests reservationRequest)
        {
            reservationRequests.Add(reservationRequest);
            Save();
        }

        public void Save()
        {
            serializer.ToCSV(fileName, reservationRequests);
        }

        public int MakeId()
        {
            return reservationRequests.Count == 0 ? 1 : reservationRequests[reservationRequests.Count - 1].Id + 1;
        }
    }
}

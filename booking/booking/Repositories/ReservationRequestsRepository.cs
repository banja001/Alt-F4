using booking.Domain.DTO;
using booking.Domain.Model;
using booking.Serializer;
using Domain.RepositoryInterfaces;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace booking.Repositories
{
    public class ReservationRequestsRepository: IReservationRequestsRepository
    {
        private List<ReservationRequests> reservationRequests;
        private Serializer<ReservationRequests> serializer;
        private readonly string fileName = "../../../Resources/Data/reservationRequests.csv";

        public ReservationRequestsRepository()
        {
            serializer = new Serializer<ReservationRequests>();
            Load();
        }
        public void Load()
        {
            reservationRequests = serializer.FromCSV(fileName);
        }

        public List<ReservationRequests> GetAll()
        {
            return serializer.FromCSV(fileName);
        }

        public ReservationRequests GetById(int id)
        {
            Load();
            return reservationRequests.Find(r => r.Id == id);
        }

        public void Remove(ReservationRequests r)
        {
            //Load();
            reservationRequests.Remove(r);
            Save();
        }
        public void RemoveAllByReservationId(int id)
        {
            Load();
            reservationRequests.RemoveAll(r => r.ReservationId == id);
            Save();
        }
        public List<ReservationRequests> GetPending()
        {
            
            List < ReservationRequests> list= new List<ReservationRequests>();
            foreach (ReservationRequests res in reservationRequests)
            {
                if (res.isCanceled == RequestStatus.Pending)
                {
                    list.Add(res);
                }
            }
            return list;


        }
        public void UpdateDecline(ReservationRequests r,string comment)
        {
            reservationRequests.Remove(r);
            r.isCanceled = RequestStatus.Canceled;
            r.Comment = comment;
            reservationRequests.Add(r);
            Save();
        }

        public void UpdateAllow(ReservationRequests r)
        {
            reservationRequests.Remove(r);
            r.isCanceled = RequestStatus.Postponed;
            reservationRequests.Add(r);
            Save();
        }

        public void Add(ReservationRequests reservationRequest)
        {
            Load();
            reservationRequests.Add(reservationRequest);
            Save();
        }

        public void Save()
        {
            serializer.ToCSV(fileName, reservationRequests);
        }

        public int MakeId()
        {
            return reservationRequests.Count == 0 ? 0 : reservationRequests.Max(d => d.Id) + 1;
        }

    }
}

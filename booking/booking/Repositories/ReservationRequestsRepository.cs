﻿using booking.Domain.DTO;
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
            return serializer.FromCSV(fileName);//nije lose ovako svuda raditi, zbog kasnijih update-ova dataGrida/listView-a i ostalog
        }

        public ReservationRequests GetById(int id)
        {
            return reservationRequests.Find(r => r.Id == id);
        }

        public void Remove(ReservationRequests r)
        {
            reservationRequests.Remove(r);
            Save();
        }
        public void RemoveAllByReservationId(int id)
        {
            reservationRequests.RemoveAll(r => r.ReservationId == id);
            Save();
        }
        public List<ReservationRequests> GetPostpone()
        {
            List < ReservationRequests> list= new List<ReservationRequests>();
            foreach (ReservationRequests res in reservationRequests)
            {
                if (res.RequestType == RequestType.Postpone)
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

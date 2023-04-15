﻿using booking.Model;
using booking.Serializer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace booking.Repository
{
    public class ReservedDatesRepository
    {
        private List<ReservedDates> reservedDates;
        private Serializer<ReservedDates> serializer;
        private readonly string fileName = "../../../Resources/Data/reservedDates.csv";

        public ReservedDatesRepository()
        {
            serializer = new Serializer<ReservedDates>();
            reservedDates = serializer.FromCSV(fileName);
        }
        public List<ReservedDates> GetAll()
        {
            return reservedDates;
        }

        public List<ReservedDates> GetAllByAccommodationId(int id)
        {
            return reservedDates.FindAll(d => d.AccommodationId == id);
        }

        public ReservedDates GetByID(int id)
        {
            return reservedDates.Where(d => d.Id == id).ToList()[0];
        }
        public void Update(ReservedDates reservedDate)
        {
            reservedDates.Remove(reservedDates.Find(s => reservedDate.Id == s.Id));
            reservedDates.Add(reservedDate);
            Save();
        }


        public int MakeId()//Treba izmijeniti da radi preko max ida
        {
            if (reservedDates.Count == 0) return 0;
            //return reservedDates[reservedDates.Count - 1].Id + 1;
            
            return reservedDates.Max(d => d.Id)+1;
        }

        public void Add(ReservedDates reservedDate)
        {
            reservedDates.Add(reservedDate);
            Save();
        }

        public void Remove(ReservedDates reservedDate)
        {
            reservedDates.Remove(reservedDate);
            Save();
        }

        public void Save()
        {
            serializer.ToCSV(fileName, reservedDates);
        }
        public void UpdateRating(int id)
        {
            ReservedDates r = reservedDates.Find(u => u.Id == id);
            reservedDates.Remove(r);
            r.RatedGuest++;
            reservedDates.Add(r);
            serializer.ToCSV(fileName, reservedDates);
        }
    }
}

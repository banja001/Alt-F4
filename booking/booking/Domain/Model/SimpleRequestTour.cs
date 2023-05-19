using booking.Model;
using booking.Serializer;
using System;
using System.Collections.Generic;
using System.Security.Policy;
using System.Text;

namespace Domain.Model
{
    public class SimpleRequestTour : ISerializable
    {
        public int Id { get; set; }
        public SimpleRequest SimpleRequest { get; set; }    
        public Tour Tour { get; set; }
        public User Guest2 { get; set; }

        public SimpleRequestTour() 
        {
            SimpleRequest = new SimpleRequest();
            Tour = new Tour();
            Guest2 = new User();
        }  
        public SimpleRequestTour(int id, int simpleRequestId, int tourId, int userId)
        {
            Id = id;
            SimpleRequest.Id = simpleRequestId;
            Tour.Id = tourId;
            Guest2.Id = userId;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            SimpleRequest.Id = Convert.ToInt32(values[1]);
            Tour.Id = Convert.ToInt32(values[2]);
            Guest2.Id= Convert.ToInt32(values[3]);

        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), SimpleRequest.Id.ToString(), Tour.Id.ToString(), Guest2.Id.ToString() };
            return csvValues;
        }
    }
}

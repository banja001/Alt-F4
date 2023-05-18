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
        public User User { get; set; }

        public SimpleRequestTour() 
        {
            SimpleRequest = new SimpleRequest();
            Tour = new Tour();
            User = new User();
        }  
        public SimpleRequestTour(int id, int simpleRequestId, int tourId, int userId)
        {
            Id = id;
            SimpleRequest.Id = simpleRequestId;
            Tour.Id = tourId;
            User.Id = userId;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            SimpleRequest.Id = Convert.ToInt32(values[1]);
            Tour.Id = Convert.ToInt32(values[2]);
            User.Id= Convert.ToInt32(values[3]);

        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), SimpleRequest.Id.ToString(), Tour.Id.ToString(), User.Id.ToString() };
            return csvValues;
        }
    }
}

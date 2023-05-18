using booking.Model;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DTO
{
    public class SimpleRequestTourDTO
    {
        public int Id { get; set; } 
        public Tour Tour { get; set; }
        public SimpleRequest SimpleRequest { get; set; }
        public string NotificationDescription { get; set; }

        public SimpleRequestTourDTO() 
        {
            Tour = new Tour();
            SimpleRequest = new SimpleRequest();    
        }
        public SimpleRequestTourDTO(int id, Tour tour, SimpleRequest simpleRequest)
        {
            Id = id;
            Tour = tour;
            SimpleRequest = simpleRequest;

        }
    }
}

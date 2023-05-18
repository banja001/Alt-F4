using booking.Model;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DTO
{
    public  class SimpleRequestDTO
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int NumberOfGuests { get; set; }
        public string Language { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string StatusUri { get; set; } 
        public Location Location { get; set; }

        public SimpleRequestDTO() 
        {
            StartDate = new DateTime();
            EndDate = new DateTime();  
            Location = new Location();  
        }
        public SimpleRequestDTO(int id, string description, int numberOfGuests, string language, DateTime startDate, DateTime endDate, string statusUri, Location location)
        {
            Id = id;
            Description = description;
            NumberOfGuests = numberOfGuests;
            Language = language;
            StartDate = startDate;
            EndDate = endDate;
            StatusUri = statusUri;
            Location = location;
        }
    }
}

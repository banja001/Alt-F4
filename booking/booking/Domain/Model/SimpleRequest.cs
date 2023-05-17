﻿using booking.Model;
using booking.Serializer;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Policy;
using System.Text;

namespace Domain.Model
{
    public enum SimpleRequestStatus
    {
        APPROVED,
        ON_HOLD,
        INVALID
    }

    public class SimpleRequest : ISerializable
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public Location Location { get; set; }  
        public string Language { get; set; }    
        public int NumberOfGuests { get; set; } 
        public DateRange DateRange { get; set; }
        public SimpleRequestStatus Status { get; set; }

        public SimpleRequest()
        {
            DateRange = new DateRange();
            Location = new Location();
        }
        public SimpleRequest( string description, int locationId, string language, int numberOfGuests,DateTime startDate, DateTime endDate, SimpleRequestStatus status)
        {
            Location = new Location();

            Id = -1;
            Description = description;
            Location.Id = locationId;
            Language = language;
            NumberOfGuests = numberOfGuests;
            DateRange = new DateRange(startDate, endDate);
            Status = status;
        }
        public SimpleRequest(int id, SimpleRequest simpleRequest)
        {
            Id = id;
            Description = simpleRequest.Description;
            Location = simpleRequest.Location;
            Language = simpleRequest.Language;
            NumberOfGuests = simpleRequest.NumberOfGuests;
            DateRange = simpleRequest.DateRange;
            Status = simpleRequest.Status;  
        }
        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(),
                                   Description,            
                                   Location.Id.ToString(),
                                   Language,
                                   NumberOfGuests.ToString(),
                                   DateRange.StartDate.ToString("d", CultureInfo.GetCultureInfo("es-ES")),
                                   DateRange.EndDate.ToString("d", CultureInfo.GetCultureInfo("es-ES")),
                                   Status.ToString()};
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Description = Convert.ToString(values[1]);  
            Location.Id = Convert.ToInt32(values[2]);   
            Language = Convert.ToString(values[3]);
            NumberOfGuests = Convert.ToInt32(values[4]);
            DateRange.StartDate = Convert.ToDateTime(values[5], CultureInfo.GetCultureInfo("es-ES"));
            DateRange.EndDate = Convert.ToDateTime(values[6], CultureInfo.GetCultureInfo("es-ES"));
            switch(values[7])
            {
                case "0":
                    Status = SimpleRequestStatus.APPROVED;
                    break;
                case "1":
                    Status = SimpleRequestStatus.ON_HOLD;
                    break;
                case "2":
                    Status = SimpleRequestStatus.INVALID;
                    break;
                default:
                    throw new ArgumentException("Simple request Status in CSV does not exist");
            }
        }
        public string GetStatusUri()
        {
            switch (Status)
            {
                case SimpleRequestStatus.APPROVED:
                    return "../../../Resources/Icons/approved.png";
                case SimpleRequestStatus.ON_HOLD:
                    return "../../../Resources/Icons/onhold.png";
                case SimpleRequestStatus.INVALID:
                    return "../../../Resources/Icons/invalid.png";
                default:
                    return "../../../Resources/Icons/unknown.png";
            }
        }
    }
}

using booking.Model;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Domain.DTO
{
    public class SimpleAndComplexTourRequestsDTO
    {
        public int SimpleRequestId { get; set; }
        public string Description { get; set; }
        public Location Location { get; set; }
        public string Language { get; set; }
        public int NumberOfGuests { get; set; }
        public DateAndTime StartDate { get; set; }
        public DateAndTime EndDate { get; set; }
        public bool IsPartOfComplex { get; set; }

        public SimpleAndComplexTourRequestsDTO(string description, Location location, string language, int numberOfGuests, DateAndTime startDate, DateAndTime endDate, bool isPartOfComplex)
        {
            Description = description;
            Location = location;
            Language = language;
            NumberOfGuests = numberOfGuests;
            StartDate= startDate;
            EndDate= endDate;
            IsPartOfComplex = isPartOfComplex;
        }
        public SimpleAndComplexTourRequestsDTO() { }
        public SimpleAndComplexTourRequestsDTO(SimpleRequest simpleRequest)
        {
            Location = new Location();
            StartDate = new DateAndTime();
            EndDate = new DateAndTime();
            Description = simpleRequest.Description;
            Location.Id = simpleRequest.Location.Id;
            Language = simpleRequest.Language;
            NumberOfGuests= simpleRequest.NumberOfGuests;
            StartDate.Date = Convert.ToDateTime(simpleRequest.DateRange.StartDate, CultureInfo.GetCultureInfo("es-ES"));
            EndDate.Date = Convert.ToDateTime(simpleRequest.DateRange.EndDate, CultureInfo.GetCultureInfo("es-ES"));
            IsPartOfComplex = simpleRequest.IsPartOfComplex;
            SimpleRequestId = simpleRequest.Id;
        }
    }
}

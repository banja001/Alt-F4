using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DTO
{
    public enum ReservationStatus { RESERVED, CANCELED}
    public class Guest1ReportDTO
    {
        public string AccommodationName { get; set; }
        public string Location { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ReservationStatus ReservationStatus { get; set; }
        public DateTime DateOfReserving { get; set; }

        public Guest1ReportDTO() { }

        public Guest1ReportDTO(string accommodationName, string location, DateTime startDate, DateTime endDate, ReservationStatus reservationStatus, DateTime dateOfReserving)
        {
            AccommodationName = accommodationName;
            Location = location;
            StartDate = startDate;
            EndDate = endDate;
            ReservationStatus = reservationStatus;
            DateOfReserving = dateOfReserving;
        }
    }
}

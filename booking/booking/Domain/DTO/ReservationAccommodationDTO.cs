using booking.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace booking.Domain.DTO
{
    public class ReservationAccommodationDTO
    {
        public int ReservationId { get; set; }
        public string AccommodationName { get; set; }
        public string Location { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public ReservationAccommodationDTO() { }

        public ReservationAccommodationDTO(Accommodation accommodation, Location location, ReservedDates reservation)
        {
            ReservationId = reservation.Id;
            AccommodationName = accommodation.Name;
            Location = location.State + "," + location.City;
            StartDate = reservation.StartDate;
            EndDate = reservation.EndDate;
        }
    }
}

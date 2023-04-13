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
        public string AccommodationName { get; set; }
        public string Location { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public ReservationAccommodationDTO() { }

        public ReservationAccommodationDTO(Accommodation accommodation, Location location, DateTime startDate, DateTime endDate)
        {
            AccommodationName = accommodation.Name;
            Location = location.State + "," + location.City;
            StartDate = startDate;
            EndDate = endDate;
        }
    }
}

using booking.DTO;
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
        public DateTime DateOfReserving { get; set; }

        public ReservationAccommodationDTO() { }

        public ReservationAccommodationDTO(Accommodation accommodation, Location location, ReservedDates reservation, DateTime dateOfReserving)
        {
            ReservationId = reservation.Id;
            AccommodationName = accommodation.Name;
            Location = location.State + "," + location.City;
            StartDate = reservation.StartDate;
            EndDate = reservation.EndDate;
            DateOfReserving = dateOfReserving;
        }

        public ReservationAccommodationDTO(int reservationId, string accommodationName, string location, DateTime startDate, DateTime endDate)
        {
            ReservationId = reservationId;
            AccommodationName = accommodationName;
            Location = location;
            StartDate = startDate;
            EndDate = endDate;
        }

        public ReservationAccommodationDTO(ReservationAccommodationDTO reservationAccommodationDTO)
        {
            this.ReservationId = reservationAccommodationDTO.ReservationId;
            this.AccommodationName = reservationAccommodationDTO.AccommodationName;
            this.Location = reservationAccommodationDTO.Location;
            this.StartDate = reservationAccommodationDTO.StartDate;
            this.EndDate = reservationAccommodationDTO.EndDate;
            this.DateOfReserving = reservationAccommodationDTO.DateOfReserving;
        }

        public override string ToString()
        {
            return AccommodationName + "|" + Location + "|" + StartDate.ToString("dd/MM/yyyy") + "-" + EndDate.ToString("dd/MM/yyyy");
        }
    }
}

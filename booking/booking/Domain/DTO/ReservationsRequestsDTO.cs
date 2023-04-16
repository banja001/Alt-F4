using booking.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace booking.Domain.DTO
{
    public class ReservationsRequestsDTO
    {
        public string AccommodationName { get; set; }
        public string Location { get; set; }
        public string Request { get; set; }
        public string Status { get; set; }
        public int RequestId { get; set; }

        public ReservationsRequestsDTO() { }

        public ReservationsRequestsDTO(Accommodation accommodation, Location location, string request, string status, int requestId)
        {
            AccommodationName = accommodation.Name;
            Location = location.State + "," + location.City;
            Request = request;
            Status = status;
            RequestId = requestId;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace booking.DTO
{
    public class Guest1RatingDTO
    {
        public int DateId { get;set; }
        public string GuestName { get; set; }
        public string AccommodationName { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public Guest1RatingDTO(int dateid,string guestName, string accommodationName, DateTime startDate, DateTime endDate)
        {
            DateId = dateid;
            GuestName = guestName;
            AccommodationName = accommodationName;
            StartDate = startDate;
            EndDate = endDate;
        }

        public Guest1RatingDTO()
        {
        }

    }
}

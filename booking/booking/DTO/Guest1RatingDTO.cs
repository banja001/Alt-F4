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

        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }

        public Guest1RatingDTO(int dateid,string guestName, string accommodationName, DateOnly startDate, DateOnly endDate)
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

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
        public int ReservationId { get; set; }
        public string AccommodationName { get; set; }

        public string StartDate { get; set; }
        public string EndDate { get; set; }

        public Guest1RatingDTO(int dateid,string guestName, string accommodationName, string startDate, string endDate,int res)
        {
            DateId = dateid;
            GuestName = guestName;
            ReservationId = res;
            AccommodationName = accommodationName;
            StartDate = startDate;
            EndDate = endDate;
        }

        public Guest1RatingDTO()
        {
        }

    }
}

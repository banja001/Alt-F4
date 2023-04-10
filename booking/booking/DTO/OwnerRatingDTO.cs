using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace booking.DTO
{
    public class OwnerRatingDTO
    {
        public string GuestName { get; set; }
        public int KindRating { get; set; }
        public int CleanRating { get; set; }

        public string Comment { get; set; }

        public int ReservationId { get; set; }

        public OwnerRatingDTO(string guestName, int kindRating, int cleanRating, string comment, int reservationId)
        {
            GuestName = guestName;
            KindRating = kindRating;
            CleanRating = cleanRating;
            Comment = comment;
            ReservationId = reservationId;
        }

        public OwnerRatingDTO()
        {
        }

    }
}
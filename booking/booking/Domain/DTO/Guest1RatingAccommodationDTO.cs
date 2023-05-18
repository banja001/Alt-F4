using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DTO
{
    public class Guest1RatingAccommodationDTO
    {
        public int ReservationId { get; set; }
        public int CleanRating { get; set; }
        public int RulesRating { get; set; }
        public string Comment { get; set; }
        public string AccommodtionName { get; set; }
        public string Location { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public Guest1RatingAccommodationDTO(int reservationId, int cleanRating, int rulesRating, string comment, string accommodtionName, string location, DateTime startDate, DateTime endDate)
        {
            ReservationId = reservationId;
            CleanRating = cleanRating;
            RulesRating = rulesRating;
            Comment = comment;
            AccommodtionName = accommodtionName;
            Location = location;
            StartDate = startDate;
            EndDate = endDate;
        }
    }
}

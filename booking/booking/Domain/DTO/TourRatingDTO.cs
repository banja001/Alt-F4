using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using booking.Domain.Model;
using booking.Model;

namespace booking.Domain.DTO
{
    public class TourRatingDTO
    {

        public string TourName { get; set; }
        public string CheckPoint { get; set; }
        public bool IsValid { get; set; }
        public GuideRating Rating { get; set; }
        public double AverageRating { get; set; }
        public string GuestName { get; set; }
        public int AppointmentId { get; set; }

        public TourRatingDTO()
        {
            IsValid = true;
        }

        public TourRatingDTO(string tourName, string checkPoint, bool isValid, GuideRating rating, double averageRating, int appointmentId,string guestName)
        {
            TourName = tourName;
            CheckPoint = checkPoint;
            IsValid = isValid;
            Rating = rating;
            AverageRating = averageRating;
            AppointmentId = appointmentId;
            GuestName = guestName;
        }

        public void CalculateAverageRating()
        {
            this.AverageRating = Math.Round(100*(this.Rating.LanguageKnowledge + this.Rating.TourEnjoyment + this.Rating.TourKnowledge) / 3.0)/100.0;
        }
    }
}

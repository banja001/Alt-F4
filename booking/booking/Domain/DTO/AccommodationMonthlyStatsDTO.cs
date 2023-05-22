using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DTO
{
    public class AccommodationMonthlyStatsDTO
    {
        public string Month { get; set; }
        public int NumberOfReservations { get; set; }
        public int CanceledReservations { get; set; }
        public int PostponedReservations { get; set; }

        public int RenovationSuggestions { get; set; }

        public AccommodationMonthlyStatsDTO(string y, int numberOfReservations, int canceledReservations, int postponedReservations,int renov)
        {
            Month = y;
            NumberOfReservations = numberOfReservations;
            CanceledReservations = canceledReservations;
            PostponedReservations = postponedReservations;
            RenovationSuggestions = renov;
        }
    }
}


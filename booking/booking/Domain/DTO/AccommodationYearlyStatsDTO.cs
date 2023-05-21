using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DTO
{
    public class AccommodationYearlyStatsDTO
    {
        public int year { get; set; }
        public int NumberOfReservations { get; set; }
        public int CanceledReservations { get; set; }
        public int PostponedReservations { get; set; }

        public int RenovationSuggestions { get; set; }

        public AccommodationYearlyStatsDTO(int y, int numberOfReservations, int canceledReservations, int postponedReservations,int ren)
        {
            year = y;
            NumberOfReservations = numberOfReservations;
            CanceledReservations = canceledReservations;
            PostponedReservations = postponedReservations;
            RenovationSuggestions = ren;
        }
    }
}

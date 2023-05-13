using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DTO
{
    public class RenovationAccommodationDTO
    {
        public int AccommodationId { get; set; }
        public string AccommodationName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public RenovationAccommodationDTO(int id,string s,DateTime startDate, DateTime endDate)
        {
            AccommodationId = id;
            AccommodationName = s;
            StartDate = startDate;
            EndDate = endDate;
        }
    }
}

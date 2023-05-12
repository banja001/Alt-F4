using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DTO
{
    public class DateIntervalDTO
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public DateIntervalDTO(DateTime startDate, DateTime endDate)
        {
            StartDate = startDate;
            EndDate = endDate;
        }
    }
}

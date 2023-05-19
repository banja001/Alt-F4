using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Model
{
    public class DateRange
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public DateRange() 
        { 
            StartDate = new DateTime();
            EndDate = new DateTime();
        }
        public DateRange(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
            {
                throw new ArgumentException("The start date must be earlier than the end date.");
            }

            StartDate = startDate;
            EndDate = endDate;
        }
        public bool Overlaps(DateRange dateRange)
        {
            return StartDate <= dateRange.EndDate && dateRange.StartDate <= EndDate;
        }
    }
}

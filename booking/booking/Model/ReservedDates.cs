using booking.Serializer;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;

namespace booking.Model
{
    public class ReservedDates : ISerializable
    {
        public int Id { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public int AccommodationId { get; set; }

        public ReservedDates() { }

        public ReservedDates(int id, DateOnly startDate, DateOnly endDate, int accommodationId)
        {
            Id = id;
            StartDate = startDate;
            EndDate = endDate;
            AccommodationId = accommodationId;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), StartDate.ToString("dd/MM/yyyy"), EndDate.ToString("dd/MM/yyyy"), AccommodationId.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            StartDate = DateOnly.ParseExact(values[1], "dd/MM/yyyy", CultureInfo.InvariantCulture);
            EndDate = DateOnly.ParseExact(values[2], "dd/MM/yyyy", CultureInfo.InvariantCulture);
            AccommodationId = Convert.ToInt32(values[3]);
        }
    }
}

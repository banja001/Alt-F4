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
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int AccommodationId { get; set; }

        public int UserId { get; set; }
        public int Rated { get; set; }

        public int NumOfGuests { get; set; }

        public ReservedDates() { }

        public ReservedDates(DateTime startDate, DateTime endDate, int accommodationId, int userId, int rated, int id = 0, int numOfGuests = 0)
        {
            Id = id;
            StartDate = startDate;
            EndDate = endDate;
            AccommodationId = accommodationId;
            UserId = userId;
            Rated = rated;
            NumOfGuests = numOfGuests;

        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), StartDate.ToString("dd/MM/yyyy"), EndDate.ToString("dd/MM/yyyy"), AccommodationId.ToString(), NumOfGuests.ToString(),UserId.ToString(),
            Rated.ToString()};

            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            StartDate = DateTime.ParseExact(values[1], "dd/MM/yyyy", CultureInfo.InvariantCulture);
            EndDate = DateTime.ParseExact(values[2], "dd/MM/yyyy", CultureInfo.InvariantCulture);
            AccommodationId = Convert.ToInt32(values[3]);
            NumOfGuests = Convert.ToInt32(values[4]);
            UserId = Convert.ToInt32(values[5]);
            Rated = Convert.ToInt32(values[6]);
        }
    }
}

using booking.Model;
using booking.Serializer;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Domain.Model
{
    public class RenovationDates : ISerializable
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int AccommodationId { get; set; }
        public string Description { get; set; }

        public RenovationDates(int id, DateTime startDate, DateTime endDate, int accommodationId,string des)
        {
            Id = id;
            StartDate = startDate;
            EndDate = endDate;
            AccommodationId = accommodationId;
            Description = des;
        }

        public RenovationDates()
        {
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), StartDate.ToString("dd/MM/yyyy"), EndDate.ToString("dd/MM/yyyy"), AccommodationId.ToString(),Description};

            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            StartDate = DateTime.ParseExact(values[1], "dd/MM/yyyy", CultureInfo.InvariantCulture);
            EndDate = DateTime.ParseExact(values[2], "dd/MM/yyyy", CultureInfo.InvariantCulture);
            AccommodationId = Convert.ToInt32(values[3]);
            Description = values[4];
        }

    }
}

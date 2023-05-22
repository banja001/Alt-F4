using booking.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml.Linq;

namespace booking.Model
{
    public class AccommodationImage : ISerializable
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public int AccommodationId { get; set; }

        public AccommodationImage() { }

        public AccommodationImage(int id, string url, int accomodation)
        {
            this.Id = id;
            this.Url = url;
            this.AccommodationId = accomodation;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(),Url, AccommodationId.ToString()};
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Url = values[1];
            AccommodationId = Convert.ToInt32(values[2]);

        }
    }
}

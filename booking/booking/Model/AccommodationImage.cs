using booking.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace booking.Model
{
    public class AccommodationImage : ISerializable
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public int AccomodationId { get; set; }

        public AccommodationImage() { }

        public AccommodationImage(int id, string url, int accomodation)
        {
            this.Id = id;
            this.Url = url;
            this.AccomodationId = accomodation;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(),Url, AccomodationId.ToString()};
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Url = values[1];
            AccomodationId = Convert.ToInt32(values[2]);

        }
    }
}

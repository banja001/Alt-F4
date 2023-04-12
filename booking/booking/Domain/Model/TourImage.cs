using booking.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace booking.Model
{
    public class TourImage : ISerializable
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public int TourId { get; set; }

        public TourImage() { }

        public TourImage(int id, string url, int Tour)
        {
            this.Id = id;
            this.Url = url;
            this.TourId = Tour;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Url, TourId.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Url = values[1];
            TourId = Convert.ToInt32(values[2]);

        }
    }
}

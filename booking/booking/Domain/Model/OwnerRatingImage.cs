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
    public class OwnerRatingImage : ISerializable
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public int RatedDatesId { get; set; }

        public OwnerRatingImage() { }

        public OwnerRatingImage(int id, string url, int reservedDateId)
        {
            this.Id = id;
            this.Url = url;
            this.RatedDatesId = reservedDateId;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Url, RatedDatesId.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Url = values[1];
            RatedDatesId = Convert.ToInt32(values[2]);

        }
    }
}


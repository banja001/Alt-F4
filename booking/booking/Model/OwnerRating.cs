using booking.Serializer;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace booking.Model
{
    public class OwnerRating : ISerializable
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public int CleanRating { get; set; }
        public int KindRating { get; set; }
        public string Comment { get; set; }

        public int ReservationId { get; set; }

        public OwnerRating() { }
        public OwnerRating(int id, int guestid, int cleanRating, int rulesrating, string comment)
        {
            Id = id;
            OwnerId = guestid;
            CleanRating = cleanRating;
            KindRating = rulesrating;
            Comment = comment;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), OwnerId.ToString(), CleanRating.ToString(), KindRating.ToString(), Comment ,ReservationId.ToString()};
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            OwnerId = Convert.ToInt32(values[1]);
            CleanRating = Convert.ToInt32(values[2]);
            KindRating = Convert.ToInt32(values[3]);
            Comment = values[4];
            ReservationId = Convert.ToInt32(values[5]);
        }
    }
}
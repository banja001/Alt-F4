using booking.Serializer;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace booking.Model
{
    public class Guest1Rating : ISerializable
    {
        public int Id { get; set; }
        public int GuestId { get; set; }
        public int CleanRating { get; set; }
        public int RulesRating { get; set; }
        public string Comment { get; set; }

        public Guest1Rating() { }
        public Guest1Rating(int id,int guestid, int cleanRating, int rulesrating, string comment)
        {
            Id = id;
            GuestId = guestid;
            CleanRating = cleanRating;
            RulesRating = rulesrating;
            Comment = comment;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), GuestId.ToString(),CleanRating.ToString(),RulesRating.ToString(),Comment};
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            GuestId = Convert.ToInt32(values[1]);
            CleanRating = Convert.ToInt32(values[2]);
            RulesRating = Convert.ToInt32(values[3]);
            Comment = values[4];

        }
    }
}

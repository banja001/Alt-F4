using booking.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace booking.Domain.Model
{
    public class GuideRatingImage : ISerializable
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public int GuideRatingId { get; set; }

        public GuideRatingImage() 
        {
        }
        public GuideRatingImage(int id, string url, int guideRatingId)
        {
            Id = id;
            this.Url = url;
            GuideRatingId = guideRatingId;
        }
        public GuideRatingImage(string url)
        {
            this.Url = url;
        }

        public string[] ToCSV()
        {
            string[] cssValues =
            {
                Id.ToString(),
                Url,
                GuideRatingId.ToString()
            };
            return cssValues;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            Url = values[1].ToString();
            GuideRatingId = int.Parse(values[2]);
        }
    }
}

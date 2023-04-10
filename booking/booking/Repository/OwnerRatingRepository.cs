using booking.Model;
using booking.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace booking.Repository
{
    public class OwnerRatingRepository
    {
        private List<OwnerRating> OwnerRatings;
        private Serializer<OwnerRating> Serializer;

        public readonly string fileName = "../../../Resources/Data/ownerratings.csv";
        public OwnerRatingRepository()
        {

            Serializer = new Serializer<OwnerRating>();
            OwnerRatings = Serializer.FromCSV(fileName);
        }

        public List<OwnerRating> GetAll()
        {
            return OwnerRatings;
        }

        public void AddRating(OwnerRating acci)
        {

            OwnerRatings.Add(acci);
            Serializer.ToCSV(fileName, OwnerRatings);

        }
    }
}

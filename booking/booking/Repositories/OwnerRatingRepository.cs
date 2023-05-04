using booking.Model;
using booking.Serializer;
using Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace booking.Repository
{
    public class OwnerRatingRepository : IOwnerRatingRepository
    {
        private List<OwnerRating> OwnerRatings;
        private Serializer<OwnerRating> Serializer;

        public readonly string fileName = "../../../Resources/Data/ownerratings.csv";
        public OwnerRatingRepository()
        {

            Serializer = new Serializer<OwnerRating>();
            Load();
        }

        public void Load()
        {
            OwnerRatings = Serializer.FromCSV(fileName);
        }

        public List<OwnerRating> GetAll()
        {
            return Serializer.FromCSV(fileName);
        }

        public void Add(OwnerRating acci)
        {
            Load();
            OwnerRatings.Add(acci);
            Save();
        }

        public void Save()
        {
            Serializer.ToCSV(fileName, OwnerRatings);
        }

        public int MakeId()
        {
            Load();
            return OwnerRatings.Count == 0 ? 1 : OwnerRatings.Max(d => d.Id) + 1;
        }
    }
}

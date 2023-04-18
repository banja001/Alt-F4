using booking.DTO;
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
    public class OwnerRatingImageRepository : IOwnerRatingImageRepository
    {
        private List<OwnerRatingImage> OwnerRatingImages;
        private Serializer<OwnerRatingImage> Serializer;


        public readonly string fileName = "../../../Resources/Data/ownerratingimages.csv";
        public OwnerRatingImageRepository()
        {
            Serializer = new Serializer<OwnerRatingImage>();
            Load();
        }

        public void Load()
        {
            OwnerRatingImages = Serializer.FromCSV(fileName);
        }

        public List<OwnerRatingImage> GetAll()
        {
            return Serializer.FromCSV(fileName);
        }

        public void Add(OwnerRatingImage acci)
        {
            Load();
            OwnerRatingImages.Add(acci);
            Save();
        }

        public void Save()
        {
            Serializer.ToCSV(fileName, OwnerRatingImages);
        }
        
        public List<OwnerRatingImage> GetByReservedDatesId(int ReservedDatesId)
        {
            List<OwnerRatingImage> ownerRatingImages = new List<OwnerRatingImage>();

            foreach (OwnerRatingImage image in OwnerRatingImages)
            {
                if (image.RatedDatesId == ReservedDatesId)
                {
                    ownerRatingImages.Add(image);
                }
            }

            return ownerRatingImages;
        }
        
        public int MakeId()
        {
            Load();
            return OwnerRatingImages.Count == 0 ? 1 : OwnerRatingImages.Max(i => i.Id) + 1;
        }
    }
}

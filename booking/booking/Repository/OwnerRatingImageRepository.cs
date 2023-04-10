using booking.DTO;
using booking.Model;
using booking.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace booking.Repository
{
    public class OwnerRatingImageRepository
    {
        private List<OwnerRatingImage> OwnerRatingImages;
        private Serializer<OwnerRatingImage> Serializer;


        public readonly string fileName = "../../../Resources/Data/ownerratingimages.csv";
        public OwnerRatingImageRepository()
        {


            Serializer = new Serializer<OwnerRatingImage>();
            OwnerRatingImages = Serializer.FromCSV(fileName);
        }

        public List<OwnerRatingImage> GetAll()
        {
            return OwnerRatingImages;
        }

        public void AddOwnerRatingImage(OwnerRatingImage acci)
        {

            OwnerRatingImages.Add(acci);
            Serializer.ToCSV(fileName, OwnerRatingImages);

        }
        
        public List<OwnerRatingImage> Get(int ReservedDatesId)
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
        
    }
}

using booking.Model;
using booking.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace booking.Manager
{
    public class AccommodationImageRepository
    {
        private List<AccommodationImage> AccommodationImages;
        private Serializer<AccommodationImage> Serializer;

        public readonly string fileName = "../../../Resources/Data/accommodationImage.csv";
        public AccommodationImageRepository()
        {
            AccommodationImages = new List<AccommodationImage>();
            Serializer = new Serializer<AccommodationImage>();
            AccommodationImages = Serializer.FromCSV(fileName);
        }

        public List<AccommodationImage> GetAllImages()
        {
            return AccommodationImages;
        }

        public void AddAccommodationImage(AccommodationImage acci)
        {

            AccommodationImages.Add(acci);
            Serializer.ToCSV(fileName, AccommodationImages);

        }

    }
}

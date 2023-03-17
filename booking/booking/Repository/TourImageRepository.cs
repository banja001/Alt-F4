using booking.Model;
using booking.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace booking.Repository
{
    public class TourImageRepository
    {
        private List<TourImage> tourImages;
        private Serializer<TourImage> serializer;

        private readonly string fileName = "../../../Resources/Data/tourimage.csv";

        public TourImageRepository()
        {
            serializer = new Serializer<TourImage>();
            tourImages = serializer.FromCSV(fileName);
        }
        public List<TourImage> findAll()
        {
            return tourImages;
        }
        public void Add(TourImage tourImage)
        { 
            tourImages.Add(tourImage);
            serializer.ToCSV(fileName,tourImages);
        }
        public void AddRange(List<TourImage> TourImagesFromList)
        {
            tourImages.AddRange(TourImagesFromList);
            serializer.ToCSV(fileName, tourImages);
        }
        public int MakeID()
        {
            return tourImages[tourImages.Count - 1].Id + 1;
        }
    }
}

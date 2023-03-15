using booking.Model;
using booking.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}

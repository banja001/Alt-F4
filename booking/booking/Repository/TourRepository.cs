using booking.Model;
using booking.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace booking.Repository
{
    public class TourRepository
    {
        private List<Tour> tours;
        private Serializer<Tour> serializer;

        private readonly string fileName = "../../../Resources/Data/tour.csv";

        public TourRepository()
        {
            serializer = new Serializer<Tour>();
            tours = serializer.FromCSV(fileName);
        }
        public List<Tour> FindAll()
        {
            return tours;
        }
        public void Add(Tour tour)
        {
            tours.Add(tour);
            serializer.ToCSV(fileName, tours);
        }

        public int MakeID()
        {
            return tours[tours.Count - 1].Id + 1;
        }
    }
}

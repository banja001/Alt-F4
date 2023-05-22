using booking.Model;
using booking.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.RepositoryInterfaces;

namespace booking.Repository
{
    public class TourRepository:ITourRepository
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
            tours = serializer.FromCSV(fileName);
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

        public void Delete(Tour tour)
        {
            tours.Remove(tours.Find(t => t.Id == tour.Id));
            serializer.ToCSV(fileName, tours);
        }

        public Tour FindById(int id)
        {
            tours = serializer.FromCSV(fileName);
            return tours.Find(tour => tour.Id == id);
        }
    }
}

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
    public class LocationRepository: ILocationRepository
    {

        private List<Location> Locations;
        private Serializer<Location> Serializer;


        public readonly string fileName = "../../../Resources/Data/location.csv";
        public LocationRepository()
        {

            Serializer = new Serializer<Location>();
            Locations = Serializer.FromCSV(fileName);
        }

        public List<Location> GetAll()
        {
            Locations = Serializer.FromCSV(fileName).ToList();
            return Locations;
        }

        public Location GetById(int id)
        {
            Locations = Serializer.FromCSV(fileName).ToList();
            return Locations.Where(l => l.Id == id).ToList()[0];
        }

        public void Add(Location loc)
        {

            Locations.Add(loc);
            Serializer.ToCSV(fileName, Locations);

        }

        public int MakeID()
        {
            return Locations[Locations.Count - 1].Id + 1;
        }
    }
}

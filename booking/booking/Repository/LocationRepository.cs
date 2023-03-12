using booking.Model;
using booking.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace booking.Repository
{
    public class LocationRepository
    {

        private List<Location> Locations;
        private Serializer<Location> Serializer;

        public readonly string fileName = "../../../Resources/Data/location.csv";
        public LocationRepository()
        {
            
            Serializer = new Serializer<Location>();
            Locations = Serializer.FromCSV(fileName);
        }

        public List<Location> GetAllLocations()
        {
            return Locations;
        }

        public void AddLocation(Location loc)
        {

            Locations.Add(loc);
            Serializer.ToCSV(fileName, Locations);

        }
    }
}

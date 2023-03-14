using booking.Model;
using booking.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace booking.Repository
{
    public class AccommodationRepository
    {
        private List<Accommodation> accommodations;
        private Serializer<Accommodation> serializer;

        public readonly string fileName = "../../../Resources/Data/accommodation.csv";
        public AccommodationRepository()
        {
            //accommodations = new List<Accommodation>();
            serializer = new Serializer<Accommodation>();
            accommodations = serializer.FromCSV(fileName);
        }

        public List<Accommodation> FindAll()
        {
            return accommodations;
        }

        public void AddAccommodation(Accommodation acc)
        {

            accommodations.Add(acc);
            serializer.ToCSV(fileName, accommodations);

        }



    }
}

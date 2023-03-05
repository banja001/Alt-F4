using booking.Model;
using booking.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace booking.Manager
{
    public class AccommodationManager
    {
        private List<Accommodation> accommodations;
        private Serializer<Accommodation> serializer;

        public readonly string fileName = "../../../Resources/Data/accommodation.csv";
        public AccommodationManager() 
        {
            accommodations = new List<Accommodation>();
            serializer = new Serializer<Accommodation>();
            accommodations= serializer.FromCSV(fileName);
        }

        public void GetAllAccommodations()
        {
            accommodations = serializer.FromCSV(fileName);
        }

        public void AddAccommodation(Accommodation acc)
        {
            
            accommodations.Add(acc);
            serializer.ToCSV(fileName,accommodations);

        }


        
    }
}

using booking.Model;
using booking.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace booking.Repository
{
    internal class AccommodationRepository
    {
        private const string filePath = "../../../Resources/Data/accommodation.csv";

        private readonly Serializer<Accommodation> _serializer;

        private List<Accommodation> _accomodations;

        public AccommodationRepository()
        {
            _serializer = new Serializer<Accommodation>();
            _accomodations = _serializer.FromCSV(filePath);
        }

        public List<Accommodation> GetAll()
        {
            return _serializer.FromCSV(filePath);
        }
    }
}

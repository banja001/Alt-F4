using booking.Model;
using booking.Serializer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace booking.Repository
{
    public class ReservedDatesRepository
    {
        private List<ReservedDates> reservedDates;
        private Serializer<ReservedDates> serializer;

        private readonly string fileName = "../../../Resources/Data/reservedDates.csv";

        public ReservedDatesRepository()
        {
            serializer = new Serializer<ReservedDates>();
            reservedDates = serializer.FromCSV(fileName);
        }

        public List<ReservedDates> GetAll()
        {
            return reservedDates;
        }

        public List<ReservedDates> GetAllByAccommodationId(int id)
        {
            return reservedDates.FindAll(d => d.AccommodationId == id);
        }

        public int MakeId()
        {
            return reservedDates[reservedDates.Count - 1].Id + 1;
        }
    }
}

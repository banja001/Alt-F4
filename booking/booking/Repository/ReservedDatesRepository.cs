using booking.Model;
using booking.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace booking.Repository
{
    public class ReservedDatesRepository
    {
        private List<ReservedDates> reservedDates;
        private Serializer<ReservedDates> serializer;

        public readonly string fileName = "../../../Resources/Data/reservedDates.csv";

        public ReservedDatesRepository()
        {
            serializer = new Serializer<ReservedDates>();
            reservedDates = serializer.FromCSV(fileName);
        }

        public List<ReservedDates> findAll()
        {
            return reservedDates;
        }
    }
}

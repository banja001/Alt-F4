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


        public readonly string fileName = "../../../Resources/Data/reservedDates.csv";


        


        public ReservedDatesRepository()
        {
            serializer = new Serializer<ReservedDates>();
            reservedDates = serializer.FromCSV(fileName);
        }
        public List<ReservedDates> GetAll()
        {
            return reservedDates;
        }

        public void UpdateRating(int id)
        {
            ReservedDates r=reservedDates.Find(u => u.Id == id);
            reservedDates.Remove(r);
            r.Rated = 1;
            reservedDates.Add(r);
            serializer.ToCSV(fileName, reservedDates);
        }

        

        public List<ReservedDates> GetAllByAccommodationId(int id)
        {
            return reservedDates.FindAll(d => d.AccommodationId == id);
        }

        public int MakeId()
        {
            return reservedDates[reservedDates.Count - 1].Id + 1;
        }

        public void Add(ReservedDates reservedDate)
        {
            reservedDates.Add(reservedDate);
            Save();
        }

        public void Save()
        {
            serializer.ToCSV(fileName, reservedDates);
        }
    }
}

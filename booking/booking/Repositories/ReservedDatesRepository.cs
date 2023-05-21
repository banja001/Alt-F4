using booking.Domain.Model;
using booking.Model;
using booking.Serializer;
using Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace booking.Repository
{
    public class ReservedDatesRepository: IReservedDatesRepository
    {
        private List<ReservedDates> reservedDates;
        private Serializer<ReservedDates> serializer;
        private List<ReservedDates> canceledDates;
        private readonly string fileName = "../../../Resources/Data/reservedDates.csv";
        private readonly string fileNameCancel = "../../../Resources/Data/canceledDates.csv";
        public ReservedDatesRepository()
        {
            serializer = new Serializer<ReservedDates>();
            canceledDates = serializer.FromCSV(fileNameCancel);
            Load();
        }
        public void Load()
        {
            canceledDates = serializer.FromCSV(fileNameCancel);
            reservedDates = serializer.FromCSV(fileName);
        }
        public List<ReservedDates> GetAll()
        {
            Load();
            return reservedDates;
        }

        public List<ReservedDates> GetAllCanceled()
        {
            return canceledDates;
        }

        public List<ReservedDates> GetAllByAccommodationId(int id)
        {
            Load();
            return reservedDates.FindAll(d => d.AccommodationId == id);
        }

        public ReservedDates GetById(int id)
        {
            Load();
            return reservedDates.Where(d => d.Id == id).ToList()[0];
        }
        public List<ReservedDates> GetByGuestId(int guestId)
        {
            return reservedDates.Where(d => d.UserId == guestId).ToList();
        }
        public void Update(ReservedDates reservedDate)
        {
            reservedDates.Remove(reservedDates.Find(s => reservedDate.Id == s.Id));
            reservedDates.Add(reservedDate);
            Save();
            /*Load();
            reservedDates[reservedDates.FindIndex(s => reservedDate.Id == s.Id)] = reservedDate; //bolje ovako da ne bi ponovo upisivao na kraj fajla
            */
        }


        public int MakeId()
        {
            Load();
            return reservedDates.Count == 0 ? 0 : reservedDates.Max(d => d.Id) + 1;
        }

        public void Add(ReservedDates reservedDate)
        {
            Load();
            reservedDates.Add(reservedDate);
            Save();
        }
        public void AddCanceled(ReservedDates reservedDate)
        {

            canceledDates = serializer.FromCSV(fileNameCancel);
            canceledDates.Add(reservedDate);
            serializer.ToCSV(fileNameCancel, canceledDates);
        }
        public void Remove(ReservedDates reservedDate)
        {
            //Load();
            reservedDates.Remove(reservedDate);
            Save();
        }

        public void Save()
        {
            serializer.ToCSV(fileName, reservedDates);
        }

        public void Delete(ReservedDates reservedDate)
        {
            Load();
            reservedDates.Remove(reservedDates.Find(d => d.Id == reservedDate.Id));
            Save();
        }

        public void UpdateRating(int id)
        {
            //Load();
            ReservedDates r = reservedDates.Find(u => u.Id == id);
            reservedDates.Remove(r);
            r.RatedByOwner = true;
            reservedDates.Add(r);
            serializer.ToCSV(fileName, reservedDates);
        }
    }
}

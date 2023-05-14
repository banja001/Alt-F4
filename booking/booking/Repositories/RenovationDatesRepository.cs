using booking.Model;
using booking.Serializer;
using Domain.Model;
using Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repositories
{
    public class RenovationDatesRepository : IRenovationDatesRepository
    {
        private List<RenovationDates> renovationDates;
        private Serializer<RenovationDates> serializer;
        private readonly string fileName = "../../../Resources/Data/renovationDates.csv";

        public RenovationDatesRepository()
        {
            serializer = new Serializer<RenovationDates>();
        }

        public void Load()
        {
            renovationDates = serializer.FromCSV(fileName);
        }
        public List<RenovationDates> GetAll()
        {
            Load();
            return renovationDates;
        }
        public void Update(RenovationDates renovationDate)
        {
            renovationDates.Remove(renovationDates.Find(s => renovationDate.Id == s.Id));
            renovationDates.Add(renovationDate);
            Save();
        }
        public int MakeId()
        {
            Load();
            return renovationDates.Count == 0 ? 0 : renovationDates.Max(d => d.Id) + 1;
        }

        public void Add(RenovationDates renovationDate)
        {
            Load();
            renovationDates.Add(renovationDate);
            Save();
        }

        public void Remove(RenovationDates renovationDate)
        {
            //Load();
            renovationDates.Remove(renovationDate);
            Save();
        }

        public void Save()
        {
            serializer.ToCSV(fileName, renovationDates);
        }

    }
}

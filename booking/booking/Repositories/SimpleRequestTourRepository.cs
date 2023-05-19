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
    public class SimpleRequestTourRepository : ISimpleRequestTourRepository
    {
        private List<SimpleRequestTour> _simpleRequestTours;
        private Serializer<SimpleRequestTour> _serializer;

        private readonly string fileName = "../../../Resources/Data/simplerequesttour.csv";
        public SimpleRequestTourRepository()
        {
            _serializer = new Serializer<SimpleRequestTour>();
            Load();
        }

        public void Load()
        {
            _simpleRequestTours = _serializer.FromCSV(fileName);
        }

        public List<SimpleRequestTour> GetAll()
        {
            Load();
            return _simpleRequestTours;
        }

        public SimpleRequestTour GetById(int id)
        {
            Load();
            return _simpleRequestTours.Find(s => s.Id == id);
        }

        public void Update(SimpleRequestTour simpleRequestTour)
        {
            Load();
            int idx = _simpleRequestTours.FindIndex(r => r.Id == simpleRequestTour.Id);
            if (idx >= 0)
            {
                _simpleRequestTours[idx] = simpleRequestTour;
                Save();
            }
            else
            {
                throw new ArgumentException("The specified simpleRequest does not exist in the CSV.");
            }
        }

        public int MakeId()
        {
            Load();
            return _simpleRequestTours.Count == 0 ? 0 : _simpleRequestTours.Max(n => n.Id) + 1;
        }

        public void Add(SimpleRequestTour simpleRequestTour)
        {
            Load();
            _simpleRequestTours.Add(simpleRequestTour);
            Save();
        }

        public void Save()
        {
            _serializer.ToCSV(fileName, _simpleRequestTours);
        }

        public void Delete(SimpleRequestTour simpleRequestTour)
        {
            Load();
            _simpleRequestTours.Remove(simpleRequestTour);
            Save();
        }

        public List<SimpleRequestTour> GetAllByGuest2(User user)
        {
            Load();
            return _simpleRequestTours.FindAll(s => s.Guest2.Id == user.Id);
        }
    }
}

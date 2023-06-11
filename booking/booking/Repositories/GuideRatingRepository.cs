using booking.Domain.Model;
using booking.Domain.RepositoryInterfaces;
using booking.Model;
using booking.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace booking.Repositories
{
    public class GuideRatingRepository : IGuideRatingRepository
    {
        private List<GuideRating> _guideRatings;
        private Serializer<GuideRating> _serializer;

        public readonly string fileName = "../../../Resources/Data/guiderating.csv";
        public GuideRatingRepository()
        {

            _serializer = new Serializer<GuideRating>();
            _guideRatings = _serializer.FromCSV(fileName);
        }
        public void Add(object entity)
        {
            _guideRatings.Add((GuideRating)entity);
            _serializer.ToCSV(fileName, _guideRatings);
        }

        public void Delete(int id)
        {
            foreach(var guideRating in _guideRatings)
            {
                if (guideRating.Id == id)
                    _guideRatings.Remove(guideRating);
            }
            _serializer.ToCSV(fileName, _guideRatings);
        }

        public IEnumerable<GuideRating> GetAll()
        {
            _guideRatings = _serializer.FromCSV(fileName);
            return _guideRatings.ToList();
        }

        public object GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            _serializer.ToCSV(fileName, _guideRatings);
        }
        public int MakeID()
        {
            if (_guideRatings.Count > 0)
                return _guideRatings[_guideRatings.Count - 1].Id + 1;
            else
                return 1;
        }

        public int GetIdOf(GuideRating guideRating)
        {
            return _guideRatings.Find(g => g.Id == guideRating.Id).Id;
        }
        

        public void Update(GuideRating guideRating)
        {
            int idx = _guideRatings.FindIndex(a => a.Id == guideRating.Id);
            _guideRatings[idx] = guideRating;
            _serializer.ToCSV(fileName, _guideRatings);
        }

        public GuideRating GetByAppointmentId(int id)
        {
            return _guideRatings.Find(g => g.AppointmentId== id);
        }
    }
}

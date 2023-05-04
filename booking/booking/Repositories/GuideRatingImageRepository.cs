using booking.Domain.Model;
using booking.Domain.RepositoryInterfaces;
using booking.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace booking.Repositories
{
    public class GuideRatingImageRepository : IGuideRatingImageRepository
    {
        private List<GuideRatingImage> _guideRatingsImages;
        private Serializer<GuideRatingImage> _serializer;

        public readonly string fileName = "../../../Resources/Data/guideratingimage.csv";
        public GuideRatingImageRepository()
        {

            _serializer = new Serializer<GuideRatingImage>();
            _guideRatingsImages = _serializer.FromCSV(fileName);
        }
        public void Add(object entity)
        {
            _guideRatingsImages.Add((GuideRatingImage)entity);
            _serializer.ToCSV(fileName, _guideRatingsImages);
        }

        public void Delete(int id)
        {
            foreach (var guideRatingsImage in _guideRatingsImages)
            {
                if (guideRatingsImage.Id == id)
                    _guideRatingsImages.Remove(guideRatingsImage);
            }
            _serializer.ToCSV(fileName, _guideRatingsImages);
        }

        public IEnumerable<object> GetAll()
        {
            return _guideRatingsImages.ToList();
        }

        public object GetById(int id)
        {
            return _guideRatingsImages.Find(g => g.Id == id);
        }

        public void Save()
        {
            _serializer.ToCSV(fileName, _guideRatingsImages);
        }
        public int MakeID()
        {
            if (_guideRatingsImages.Count > 0)
                return _guideRatingsImages[_guideRatingsImages.Count - 1].Id + 1;
            else
                return 1;
        }

        IEnumerable<GuideRatingImage> IGuideRatingImageRepository.GetAll()
        {
            throw new NotImplementedException();
        }
    }
}

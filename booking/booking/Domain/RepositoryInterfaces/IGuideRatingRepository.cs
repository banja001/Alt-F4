using booking.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace booking.Domain.RepositoryInterfaces
{
    public interface IGuideRatingRepository
    {
        public int MakeID();
        public int GetIdOf(GuideRating guideRating);
        IEnumerable<GuideRating> GetAll();
        object GetById(int id);
        void Save();
        void Delete(int id);
        void Add(object entity);
        public void Update(GuideRating guideRating);
        public GuideRating GetByAppointmentId(int id);
    }
}

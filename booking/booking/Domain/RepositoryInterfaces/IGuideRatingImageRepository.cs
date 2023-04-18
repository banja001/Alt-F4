using booking.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace booking.Domain.RepositoryInterfaces
{
    public interface IGuideRatingImageRepository
    {
        public int MakeID();
        IEnumerable<GuideRatingImage> GetAll();
        object GetById(int id);
        void Save();
        void Delete(int id);
        void Add(object entity);
    }
}

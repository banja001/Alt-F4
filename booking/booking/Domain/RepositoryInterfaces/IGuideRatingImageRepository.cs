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
        public void Add(object entity);
    }
}

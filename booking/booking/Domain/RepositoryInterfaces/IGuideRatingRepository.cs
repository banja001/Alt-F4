using booking.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace booking.Domain.RepositoryInterfaces
{
    public interface IGuideRatingRepository : IRepository
    {
        public int MakeID();
        public int GetIdOf(GuideRating guideRating);
    }
}

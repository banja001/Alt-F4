using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace booking.Domain.RepositoryInterfaces
{
    public interface IRepository
    {
        IEnumerable<object> GetAll();
        object GetById(int id);
        void Save();
        void Delete(int id);
        void Add(object entity);
    }
}

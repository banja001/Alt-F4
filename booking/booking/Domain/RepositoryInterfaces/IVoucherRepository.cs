using booking.Domain.RepositoryInterfaces;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.RepositoryInterfaces
{
    public interface IVoucherRepository
    {
        public int MakeID();
        public bool Contains(object entity);
        public void AddRange(IEnumerable<object> entities);
        IEnumerable<Voucher> GetAll();
        object GetById(int id);
        void Save();
        void Delete(int id);
        void Add(object entity);
        void Update(Voucher voucher);
    }
}

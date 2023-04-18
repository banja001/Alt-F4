using booking.Domain.Model;
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
    public class VoucherRepository : IVoucherRepository
    {
        private List<Voucher> _vouchers;
        private Serializer<Voucher> _serializer;

        public readonly string fileName = "../../../Resources/Data/voucher.csv";
        public VoucherRepository()
        {

            _serializer = new Serializer<Voucher>();
            _vouchers = _serializer.FromCSV(fileName);
        }
        public void Add(object entity)
        {

            _vouchers.Add((Voucher)entity);
            _serializer.ToCSV(fileName, _vouchers);
        }

        public void Delete(int id)
        {
            List<Voucher> vouchers = _vouchers.ToList();
            foreach (var voucher in vouchers)
            {
                if (voucher.Id == id)
                    _vouchers.Remove(voucher);
            }
            _serializer.ToCSV(fileName, _vouchers);
        }

        public IEnumerable<Voucher> GetAll()
        {
            return _vouchers.ToList();
        }

        public object GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            _serializer.ToCSV(fileName, _vouchers);
        }

        public int MakeID()
        {
            if (_vouchers.Count > 0)
                return _vouchers[_vouchers.Count - 1].Id + 1;
            else
                return 1;
        }

        public bool Contains(object entity)
        {
            Voucher voucher = entity as Voucher;
            _vouchers = _serializer.FromCSV(fileName);
            foreach (Voucher v in _vouchers)
            {
                bool isUnique = (voucher.ObtainDate.Date != v.ObtainDate.Date) || (voucher.GuideId != v.GuideId) || (voucher.Guest2Id != v.Guest2Id);
                bool isGivenByGuide = v.GuideId>0;
                if (!isUnique && !isGivenByGuide)
                    return true;
            }
            return false;
        }

        public void AddRange(IEnumerable<object> entities)
        {
            List<Voucher> newVouchers = entities as List<Voucher>;
            foreach(var voucher in newVouchers)
            {
                voucher.Id = MakeID();
                _vouchers.Add(voucher);
            }
            Save();
        }
        public void Update(Voucher voucher)
        {
            int idx = _vouchers.IndexOf(_vouchers.Find(v => v.Id == voucher.Id));
            _vouchers[idx].ExpirationDate = voucher.ExpirationDate;
            _vouchers[idx].ObtainDate = voucher.ObtainDate;
            _vouchers[idx].IsUsed = voucher.IsUsed;
            _vouchers[idx].GuideId = voucher.GuideId;
            _vouchers[idx].Guest2Id = voucher.Guest2Id;
            Save();
        }

    }
}

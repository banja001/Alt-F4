using booking.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.RepositoryInterfaces
{
    public interface IOwnerRatingImageRepository
    {
        public void Load();
        public List<OwnerRatingImage> GetAll();
        public void Add(OwnerRatingImage acci);
        public List<OwnerRatingImage> GetByReservedDatesId(int ReservedDatesId);
        public void Save();
        public int MakeId();
    }
}

using booking.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.RepositoryInterfaces
{
    public interface IOwnerRatingRepository
    {
        public void Load();
        public List<OwnerRating> GetAll();
        public void AddRating(OwnerRating acci);
        public void Save();
        public int MakeId();
    }
}

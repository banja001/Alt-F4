using booking.Model;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.RepositoryInterfaces
{
    internal interface IRenovationDatesRepository
    {
        public void Load();
        public List<RenovationDates> GetAll();
        
        
        public void Update(RenovationDates renovationDate);
        public int MakeId();
        public void Add(RenovationDates renovationDate);
        public void Remove(RenovationDates renovationDate);
        public void Save();
        
        
    }
}

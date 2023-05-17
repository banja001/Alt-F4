using booking.Model;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.RepositoryInterfaces
{
    public interface ISimpleRequestRepository
    {
        public void Load();
        public List<SimpleRequest> GetAll();
        public SimpleRequest GetById(int id);
        public void Update(SimpleRequest simpleRequest);
        public int MakeId();
        public void Add(SimpleRequest simpleRequest);
        public void Save();
        public void Delete(SimpleRequest simpleRequest);
    }
}

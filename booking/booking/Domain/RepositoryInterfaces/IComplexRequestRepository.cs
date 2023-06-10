using booking.Model;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.RepositoryInterfaces
{
    public interface IComplexRequestRepository
    {
        public void Load();
        public List<ComplexRequest> GetAll();
        public ComplexRequest GetById(int id);
        public void Update(ComplexRequest complexRequest);
        public int MakeId();
        public void Add(ComplexRequest complexRequest);
        public void Save();
        public void Delete(ComplexRequest complexRequest);
        public List<ComplexRequest> GetAllByGuest2(User user);
        public void UpdateStatus(int id, SimpleRequestStatus status);
    }
}

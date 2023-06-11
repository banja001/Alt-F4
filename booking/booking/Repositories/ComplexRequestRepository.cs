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
    public class ComplexRequestRepository : IComplexRequestRepository
    {
        private List<ComplexRequest> _complexRequests;
        private Serializer<ComplexRequest> _serializer;

        private readonly string fileName = "../../../Resources/Data/complexrequest.csv";
        public ComplexRequestRepository()
        {
            _serializer = new Serializer<ComplexRequest>();
            Load();
        }

        public void Load()
        {
            _complexRequests = _serializer.FromCSV(fileName);
        }

        public List<ComplexRequest> GetAll()
        {
            Load();
            return _complexRequests;
        }

        public ComplexRequest GetById(int id)
        {
            Load();
            return _complexRequests.Find(s => s.Id == id);
        }

        public void Update(ComplexRequest complexRequest)
        {
            Load();
            int idx = _complexRequests.FindIndex(r => r.Id == complexRequest.Id);
            if (idx >= 0)
            {
                _complexRequests[idx] = complexRequest;
                Save();
            }
            else
            {
                throw new ArgumentException("The specified simpleRequest does not exist in the CSV.");
            }
        }

        public int MakeId()
        {
            Load();
            return _complexRequests.Count == 0 ? 1 : _complexRequests.Max(n => n.Id) + 1;
        }

        public void Add(ComplexRequest complexRequest)
        {
            Load();
            complexRequest.Id = MakeId();   
            _complexRequests.Add(complexRequest);
            Save();
        }

        public void Save()
        {
            _serializer.ToCSV(fileName, _complexRequests);
        }

        public void Delete(ComplexRequest complexRequest)
        {
            Load();
            _complexRequests.Remove(complexRequest);
            Save();
        }
        public List<ComplexRequest> GetAllByGuest2(User user)
        {
            Load();
            return _complexRequests.FindAll(s => s.User.Id == user.Id);
        }
        public void UpdateStatus(int id, SimpleRequestStatus status)
        {
            Load();
            int idx = _complexRequests.FindIndex(r => r.Id == id);
            if (idx >= 0)
            {
                _complexRequests[idx].Status = status;
                Save();
            }
            else
            {
                throw new ArgumentException("The specified simpleRequest does not exist in the CSV.");
            }
        }
    }
}

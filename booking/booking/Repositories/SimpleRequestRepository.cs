using booking.Model;
using booking.Serializer;
using Domain.Model;
using Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Repositories
{
    public class SimpleRequestRepository :ISimpleRequestRepository
    {
        private List<SimpleRequest> _simpleRequests;
        private Serializer<SimpleRequest> _serializer;

        private readonly string fileName = "../../../Resources/Data/simplerequest.csv";
        public SimpleRequestRepository()
        {
            _serializer = new Serializer<SimpleRequest>();
            Load();
        }

        public void Load()
        {
            _simpleRequests = _serializer.FromCSV(fileName);
        }

        public List<SimpleRequest> GetAll()
        {
            Load();
            return _simpleRequests;
        }

        public SimpleRequest GetById(int id)
        {
            Load();
            return _simpleRequests.Find(s => s.Id == id);
        }

        public void Update(SimpleRequest simpleRequest)
        {
            Load();
            int idx = _simpleRequests.FindIndex(r => r.Id == simpleRequest.Id);
            if (idx >= 0)
            {
                _simpleRequests[idx] = simpleRequest;
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
            return _simpleRequests.Count == 0 ? 0 : _simpleRequests.Max(n => n.Id) + 1;
        }

        public void Add(SimpleRequest simpleRequest)
        {
            Load();
            _simpleRequests.Add(simpleRequest);
            Save(); 
        }

        public void Save()
        {
            _serializer.ToCSV(fileName, _simpleRequests);
        }

        public void Delete(SimpleRequest simpleRequest)
        {
            Load();
            _simpleRequests.Remove(simpleRequest);
            Save();
        }
        public List<SimpleRequest> GetAllByGuest2(User user)
        {
            Load();
            return _simpleRequests.FindAll(s => s.User.Id == user.Id);
        }
        public void UpdateStatus(int id,SimpleRequestStatus status)
        {
            Load();
            int idx = _simpleRequests.FindIndex(r => r.Id == id);
            if (idx >= 0)
            {
                _simpleRequests[idx].Status = status;
                Save();
            }
            else
            {
                throw new ArgumentException("The specified simpleRequest does not exist in the CSV.");
            }
        }
    }
}

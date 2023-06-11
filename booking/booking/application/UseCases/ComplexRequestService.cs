using booking.Injector;
using booking.Model;
using Domain.Model;
using Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace application.UseCases
{
    public class ComplexRequestService
    {
        private readonly IComplexRequestRepository _complexRequestRepository;

        public ComplexRequestService()
        {
            _complexRequestRepository = Injector.CreateInstance<IComplexRequestRepository>();
        }
        public void Update(ComplexRequest complexRequest)
        {
            _complexRequestRepository.Update(complexRequest);
        }
        public void Save()
        {
            _complexRequestRepository.Save();   
        }
        public List<ComplexRequest> GetAllByGuest2(User guest2)
        {
            return _complexRequestRepository.GetAllByGuest2(guest2);
        }
        public void Add(ComplexRequest complexRequest) 
        { 
            _complexRequestRepository.Add(complexRequest);  
        }
        public List<ComplexRequest> GetAll()
        {
            return _complexRequestRepository.GetAll();
        }
        public ComplexRequest GetById(int id)
        {
            return _complexRequestRepository.GetById(id);
        }
    }
}

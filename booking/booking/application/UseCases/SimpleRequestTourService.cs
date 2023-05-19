using booking.Injector;
using booking.Model;
using Domain.Model;
using Domain.RepositoryInterfaces;
using Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace application.UseCases
{
    public class SimpleRequestTourService
    {
        private readonly ISimpleRequestTourRepository _simpleRequestTourRepository;
        public SimpleRequestTourService()
        {
            _simpleRequestTourRepository = Injector.CreateInstance<ISimpleRequestTourRepository>();
        }
        public List<SimpleRequestTour> GetAll()
        {
            return _simpleRequestTourRepository.GetAll();  
        }
        public List<SimpleRequestTour> GetAllByGuest2(User user)
        {
            return _simpleRequestTourRepository.GetAllByGuest2(user);
        }
        public void Add(SimpleRequestTour srt)
        {
            srt.Id = _simpleRequestTourRepository.MakeId();
            _simpleRequestTourRepository.Add(srt);
        }
    }
}

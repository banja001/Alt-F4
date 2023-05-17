using booking.Injector;
using Domain.DTO;
using Domain.Model;
using Domain.RepositoryInterfaces;
using Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace application.UseCases
{
    public class SimpleRequestService
    {
        private readonly ISimpleRequestRepository _simpleRequestRepository;
        public SimpleRequestService() 
        {
            _simpleRequestRepository = Injector.CreateInstance<ISimpleRequestRepository>();
        }
        public void Add(SimpleRequest simpleRequest)
        {
            _simpleRequestRepository.Add(new SimpleRequest(_simpleRequestRepository.MakeId(), simpleRequest));
        }
        public List<SimpleRequestDTO> GetAllDTOs()
        {
            List<SimpleRequestDTO> simpleRequestDTOs = new List<SimpleRequestDTO>();
            var simpleRequests = _simpleRequestRepository.GetAll();
            foreach(var simpleRequest in simpleRequests)
            {
                simpleRequestDTOs.Add(new SimpleRequestDTO(simpleRequest.Id + 1,
                                                           simpleRequest.Description,
                                                           simpleRequest.NumberOfGuests,
                                                           simpleRequest.Language,
                                                           simpleRequest.DateRange.StartDate,
                                                           simpleRequest.DateRange.EndDate,
                                                           simpleRequest.GetStatusUri()
                                                            ));
            }
            return simpleRequestDTOs;
        }
    }
}

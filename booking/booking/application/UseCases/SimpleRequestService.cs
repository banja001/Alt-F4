using booking.Injector;
using booking.Model;
using Domain.DTO;
using Domain.Model;
using Domain.RepositoryInterfaces;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace application.UseCases
{
    public class SimpleRequestService
    {
        private readonly ISimpleRequestRepository _simpleRequestRepository;
        private readonly LocationService _locationService;
        public SimpleRequestService() 
        {
            _simpleRequestRepository = Injector.CreateInstance<ISimpleRequestRepository>();
            _locationService = new LocationService();
        }
        public void Add(SimpleRequest simpleRequest)
        {
            simpleRequest.Id = _simpleRequestRepository.MakeId();
            _simpleRequestRepository.Add(simpleRequest);
        }
        public List<SimpleRequestDTO> CreateDTOsByGuest2(User user)
        {
            List<SimpleRequestDTO> simpleRequestDTOs = new List<SimpleRequestDTO>();
            var simpleRequests = _simpleRequestRepository.GetAllByGuest2(user);
            int madeId = simpleRequests.Count == 0 ? 1 : simpleRequests.Max(r => r.Id) + 1;

            foreach (var simpleRequest in simpleRequests)
            {
                if (simpleRequest.User.Id != user.Id)
                    continue;
                simpleRequestDTOs.Add(new SimpleRequestDTO(madeId++,
                                                           simpleRequest.Description,
                                                           simpleRequest.NumberOfGuests,
                                                           simpleRequest.Language,
                                                           simpleRequest.DateRange.StartDate,
                                                           simpleRequest.DateRange.EndDate,
                                                           simpleRequest.GetStatusUri(),
                                                           _locationService.GetById(simpleRequest.Location.Id)
                                                            ));
            }
            return simpleRequestDTOs;
        }
        public void CheckApproval()
        {
            var simpleRequests = _simpleRequestRepository.GetAll();
            foreach(var simpleRequest in simpleRequests)
            {
                bool isInvalid = simpleRequest.DateRange.StartDate.Date >= DateTime.Now.AddDays(-2);
                if (!isInvalid)
                    continue;
                simpleRequest.Status = SimpleRequestStatus.INVALID;
                _simpleRequestRepository.Update(simpleRequest);
            }
        }

        public List<SimpleAndComplexTourRequestsDTO> CreateListOfSimpleRequests()
        {
            List<SimpleAndComplexTourRequestsDTO> simpleRequests=new List<SimpleAndComplexTourRequestsDTO>();
            foreach(SimpleRequest simpleRequest in _simpleRequestRepository.GetAll())
            {
                SimpleAndComplexTourRequestsDTO SaCTRDTO=new SimpleAndComplexTourRequestsDTO(simpleRequest);
                SaCTRDTO.Location = _locaRepository.GetAll().Find(l => l.Id == SaCTRDTO.Location.Id);
                simpleRequests.Add( SaCTRDTO );
            }
            return simpleRequests;
        }
        
    }
}

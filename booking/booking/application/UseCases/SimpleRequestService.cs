﻿using booking.application.UseCases;
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
        private readonly SimpleRequestTourService _simpleRequestTourService;
        private readonly TourService _tourService;
        public SimpleRequestService() 
        {
            _simpleRequestRepository = Injector.CreateInstance<ISimpleRequestRepository>();
            _locationService = new LocationService();
            _simpleRequestTourService = new SimpleRequestTourService();
            _tourService = new TourService();
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
            int idx = 0;
                //simpleRequests.Count == 0 ? 1 : simpleRequests.Max(r => r.Id) + 1;

            foreach (var simpleRequest in simpleRequests)
            {
                if (simpleRequest.User.Id != user.Id)
                    continue;
                simpleRequestDTOs.Add(new SimpleRequestDTO(idx++,
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
        public List<SimpleRequestTourDTO> CreateNotificationsByGuest2(User user)
        {
            List<SimpleRequestTourDTO> simpleRequestTourDTOs = new List<SimpleRequestTourDTO>();
            var simpleRequestTours = _simpleRequestTourService.GetAllByGuest2(user);
            int idx = 0;
            foreach (var simpleRequestTour in simpleRequestTours)
            {
                simpleRequestTourDTOs.Add(new SimpleRequestTourDTO(idx,
                                                                   _tourService.GetById(simpleRequestTour.Tour.Id),
                                                                   _simpleRequestRepository.GetById(simpleRequestTour.SimpleRequest.Id)));
            }
            return simpleRequestTourDTOs;
        }
        public List<SimpleRequestTourDTO> CreateApprovedNotificationsByGuest2(User user)
        {
            var notifications = CreateNotificationsByGuest2(user);

            foreach (var notification in notifications)
            {
                if (notification.SimpleRequest.Status == SimpleRequestStatus.APPROVED)
                    continue;
                notifications.Remove(notification);
            }
            return notifications;
        }
        public List<SimpleRequestTourDTO> CreateSuggestionNotificationsByGuest2(User user)
        {
            var notifications = CreateNotificationsByGuest2(user);

            foreach (var notification in notifications)
            {
                if (notification.SimpleRequest.Status == SimpleRequestStatus.INVALID)
                    continue;
                notifications.Remove(notification);
            }
            return notifications;
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
                if (simpleRequest.Status == SimpleRequestStatus.ON_HOLD)
                {
                    SimpleAndComplexTourRequestsDTO SaCTRDTO = new SimpleAndComplexTourRequestsDTO(simpleRequest);
                    SaCTRDTO.Location = _locationService.GetAll().Find(l => l.Id == SaCTRDTO.Location.Id);
                    simpleRequests.Add(SaCTRDTO);
                }
            }
            return simpleRequests;
        }
        public List<SimpleRequest> GetAll()
        {
            return _simpleRequestRepository.GetAll();
        }
        public SimpleRequest GetById(int id) 
        { 
            return _simpleRequestRepository.GetById(id);
        }
        public void Update(SimpleRequest simpleRequest)
        {
            _simpleRequestRepository.Update(simpleRequest);
        }
    }
}

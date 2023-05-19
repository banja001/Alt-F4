using booking.application.UseCases;
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
using System.Windows.Media.Animation;

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
        public List<string> GetAvailableRequestsYears(User user)
        {
            var simpleRequests = _simpleRequestRepository.GetAllByGuest2(user);
            var years = simpleRequests.Select(y => y.DateRange.StartDate.Year.ToString()).Distinct().ToList();
            return years;
        }
        public double GetApprovedRequestsRatioByGuest2(User user, DateTime desiredYear)
        {
            var simpleRequests = _simpleRequestRepository.GetAllByGuest2(user);
            if (desiredYear == new DateTime())
                return ((double)simpleRequests.Count(s => s.Status == SimpleRequestStatus.APPROVED) /  simpleRequests.Count());
            return (double)simpleRequests.Count(s => s.Status == SimpleRequestStatus.APPROVED && s.DateRange.StartDate.Year == desiredYear.Year) / simpleRequests.Count(s => s.DateRange.StartDate.Year == desiredYear.Year);

        }
        public double GetInvalidRequestsRatioByGuest2(User user, DateTime desiredYear)
        {
            var simpleRequests = _simpleRequestRepository.GetAllByGuest2(user);
            if (desiredYear == new DateTime())
                return ((double)simpleRequests.Count(s => s.Status == SimpleRequestStatus.INVALID) / simpleRequests.Count());
            return (double)simpleRequests.Count(s => s.Status == SimpleRequestStatus.INVALID && s.DateRange.StartDate.Year == desiredYear.Year) / simpleRequests.Count(s => s.DateRange.StartDate.Year == desiredYear.Year);
        }
        public double GetAveragePeopleCountByGuest2(User user, DateTime desiredYear)
        {
            var simpleRequests = _simpleRequestRepository.GetAllByGuest2(user);
            if (desiredYear == new DateTime())
                return simpleRequests.FindAll(s => s.User.Id == user.Id).Average(s => s.NumberOfGuests);

            var validRequests = simpleRequests.FindAll(s => s.Status == SimpleRequestStatus.APPROVED && s.User.Id == user.Id && s.DateRange.StartDate.Year == desiredYear.Year);

            return validRequests.Count == 0 ? 0 : validRequests.Average(s => s.NumberOfGuests);
        }
        public void Add(SimpleRequest simpleRequest)
        {
            simpleRequest.Id = _simpleRequestRepository.MakeId();
            _simpleRequestRepository.Add(simpleRequest);
        }
        public List<Location> GetInvalidRequestsLocationsForGuest2(User user)
        {
            var locations = new List<Location>();
            var simpleRequests = _simpleRequestRepository.GetAllByGuest2(user);
            foreach (SimpleRequest simpleRequest in simpleRequests) 
            {
                if (simpleRequest.Status != SimpleRequestStatus.INVALID)
                    continue;
                locations.Add(_locationService.GetById(simpleRequest.Location.Id));
            }
            return locations;
        }
        public List<string> GetInvlaidRequestsLanguagesForGuest2(User user)
        {
            var languages = new List<string>();
            var simpleRequests = _simpleRequestRepository.GetAllByGuest2(user);
            foreach (SimpleRequest simpleRequest in simpleRequests)
            {
                if (simpleRequest.Status != SimpleRequestStatus.INVALID)
                    continue;
                languages.Add(simpleRequest.Language);
            }
            return languages;
        }
        public List<SimpleRequestDTO> CreateDTOsByGuest2(User user)
        {
            List<SimpleRequestDTO> simpleRequestDTOs = new List<SimpleRequestDTO>();
            var simpleRequests = _simpleRequestRepository.GetAllByGuest2(user);
            int idx = 0;

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
                var dto = new SimpleRequestTourDTO(idx++,
                                                   _tourService.GetById(simpleRequestTour.Tour.Id),
                                                   _simpleRequestRepository.GetById(simpleRequestTour.SimpleRequest.Id));
                dto.Tour.Location = _locationService.GetById(dto.Tour.Location.Id);
                simpleRequestTourDTOs.Add(dto);
            }
            return simpleRequestTourDTOs;
        }
        public List<SimpleRequestTourDTO> CreateNotifications()
        {
            List<SimpleRequestTourDTO> simpleRequestTourDTOs = new List<SimpleRequestTourDTO>();
            var simpleRequestTours = _simpleRequestTourService.GetAll();
            int idx = 0;
            foreach (var simpleRequestTour in simpleRequestTours)
            {
                var dto = new SimpleRequestTourDTO(idx++,
                                                   _tourService.GetById(simpleRequestTour.Tour.Id),
                                                   _simpleRequestRepository.GetById(simpleRequestTour.SimpleRequest.Id));
                dto.Tour.Location = _locationService.GetById(dto.Tour.Location.Id);
                simpleRequestTourDTOs.Add(dto);
            }
            return simpleRequestTourDTOs;
        }
        public List<SimpleRequestTourDTO> CreateApprovedNotificationsByGuest2(User user)
        {
            var notifications = CreateNotificationsByGuest2(user);
            List<SimpleRequestTourDTO> approvedNotifications = new List<SimpleRequestTourDTO>();

            foreach (var notification in notifications)
            {
                if (notification.SimpleRequest.Status != SimpleRequestStatus.APPROVED)
                    continue;
                approvedNotifications.Add(notification);
            }
            return approvedNotifications;
        }
        public List<SimpleRequestTourDTO> CreateSuggestionNotificationsByGuest2(User user)
        {
            var notifications = CreateNotifications();
            var invalidRequestLocations = GetInvalidRequestsLocationsForGuest2(user);
            var invalidRequestsLanguages = GetInvlaidRequestsLanguagesForGuest2(user);
            List<SimpleRequestTourDTO> suggestionNotifications = new List<SimpleRequestTourDTO>();

            foreach (var notification in notifications)
            {
                if(notification.SimpleRequest.Status == SimpleRequestStatus.APPROVED && notification.SimpleRequest.User.Id != user.Id)
                {
                    CheckRequestCompatibility(invalidRequestLocations, invalidRequestsLanguages, suggestionNotifications, notification);
                }
            }
            return suggestionNotifications;
        }

        private void CheckRequestCompatibility(List<Location> invalidRequestLocations, List<string> invalidRequestsLanguages, List<SimpleRequestTourDTO> suggestionNotifications, SimpleRequestTourDTO notification)
        {
            var notificationLocation = notification.Tour.Location;

            notificationLocation = _locationService.GetById(notificationLocation.Id);
            bool containsLocation = invalidRequestLocations.Any(l => l.State.Equals(notificationLocation.State) && l.City.Equals(notificationLocation.City));
            bool containsLanguage = invalidRequestsLanguages.Any(l => l.Equals(notification.Tour.Language));

            if (containsLocation || containsLanguage)
                suggestionNotifications.Add(notification);
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
        public void UpdateStatus(int id, SimpleRequestStatus status) 
        {
            _simpleRequestRepository.UpdateStatus(id,status);
        }
    }
}

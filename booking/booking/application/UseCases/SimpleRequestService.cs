using booking.application.UseCases;
using booking.Injector;
using booking.Model;
using Domain.DTO;
using Domain.Model;
using Domain.RepositoryInterfaces;
using Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace application.UseCases
{
    public class SimpleRequestService
    {
        private readonly ISimpleRequestRepository _simpleRequestRepository;
        private readonly LocationService _locationService;
        private readonly SimpleRequestTourService _simpleRequestTourService;
        private readonly ComplexRequestService _complexRequestService;
        private readonly TourService _tourService;
        private readonly UserService _userService;
        public SimpleRequestService()
        {
            _simpleRequestRepository = Injector.CreateInstance<ISimpleRequestRepository>();
            _locationService = new LocationService();
            _complexRequestService = new ComplexRequestService();   
            _simpleRequestTourService = new SimpleRequestTourService();
            _tourService = new TourService();
            _userService = new UserService();
        }
        public void InitializeTimer(EventHandler dispatcherTicker, int incrementer)
        {
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval = TimeSpan.FromHours(1);
            dispatcherTimer.Tick += dispatcherTicker;
            dispatcherTimer.Start();
        }
        public List<SimpleRequest> ConvertByGuest2(List<SimpleRequestDTO> simpleRequestDTOs, User guest2)
        {
            var list = new List<SimpleRequest>();
            foreach (var simpleRequestDTO in simpleRequestDTOs)
            {
                list.Add(new SimpleRequest(guest2, simpleRequestDTO));
            }
            return list;    
        }
        public Dictionary<string, int> GetLanguageChartByGuest2(User user)
        {
            var simpleRequests = _simpleRequestRepository.GetAllByGuest2(user);
            var languages = simpleRequests.Where(s => s.Status == SimpleRequestStatus.APPROVED).Select(s => s.Language).Distinct();
            Dictionary<string, int> languageRequestCountPairs = new Dictionary<string, int>();
            foreach (var language in languages)
            {
                int requestCount = simpleRequests.Count(s => s.Language == language);
                languageRequestCountPairs.Add(language, requestCount);
            }
            return languageRequestCountPairs;
        }
        public Dictionary<string, int> GetLocationChartByGuest2(User user)
        {
            var simpleRequests = _simpleRequestRepository.GetAllByGuest2(user);
            var locationIds = simpleRequests.Where(s => s.Status == SimpleRequestStatus.APPROVED).Select(s => s.Location.Id).Distinct();
            var locations = locationIds.Select(l => _locationService.GetById(l).CityState).Distinct();
            Dictionary<string, int> LocationsRequestCountPairs = new Dictionary<string, int>();
            foreach (var location in locations)
            {
                int requestCount = simpleRequests.Count(s => _locationService.GetById(s.Location.Id).CityState == location);
                LocationsRequestCountPairs.Add(location, requestCount);
            }
            return LocationsRequestCountPairs;
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
            List<SimpleRequest> validRequests;
            if (desiredYear == new DateTime())
            {
                validRequests = simpleRequests.FindAll(s => s.Status == SimpleRequestStatus.APPROVED && s.User.Id == user.Id);
                return validRequests.Count == 0 ? 0 : validRequests.Average(s => s.NumberOfGuests);
            }
            validRequests = simpleRequests.FindAll(s => s.Status == SimpleRequestStatus.APPROVED && s.User.Id == user.Id && s.DateRange.StartDate.Year == desiredYear.Year);
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
            var complexRequests = _complexRequestService.GetAll();
            foreach(var simpleRequest in simpleRequests)
            {
                bool isInvalid = simpleRequest.DateRange.StartDate.Date >= DateTime.Now.AddDays(-2);
                if (!isInvalid)
                    continue;
                simpleRequest.Status = SimpleRequestStatus.INVALID;
                _simpleRequestRepository.Update(simpleRequest);
            }
            foreach (var complexRequest in complexRequests)
            {
                var earliestSimpleRequest = _simpleRequestRepository.GetById(complexRequest.SimpleRequests[0].Id);
                bool isInvalid = earliestSimpleRequest.DateRange.StartDate.Date >= DateTime.Now.AddDays(-2);
                if (!isInvalid)
                    continue;
                complexRequest.Status = SimpleRequestStatus.INVALID;
                _complexRequestService.Update(complexRequest);
            }
        }

        public List<SimpleAndComplexTourRequestsDTO> CreateListOfSimpleRequestsDTO()
        {
            List<SimpleAndComplexTourRequestsDTO> simpleRequests=new List<SimpleAndComplexTourRequestsDTO>();
            foreach(SimpleRequest simpleRequest in _simpleRequestRepository.GetAll())
            {
                if (simpleRequest.Status == SimpleRequestStatus.ON_HOLD && simpleRequest.DateRange.StartDate>DateTime.Now.AddDays(2))
                {
                    SimpleAndComplexTourRequestsDTO SaCTRDTO = new SimpleAndComplexTourRequestsDTO(simpleRequest);
                    SaCTRDTO.Location = _locationService.GetAll().Find(l => l.Id == SaCTRDTO.Location.Id);
                    simpleRequests.Add(SaCTRDTO);
                }
            }
            return simpleRequests;
        }
        public List<SimpleRequest> FindAllSimpleTourInSimpleRequestsTours(int guideId)
        {
            List<SimpleRequest> simpleRequests=new List<SimpleRequest>();
            List<SimpleRequestTour> all = _simpleRequestTourService.GetAll();
            foreach (var str in all)
            {
                str.Tour = _tourService.FindAll().Find(t=>t.Id==str.Tour.Id);
                SimpleRequest s= _simpleRequestRepository.GetAll().Find(sr=>sr.Id==str.SimpleRequest.Id);
                s = _simpleRequestRepository.GetById(str.SimpleRequest.Id);
                if(s.Status==SimpleRequestStatus.APPROVED && str.Tour.Guide.Id==guideId)
                    simpleRequests.Add(s);
            }
            return simpleRequests;
        }

        
        public ComplexRequest CheckWhichComplexTour(int id)
        {
            List<ComplexRequest> complexRequests = _complexRequestService.GetAll();
            foreach (var str in complexRequests)
            {
                if (str.SimpleRequests.Find(sr => sr.Id == id) != null)
                {
                    return str;
                }

            }
            return null;
        }
        public List<SimpleRequest> GetAll()
        {
            return _simpleRequestRepository.GetAll();
        }
        public List<SimpleRequest> GetAllWithLocation()
        {
            List<SimpleRequest> list= new List<SimpleRequest>();
            foreach (SimpleRequest sr in _simpleRequestRepository.GetAll())
            {
                sr.Location = _locationService.GetAll().Find(l => l.Id == sr.Location.Id);
                list.Add(sr);
            }
            return list;
        }
        public List<SimpleRequest> GetAllAcceptedWithLocation()
        {
            List<SimpleRequest> list = new List<SimpleRequest>();
            foreach (SimpleRequest sr in _simpleRequestRepository.GetAll())
            {
                if(sr.Status==SimpleRequestStatus.APPROVED) 
                {
                    sr.Location = _locationService.GetAll().Find(l => l.Id == sr.Location.Id);
                    list.Add(sr);
                }
            }
            return list;
        }
        public List<SimpleRequest> GetAllDeclinedWithLocation()
        {
            List<SimpleRequest> list = new List<SimpleRequest>();
            foreach (SimpleRequest sr in _simpleRequestRepository.GetAll())
            {
                if (sr.Status == SimpleRequestStatus.INVALID)
                {
                    sr.Location = _locationService.GetAll().Find(l => l.Id == sr.Location.Id);
                    list.Add(sr);
                }
            }
            return list;
        }

        public string GuestMostRequestedTours(List<SimpleRequest> allRequests)
        {
            var userRequests = allRequests.GroupBy(r => r.User.Id).Select(group => new { UserId = group.Key, RequestsCount = group.Count() });
            var userWithMostRequests = userRequests.OrderByDescending(user => user.RequestsCount).FirstOrDefault();

            return _userService.GetUserNameById(userWithMostRequests.UserId);
        }

        public string MostSpokenLanguageOnRequestedTours(List<SimpleRequest> allRequests)
        {
            var languages = allRequests.GroupBy(r => r.Language).Select(group => new { LanguageId = group.Key, LanguagesCount = group.Count() });
            var mostSpokenLanguage = languages.OrderByDescending(l => l.LanguagesCount).FirstOrDefault();

            return mostSpokenLanguage.LanguageId;
        }

        public string MostReguestedLocationOnRequestedTours(List<SimpleRequest> allRequests)
        {
            var locations = allRequests.GroupBy(r => r.Location.Id).Select(group => new { LocationId = group.Key, LocationCount = group.Count() });
            var mostRequestedLocation = locations.OrderByDescending(l => l.LocationCount).FirstOrDefault();
            return _locationService.GetById(mostRequestedLocation.LocationId).CityState;

        }
        public string MostReguestedStateOnRequestedTours(List<SimpleRequest> allRequests)
        {
            var locations = allRequests.GroupBy(r => r.Location.State).Select(group => new { LocationId = group.Key, LocationCount = group.Count() });
            var mostRequestedLocation = locations.OrderByDescending(l => l.LocationCount).FirstOrDefault();
            return _locationService.GetById(_locationService.GetByState(mostRequestedLocation.LocationId)).State;

        }
        public string MostReguestedMonthOnRequestedTours(List<SimpleRequest> allRequests)
        {
            var months = allRequests.GroupBy(r => r.DateRange.StartDate.Month).Select(group => new { monthId = group.Key, monthCount = group.Count() });
            var mostRequestedMonth = months.OrderByDescending(l => l.monthCount).FirstOrDefault();
            return MonthNumberToString(mostRequestedMonth.monthId.ToString());

        }
        public string MostReguestedYearOnRequestedTours(List<SimpleRequest> allRequests)
        {
            var years = allRequests.GroupBy(r => r.DateRange.StartDate.Year).Select(group => new { yearId = group.Key, yearCount = group.Count() });
            var mostRequestedYear = years.OrderByDescending(l => l.yearCount).FirstOrDefault();
            return mostRequestedYear.yearId.ToString();

        }
        public string AverageNumberOfGuestsOnRequestedTours(List<SimpleRequest> allRequests)
        {
            return (allRequests.Sum(a => a.NumberOfGuests)/allRequests.Count).ToString();

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
        public List<string> GetAllYears()
        {
            List<string>years=new List<string>();
            foreach(SimpleRequest s in _simpleRequestRepository.GetAll())
            {
                if (!years.Contains(s.DateRange.StartDate.Year.ToString()))
                {
                    years.Add(s.DateRange.StartDate.Year.ToString());
                }
            }
            years.Sort();
            return years;
        }
        public List<string> GetAllMonthsForYear(string year)
        {
            List<string> months = new List<string>();
            List<string> monthsNumber = new List<string>();
            foreach (SimpleRequest s in _simpleRequestRepository.GetAll())
            {
                if (!monthsNumber.Contains(s.DateRange.StartDate.Month.ToString()) && s.DateRange.StartDate.Year.ToString()==year)
                {
                    monthsNumber.Add(s.DateRange.StartDate.Month.ToString());
                }
            }
            monthsNumber.Sort();
            foreach (string s in monthsNumber)
            {
                months.Add(MonthNumberToString(s));
            }
            return months;
        }
        public Dictionary<string, int> GetYearsForChart(List<SimpleRequest> AllRequests)
        {
            List<string> allYears = GetAllYears();
            Dictionary<string, int> yearRequestCountPairs = new Dictionary<string, int>();
            foreach (var year in allYears)
            {
                int requestCount = AllRequests.Count(s => s.DateRange.StartDate.Date.Year.ToString() == year);
                yearRequestCountPairs.Add(year, requestCount);
            }
            return yearRequestCountPairs;
        }

        public Dictionary<string, int> GetMonthsForChart(List<SimpleRequest> AllRequests, string selectedYear)
        {
            List<string> allMonths = GetAllMonthsForYear(selectedYear);
            Dictionary<string, int> monthsRequestCountPairs = new Dictionary<string, int>();
            foreach (var month in allMonths)
            {
                int requestCount = AllRequests.Count(s => MonthNumberToString(s.DateRange.StartDate.Date.Month.ToString())== month);
                monthsRequestCountPairs.Add(month, requestCount);
            }
            return monthsRequestCountPairs;
        }

        public Dictionary<string, int> GetStatesForChart(List<SimpleRequest> AllRequests)
        {
            List<string> allStates = _locationService.InitializeListOfStates();
            Dictionary<string, int> statesRequestCountPairs = new Dictionary<string, int>();
            foreach (var state in allStates)
            {
                int requestCount = AllRequests.Count(s => s.Location.State == state);
                statesRequestCountPairs.Add(state, requestCount);
            }
            return statesRequestCountPairs;
        }

        public Dictionary<string, int> GetCitiesForChart(List<SimpleRequest> AllRequests,string selectedState)
        {
            List<string> allCities = _locationService.FillListWithCities(selectedState);
            Dictionary<string, int> citesRequestCountPairs = new Dictionary<string, int>();
            foreach (var city in allCities)
            {
                int requestCount = AllRequests.Count(s => s.Location.City == city);
                citesRequestCountPairs.Add(city, requestCount);
            }
            return citesRequestCountPairs;
        }

        public Dictionary<string, int> GetLanguagesForChart(List<SimpleRequest> AllRequests)
        {
            var languages= AllRequests.Select(s => s.Language).Distinct();
            Dictionary<string, int> lanuagesRequestCountPairs = new Dictionary<string, int>();
            foreach (var language in languages)
            {
                int requestCount = AllRequests.Count(s => s.Language == language);
                lanuagesRequestCountPairs.Add(language, requestCount);
            }
            return lanuagesRequestCountPairs;
        }

        public List<SimpleAndComplexTourRequestsDTO> FilterState(List<SimpleAndComplexTourRequestsDTO> AllRequests, string SelectedState)
        {
            List<SimpleAndComplexTourRequestsDTO> list = AllRequests.Where(req => req.Location.State == SelectedState).ToList();
            AllRequests = new List<SimpleAndComplexTourRequestsDTO>(list);
            return AllRequests;
        }
        public List<SimpleAndComplexTourRequestsDTO> FilterCity(List<SimpleAndComplexTourRequestsDTO> AllRequests, string SelectedCity)
        {
            List<SimpleAndComplexTourRequestsDTO> list = AllRequests.Where(req => req.Location.City == SelectedCity).ToList();
            AllRequests = new List<SimpleAndComplexTourRequestsDTO>(list);
            return AllRequests;
        }
        public List<SimpleAndComplexTourRequestsDTO> FilterLanguage(List<SimpleAndComplexTourRequestsDTO> AllRequests, string Language)
        {
            List<SimpleAndComplexTourRequestsDTO> list = AllRequests.Where(req => req.Language.ToLower() == Language.ToLower()).ToList();
            AllRequests = new List<SimpleAndComplexTourRequestsDTO>(list);
            return AllRequests;

        }
        public List<SimpleAndComplexTourRequestsDTO> FilterNumberOfGuests(List<SimpleAndComplexTourRequestsDTO> AllRequests, string MaxGuests)
        {
            List<SimpleAndComplexTourRequestsDTO> list = AllRequests.Where(req => req.NumberOfGuests <= Convert.ToInt32(MaxGuests)).ToList();
            AllRequests = new List<SimpleAndComplexTourRequestsDTO>(list);
            return AllRequests;
        }
        public List<SimpleAndComplexTourRequestsDTO> FilterStartDate(List<SimpleAndComplexTourRequestsDTO> AllRequests, DateTime SelectedStartDate)
        {
            List<SimpleAndComplexTourRequestsDTO> list = AllRequests.Where(req => req.StartDate.Date >= SelectedStartDate).ToList();
            AllRequests = new List<SimpleAndComplexTourRequestsDTO>(list);
            return AllRequests;
        }
        public List<SimpleAndComplexTourRequestsDTO> FilterEndDate(List<SimpleAndComplexTourRequestsDTO> AllRequests, DateTime SelectedEndDate)
        {
            List<SimpleAndComplexTourRequestsDTO> list = AllRequests.Where(req => req.EndDate.Date <= SelectedEndDate).ToList();
            AllRequests = new List<SimpleAndComplexTourRequestsDTO>(list);
            return AllRequests;
        }

        public string CreateTourWithHelpOfStatistics(string parameter)
        {

            List<SimpleRequest> AllRequests = GetAllWithLocation();
            List<SimpleRequest> AllRequestsFiltered = AllRequests.FindAll(r => r.DateRange.StartDate.Date < DateTime.Now.AddDays(365));
            string maxLanguage = MaxLanguage(AllRequests);
            string maxState=MaxState(AllRequests);
            string maxCity=MaxCity(AllRequests);
            switch (parameter)
            {
                case "State":
                    return maxState+",State";
                    break;
                case "City":
                    maxState = _locationService.GetAll().Find(l => l.City == maxCity).State;
                    return maxCity + ",City," + maxState;
                    break;
                case "Language":
                    return maxLanguage + ",Language";
                    break;
                case "All":
                    maxState = _locationService.GetAll().Find(l =>l.City==maxCity).State;
                    return maxState+"|"+maxCity+"|"+maxLanguage;
                    break;
                default:
                    return "error";
                    break;
            }
        }
        public string MaxLanguage(List<SimpleRequest> AllRequests)
        {
            Dictionary<string, int> listOfLanguage = AllRequests.GroupBy(r => r.Language).ToDictionary(r => r.Key, r => r.Count());
            return listOfLanguage.OrderByDescending(l => l.Value).First().Key;
        }
        public string MaxState(List<SimpleRequest> AllRequests) 
        {
            Dictionary<string, int> listOfStates = AllRequests.GroupBy(r => r.Location.State).ToDictionary(r => r.Key, r => r.Count());
            return listOfStates.OrderByDescending(l => l.Value).First().Key;
        }
        public string MaxCity(List<SimpleRequest> AllRequests)
        {
            Dictionary<string, int> listOfLCities = AllRequests.GroupBy(r => r.Location.City).ToDictionary(r => r.Key, r => r.Count());
            return listOfLCities.OrderByDescending(l => l.Value).First().Key;
        }

        //public 

        public string MonthNumberToString(string monthNumber)
        {
            string monthName;
            switch (Convert.ToInt32(monthNumber))
            {
                case 1:
                    monthName = "January";
                    break;
                case 2:
                    monthName = "February";
                    break;
                case 3:
                    monthName = "March";
                    break;
                case 4:
                    monthName = "April";
                    break;
                case 5:
                    monthName = "May";
                    break;
                case 6:
                    monthName = "June";
                    break;
                case 7:
                    monthName = "July";
                    break;
                case 8:
                    monthName = "August";
                    break;
                case 9:
                    monthName = "September";
                    break;
                case 10:
                    monthName = "October";
                    break;
                case 11:
                    monthName = "November";
                    break;
                case 12:
                    monthName = "December";
                    break;
                default:
                    monthName = "Invalid month number";
                    break;
            }
            return monthName;
        }
    }
}

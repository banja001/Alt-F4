using booking.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using booking.Model;
using Domain.RepositoryInterfaces;
using application.UseCases;

namespace booking.application.UseCases
{
    public class TourService
    {
        private readonly ITourRepository _tourRepository;
        private readonly LocationRepository _locationRepository;
        private readonly ReservationTourService _reservationTourService;
        public TourService()
        {
            _tourRepository = Injector.Injector.CreateInstance<ITourRepository>();
            _reservationTourService = new ReservationTourService();
            _locationRepository = new LocationRepository();
        }

        public List<Tour> FindAll()
        {
            return _tourRepository.FindAll();
        }

        public void DeleteTour(Tour tour)
        {
            _tourRepository.Delete(tour);
        }
        public List<Tour> GetVisitedToursByInterval(DateTime fromDate,  DateTime toDate, User guest2)
        {
            List<Tour> visitedTours = _reservationTourService.GetVisitedToursByGuest2(guest2);
            List<Tour> retTours = new List<Tour>();
            foreach (var tour in visitedTours) 
            {
                var startDate = tour.StartTime.Date.Date;
                if (startDate >= fromDate.Date && startDate.AddHours(tour.Duration).Date <= toDate.Date)
                    retTours.Add(tour);
            }
            return retTours;
        }

        public ObservableCollection<Tour> FindUpcomingTours()
        {
            ObservableCollection<Tour> tours = new ObservableCollection<Tour>();
            foreach (var t in _tourRepository.FindAll())
            {
                if (t.StartTime.Date > DateTime.Now)
                {
                    t.Location = _locationRepository.GetAll().Find(l => l.Id == t.Location.Id);
                    tours.Add(t);
                }

            }

            return tours;
        }
        public Tour GetById(int id)
        {
            return _tourRepository.FindById(id);
        }
        public bool CheckAvailability(DateTime date)
        {
            
            List<Tour> allTours=_tourRepository.FindAll();
            foreach (var t in allTours) 
            {
                if (t.StartTime.Date.Date == date.Date)
                    return false;
            }
            return true;
        }
    }
}

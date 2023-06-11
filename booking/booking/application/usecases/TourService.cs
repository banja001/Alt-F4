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
using Domain.Model;
using Repositories;

namespace booking.application.UseCases
{
    public class TourService
    {
        private readonly ITourRepository _tourRepository;
        private readonly LocationRepository _locationRepository;
        private readonly ReservationTourService _reservationTourService;
        private VoucherService _voucherService;
        public TourService()
        {
            _tourRepository = Injector.Injector.CreateInstance<ITourRepository>();
            _reservationTourService = new ReservationTourService();
            _locationRepository = new LocationRepository();
            _voucherService= new VoucherService();
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
        public List<Tour> FindToursByGuide(int guideId)
        {
            List<Tour> tours = new List<Tour>();
            foreach (Tour t in _tourRepository.FindAll())
            {
                if(t.Guide.Id==guideId)
                    tours.Add(t);
            }
            return tours;
        }
        public void GiveVouchersBecauseGuideQuitted(int guideId)
        {
            foreach (var rt in _reservationTourService.GetAll())
            {
                if (FindToursByGuide(guideId).Find(t => t.Id == rt.Tour.Id) != null)
                {
                    DateAndTime now = new DateAndTime(DateTime.Now, "00:00");
                    DateAndTime expire = new DateAndTime(DateTime.Now.AddDays(365), "00:00");
                    Voucher voucher = new Voucher(_voucherService.MakeID(), now, -1, rt.User.Id, expire, false);
                    _voucherService.Add(voucher);
                }
            }
        }
    }
}

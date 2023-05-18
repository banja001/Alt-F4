using booking.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using booking.Model;
using Domain.RepositoryInterfaces;

namespace booking.application.UseCases
{
    public class TourService
    {
        private readonly ITourRepository _tourRepository;
        private readonly LocationRepository _locationRepository;
        public TourService()
        {
            _tourRepository = Injector.Injector.CreateInstance<ITourRepository>();
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
    }
}

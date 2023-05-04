using booking.DTO;
using booking.Injector;
using booking.Model;
using booking.Repository;
using booking.View;
using Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace application.UseCases
{
    
    public class AccommodationService
    {
        private IAccommodationRepository accommodationRepository;
        private ILocationRepository locationRepository;
        

        public AccommodationService()
        {
            accommodationRepository = Injector.CreateInstance<IAccommodationRepository>();
            locationRepository = Injector.CreateInstance<ILocationRepository>();

        }
        public List<Accommodation> GetAll()
        {
            return accommodationRepository.GetAll();
        }
        public void Add(Accommodation acci)
        {
            accommodationRepository.Add(acci);
        }
        public Accommodation GetById(int id)
        {
            return accommodationRepository.GetById(id);
        }
        public Accommodation FindById(int id)
        {
            return accommodationRepository.FindById(id);
        }

        public ObservableCollection<AccommodationLocationDTO> CreateAccomodationDTOs()
        {
            List<Accommodation> accommodations = accommodationRepository.GetAll();
            List<Location> locations = locationRepository.GetAll();
            ObservableCollection<AccommodationLocationDTO> accommodationLocations = new ObservableCollection<AccommodationLocationDTO>();
            AccommodationLocationDTO accommodationLocation;

            foreach (Accommodation accommodation in accommodations)
            {
                accommodationLocation = CreateAccommodationLocation(locations, accommodation);

                accommodationLocations.Add(accommodationLocation);
            }

            return accommodationLocations;
        }

        private static AccommodationLocationDTO CreateAccommodationLocation(List<Location> locations, Accommodation accommodation)
        {
            AccommodationLocationDTO accommodationLocation;
            string locationCity = locations.Find(u => u.Id == accommodation.LocationId).City;
            string locationCountry = locations.Find(u => u.Id == accommodation.LocationId).State;

            accommodationLocation = new AccommodationLocationDTO(accommodation.Id, accommodation.Name, locationCity + "," + locationCountry,
                accommodation.Type, accommodation.MaxCapacity, accommodation.MinDaysToUse, accommodation.MinDaysToCancel, accommodation.Id);//dodao acc id
            return accommodationLocation;
        }
    }
}

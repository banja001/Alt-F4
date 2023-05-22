using booking.DTO;
using booking.Injector;
using booking.Model;
using booking.Repository;
using booking.View;
using Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace application.UseCases
{
    
    public class AccommodationService
    {
        private IAccommodationRepository accommodationRepository;
        private readonly LocationService _locationService;
        private readonly UserService _userService;

        public AccommodationService()
        {
            accommodationRepository = Injector.CreateInstance<IAccommodationRepository>();
            _locationService = new LocationService();
            _userService = new UserService();
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
        public List<Accommodation> GetAllById(int id)
        {
            return accommodationRepository.GetAllByOwnerId(id);
        }

        public Accommodation FindById(int id)
        {
            return accommodationRepository.FindById(id);
        }

        public ObservableCollection<AccommodationLocationDTO> CreateAccomodationDTOs()
        {
            List<Accommodation> accommodations = accommodationRepository.GetAll();
            List<Location> locations = _locationService.GetAll();
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

        public ObservableCollection<AccommodationLocationDTO> SortAccommodationDTOs(ObservableCollection<AccommodationLocationDTO> acommodationLocationDTOs)
        {
            List<Accommodation> accommodations = accommodationRepository.GetAll();
            ObservableCollection<AccommodationLocationDTO> SortedAccommodationDTOs = new ObservableCollection<AccommodationLocationDTO>();
            bool flag;
            Accommodation accommodation;
            foreach (var item in acommodationLocationDTOs)
            {
                accommodation = accommodations.Find(s => s.Id == item.AccommodationId);
                flag = _userService.GetAll().Find(s => accommodation.OwnerId == s.Id).Super;
                if (flag)
                {
                    if (!item.Name.Last().Equals("*"))
                        item.Name += "*";
                    SortedAccommodationDTOs.Insert(0, item);

                }
                else
                    SortedAccommodationDTOs.Add(item);
            }
            return SortedAccommodationDTOs;
        }
    }
}

using booking.DTO;
using booking.Model;
using booking.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace booking.DAO
{
    public class AccommodationDAO
    {
        public ObservableCollection<AccommodationLocationDTO> getAll(List<Accommodation> accommodations, LocationRepository locationRepository)
        {
            List<Location> locations = locationRepository.GetAllLocations();
            ObservableCollection<AccommodationLocationDTO> accommodationLocations = new ObservableCollection<AccommodationLocationDTO>();
            AccommodationLocationDTO accommodationLocation;

            foreach(Accommodation accommodation in accommodations) {
                string locationCity = locations.Find(u => u.Id == accommodation.LocationId).Grad;
                string locationCountry = locations.Find(u => u.Id == accommodation.LocationId).Drzava;

                accommodationLocation = new AccommodationLocationDTO(accommodation.Id, accommodation.Name, locationCity + "," + locationCountry,
                    accommodation.Type, accommodation.MaxCapacity, accommodation.MinDaysToUse, accommodation.MinDaysToCancel);

                accommodationLocations.Add(accommodationLocation);
            }
            
            return accommodationLocations;
        }
    }
}

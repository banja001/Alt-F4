using booking.DTO;
using booking.Model;
using booking.Serializer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace booking.Repository
{
    public class AccommodationRepository
    {
        private List<Accommodation> accommodations;
        private Serializer<Accommodation> serializer;

        private readonly string fileName = "../../../Resources/Data/accommodation.csv";
        public AccommodationRepository() 
        {
            //accommodations = new List<Accommodation>();
            serializer = new Serializer<Accommodation>();
            accommodations= serializer.FromCSV(fileName);
        }

        public List<Accommodation> findAll()
        {
            return accommodations;
        }

        public void AddAccommodation(Accommodation acc)
        {
            
            accommodations.Add(acc);
            serializer.ToCSV(fileName,accommodations);

        }

        public ObservableCollection<AccommodationLocationDTO> getAll(List<Accommodation> accommodations, LocationRepository locationRepository)///u code behind, createAccomodationDTOs
        {
            List<Location> locations = locationRepository.GetAllLocations();
            ObservableCollection<AccommodationLocationDTO> accommodationLocations = new ObservableCollection<AccommodationLocationDTO>();
            AccommodationLocationDTO accommodationLocation;

            foreach (Accommodation accommodation in accommodations)
            {
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

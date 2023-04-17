using booking.Injector;
using booking.Model;
using booking.View;
using Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace application.UseCases
{
    public class LocationService
    {
        private ILocationRepository locationRepository;
        private OwnerWindow ownerWindow;
        public LocationService(OwnerWindow ow)
        {
            ownerWindow = ow;
            locationRepository = Injector.CreateInstance<ILocationRepository>();
        }

        public List<Location> GetAll()
        {
            return locationRepository.GetAll();
        }

        public List<string> InitializeStateList(List<string> StateList)
        {
            foreach (Location loc in ownerWindow.locations)
            {

                if (StateList.Find(m => m == loc.State) == null)

                {
                    StateList.Add(loc.State);
                }
                
            }
            return StateList;
        }

        public int GetLocationId(string State, string City)
        {
            return ownerWindow.locations.Find(m => m.State == State && m.City == City).Id;
        }

        public List<string> FillCityList(List<string> CityList, string SelectedState)
        {
            foreach (var loc in ownerWindow.locations)
            {
                if (SelectedState == loc.State)
                {
                    CityList.Add(loc.City);
                }
            }
            return CityList;
        }

    }
}

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
        
        public LocationService()
        {
            
            locationRepository = Injector.CreateInstance<ILocationRepository>();
        }

        public List<Location> GetAll()
        {
            return locationRepository.GetAll();
        }

        public List<string> InitializeStateList(List<string> StateList,List<Location> locations)
        {
            foreach (Location loc in locations)
            {

                if (StateList.Find(m => m == loc.State) == null)

                {
                    StateList.Add(loc.State);
                }
                
            }
            return StateList;
        }

        public int GetLocationId(string State, string City, List<Location> locations)
        {
            return locations.Find(m => m.State == State && m.City == City).Id;
        }

        public List<string> FillCityList(List<string> CityList, string SelectedState, List<Location> locations)
        {
            foreach (var loc in locations)
            {
                if (SelectedState == loc.State)
                {
                    CityList.Add(loc.City);
                }
            }
            return CityList;
        }

        public Location GetById(int id)
        {
            return locationRepository.GetById(id);
        }

        public int MakeID()
        {
            return locationRepository.MakeID();
        }
        public void Add(Location loc)
        {
            locationRepository.Add(loc);
        }
        public bool Contains(Location location)
        {
            return locationRepository.GetAll().Find(l => l.State.Equals(location.State) && l.City.Equals(location.City)) == null ? false : true; ;
        }
        public int GetId(string state, string city)
        {
            return locationRepository.GetAll().FindLastIndex(l => l.State == state && l.City == city);
        }
        public int GetByState(string state)
        {
            return locationRepository.GetAll().Find(l => l.State == state).Id;
        }
        public List<string> InitializeListOfStates()
        {
            List<string> states = new List<string>();
            foreach (Location loc in locationRepository.GetAll())
            {

                if (states.Find(m => m == loc.State) == null)
                {
                    states.Add(loc.State);
                }

            }
            states.Sort();
            return states;
        }
        public List<string> FillListWithCities(string SelectedState)
        {
            List<string> cities = new List<string>();

            foreach (Location loc in locationRepository.GetAll())
            {
                if (SelectedState == loc.State && !cities.Contains(loc.City))
                {
                    cities.Add(loc.City);
                }
            }
            cities.Sort();
            return cities;
        }
    }
}

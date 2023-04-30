﻿using booking.Injector;
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

    }
}
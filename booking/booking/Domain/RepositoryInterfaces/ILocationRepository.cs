using booking.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.RepositoryInterfaces
{
    internal interface ILocationRepository
    {
        List<Location> GetAll();
        Location GetById(int id);
        void Add(Location loc);
        int MakeID();
    }
}

using booking.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.RepositoryInterfaces
{
    internal interface IGuest1RatingsRepository
    {
        List<Guest1Rating> GetAll();
        void Add(Guest1Rating acci);
        List<Guest1Rating> GetAllByGuest1Id(int userId);
    }
}

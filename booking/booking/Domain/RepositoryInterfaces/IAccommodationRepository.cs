using booking.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.RepositoryInterfaces
{
    internal interface IAccommodationRepository
    {
        List<Accommodation> GetAll();
        Accommodation GetById(int id);

        List<Accommodation> GetAllByOwnerId(int id);
        void Add(Accommodation acc);
        Accommodation FindById(int id);
    }
}

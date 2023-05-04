using booking.DTO;
using booking.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.RepositoryInterfaces
{
    internal interface IAccommodationImageRepository
    {
        List<AccommodationImage> GetAll();
        void Add(AccommodationImage acci);
        List<AccommodationImage> Get(AccommodationLocationDTO accommodation);
    }
}

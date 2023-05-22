using booking.Model;
using booking.Serializer;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.RepositoryInterfaces
{
    public interface ITourRepository
    {
        public List<Tour> FindAll();
        public void Add(Tour tour);

        public int MakeID();

        public void Delete(Tour tour);
        public Tour FindById(int id);
    }
}

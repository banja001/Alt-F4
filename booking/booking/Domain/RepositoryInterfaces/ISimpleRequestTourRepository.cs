using booking.Model;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.RepositoryInterfaces
{
    public interface ISimpleRequestTourRepository
    {
        public void Load();
        public List<SimpleRequestTour> GetAll();
        public SimpleRequestTour GetById(int id);
        public void Update(SimpleRequestTour simpleRequestTour);
        public int MakeId();
        public void Add(SimpleRequestTour simpleRequestTour);
        public void Save();
        public void Delete(SimpleRequestTour simpleRequestTour);
        public List<SimpleRequestTour> GetAllByGuest2(User user);
    }
}

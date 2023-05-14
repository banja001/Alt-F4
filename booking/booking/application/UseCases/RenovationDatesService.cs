using booking.Model;
using Domain.Model;
using Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace booking.application.UseCases
{
    public class RenovationDatesService
    {
        private readonly IRenovationDatesRepository _renovationDatesRepository;

        public RenovationDatesService()
        {
            _renovationDatesRepository = Injector.Injector.CreateInstance<IRenovationDatesRepository>();
        }
        public List<RenovationDates> GetAll()
        {
            return _renovationDatesRepository.GetAll();
        }
        
        public void Update(RenovationDates renovationDate)
        {
            _renovationDatesRepository.Update(renovationDate);
        }
        public int MakeId()
        {
            return _renovationDatesRepository.MakeId();
        }
        public void Add(RenovationDates renovationDate)
        {
            _renovationDatesRepository.Add(renovationDate);
        }
        public void Remove(RenovationDates renovationDate)
        {
            _renovationDatesRepository.Remove(renovationDate);
        }
        public void Save()
        {
            _renovationDatesRepository.Save();
        }
        
    }
}

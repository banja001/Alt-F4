using booking.Injector;
using booking.Model;
using booking.Repository;
using booking.View;
using Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace application.UseCases
{
    
    public class AccommodationService
    {
        private IAccommodationRepository accommodationRepository;
        

        public AccommodationService()
        {
            accommodationRepository = Injector.CreateInstance<IAccommodationRepository>();
            

        }
        public List<Accommodation> GetAll()
        {
            return accommodationRepository.GetAll();
        }
        public void Add(Accommodation acci)
        {
            accommodationRepository.Add(acci);
        }
        public Accommodation GetById(int id)
        {
            return accommodationRepository.GetById(id);
        }
        public Accommodation FindById(int id)
        {
            return accommodationRepository.FindById(id);
        }


    }
}

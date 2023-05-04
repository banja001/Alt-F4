using booking.Injector;
using booking.Model;
using Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace application.UseCases
{
    public class OwnerRatingImageService
    {
        private readonly IOwnerRatingImageRepository ownerRatingImageRepository;

        public OwnerRatingImageService()
        {
            this.ownerRatingImageRepository = Injector.CreateInstance<IOwnerRatingImageRepository>();
        }
        public void Load()
        {
            ownerRatingImageRepository.Load();
        }
        public List<OwnerRatingImage> GetAll()
        {
            return ownerRatingImageRepository.GetAll();
        }
        public void Add(OwnerRatingImage acci)
        {
            ownerRatingImageRepository.Add(acci);
        }
        public List<OwnerRatingImage> GetByReservedDatesId(int ReservedDatesId)
        {
            return ownerRatingImageRepository.GetByReservedDatesId(ReservedDatesId);
        }
        public void Save()
        {
            ownerRatingImageRepository.Save();
        }
        public int MakeId()
        {
            return ownerRatingImageRepository.MakeId();
        }

    }
}

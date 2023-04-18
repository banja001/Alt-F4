using booking.Injector;
using booking.Model;
using Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace application.UseCases
{
    public class OwnerRatingService
    {
        public readonly IOwnerRatingRepository ownerRatingRepository;

        public OwnerRatingService()
        {
            ownerRatingRepository = Injector.CreateInstance<IOwnerRatingRepository>();
        }

        public void Load()
        {
            ownerRatingRepository.Load();
        }
        public List<OwnerRating> GetAll()
        {
            return ownerRatingRepository.GetAll();
        }
        public void Add(OwnerRating acci)
        {
            ownerRatingRepository.Add(acci);
        }
        public void Save()
        {
            ownerRatingRepository.Save();
        }
        public int MakeId()
        {
            return ownerRatingRepository.MakeId();
        }
    }
}

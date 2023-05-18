using booking.Injector;
using booking.Model;
using Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace application.UseCases
{
    public class Guest1RatingsService
    {
        private IGuest1RatingsRepository guest1RatingRepository;

        public Guest1RatingsService()
        {
            this.guest1RatingRepository = Injector.CreateInstance<IGuest1RatingsRepository>();
        }
        public List<Guest1Rating> GetAll()
        {
            return guest1RatingRepository.GetAll();
        }
        public void Add(Guest1Rating acci)
        {
            guest1RatingRepository.Add(acci);
        }

        public List<Guest1Rating> GetAllByGuest1Id(int guestId)
        {
            return guest1RatingRepository.GetAllByGuest1Id(guestId);
        }
    }


}


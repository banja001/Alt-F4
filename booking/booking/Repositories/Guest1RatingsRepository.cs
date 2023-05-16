using booking.Model;
using booking.Serializer;
using Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace booking.Repository
{
    public class Guest1RatingsRepository: IGuest1RatingsRepository
    {
        private List<Guest1Rating> Guest1Ratings;
        private Serializer<Guest1Rating> Serializer;

        public readonly string fileName = "../../../Resources/Data/guest1ratings.csv";
        public Guest1RatingsRepository()
        {

            Serializer = new Serializer<Guest1Rating>();
            Guest1Ratings = Serializer.FromCSV(fileName);
        }

        public List<Guest1Rating>GetAll()
        {
            return Guest1Ratings;
        }

        public void Add(Guest1Rating acci)
        {
            Guest1Ratings.Add(acci);
            Serializer.ToCSV(fileName, Guest1Ratings);
        }

        public List<Guest1Rating> GetAllByGuest1Id(int guestId)
        {
            return Guest1Ratings.Where(r => r.GuestId == guestId).ToList();
        }
    }
}

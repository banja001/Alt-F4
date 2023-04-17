using booking.Domain.RepositoryInterfaces;
using booking.Repository;
using Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace booking.Injector
{
    public class Injector
    {
        private static Dictionary<Type, object> _implementations = new Dictionary<Type, object>
        {
            {typeof(ILocationRepository), new LocationRepository()},
            {typeof(IAccommodationImageRepository), new AccommodationImageRepository()},
            {typeof(IAccommodationRepository), new AccommodationRepository()},
            {typeof(IUserRepository), new UserRepository()}
            // treba dodati
        };

        public static T CreateInstance<T>()
        {
            Type type = typeof(T);

            if (_implementations.ContainsKey(type))
            {
                return (T)_implementations[type];
            }

            throw new ArgumentException($"No implementation found for type {type}");
        }
    }
}

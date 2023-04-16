using booking.Domain.RepositoryInterfaces;
using booking.Repositories;
using booking.Repository;
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
            {typeof(IGuideRatingRepository), new GuideRatingRepository()},
            {typeof(IGuideRatingImageRepository), new GuideRatingImageRepository() }
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

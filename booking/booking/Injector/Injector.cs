using booking.Domain.RepositoryInterfaces;
using booking.Repositories;
using booking.Repository;
using Domain.RepositoryInterfaces;
using Repositories;
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
            {typeof(IGuest1NotificationsRepository), new Guest1NotificationsRepository() },
            {typeof(IOwnerNotificationRepository), new OwnerNotificationRepository() },
            {typeof(IOwnerRatingImageRepository), new OwnerRatingImageRepository() },
            {typeof(IReservationRequestsRepository), new ReservationRequestsRepository() },
            {typeof(IReservedDatesRepository), new ReservedDatesRepository() },
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

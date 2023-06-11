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
            {typeof(IGuideRatingImageRepository), new GuideRatingImageRepository() },
            {typeof(ILocationRepository), new LocationRepository()},
            {typeof(IAccommodationImageRepository), new AccommodationImageRepository()},
            {typeof(IAccommodationRepository), new AccommodationRepository()},
            {typeof(IUserRepository), new UserRepository()},
            {typeof(IGuest1RatingsRepository), new Guest1RatingsRepository() },
            {typeof(IOwnerRatingRepository), new OwnerRatingRepository() },
            {typeof(IVoucherRepository), new VoucherRepository() },
            {typeof(IAppointmentRepository), new AppointmentRepository() },
            {typeof(ITourRepository), new TourRepository() },
            {typeof(ITourAttendanceRepository), new TourAttendanceRepository() },
            {typeof(IReservationTourRepository), new ReservationTourRepository() },
            {typeof(IRenovationDatesRepository), new RenovationDatesRepository() },
            {typeof(ISimpleRequestRepository), new SimpleRequestRepository() },
            {typeof(ISimpleRequestTourRepository), new SimpleRequestTourRepository() },
            {typeof(IForumRepository), new ForumRepository() },
            {typeof(IForumCommentRepository), new ForumCommentRepository() },
            {typeof(IComplexRequestRepository), new ComplexRequestRepository() },
            {typeof(IForumNotificationRepository), new ForumNotificationRepository()  },
            {typeof(IReportedComentsRepository), new ReportedComentsRepository()  }

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

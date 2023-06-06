using booking.Model;
using booking.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using Domain.RepositoryInterfaces;
using booking.Injector;

namespace application.UseCases
{
    public class TourAttendanceService
    {
        private readonly ITourAttendanceRepository _tourAttendanceRepository;
        private readonly IReservationTourRepository _reservationTourRepository;
        public TourAttendanceService()
        {
            _tourAttendanceRepository = Injector.CreateInstance<ITourAttendanceRepository>();
            _reservationTourRepository = Injector.CreateInstance<IReservationTourRepository>();
        }
        public void Add(TourAttendance tourAttendance, Appointment appointment)
        {
            List<ReservationTour> reservationTours = _reservationTourRepository.GetAll().FindAll(r => r.User.Id == tourAttendance.Guest.Id);
            ReservationTour reservationTour = reservationTours.Find(r => r.Tour.Id == appointment.Tour.Id);
            _tourAttendanceRepository.Add(new TourAttendance(_tourAttendanceRepository.MakeID(),
                                                             reservationTour.Id,
                                                             tourAttendance.StartedCheckPoint.Id,
                                                             tourAttendance.Appeared));
        }

        public List<TourAttendance> GetAll()
        {
            return _tourAttendanceRepository.GetAll();
        }
    }
}

using booking.Model;
using booking.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace application.UseCases
{
    public class TourAttendanceService
    {
        private readonly TourAttendanceRepository _tourAttendanceRepository;
        private readonly ReservationTourRepository _reservationTourRepository;
        public TourAttendanceService()
        {
            _tourAttendanceRepository = new TourAttendanceRepository();
            _reservationTourRepository = new ReservationTourRepository();
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
    }
}

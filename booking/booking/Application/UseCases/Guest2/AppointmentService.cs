using booking.Model;
using booking.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace booking.application.UseCases.Guest2
{
    public class AppointmentService
    {
        private readonly AppointmentRepository _appointmentRepository;
        private readonly ReservationTourRepository _reservationTourRepository;
        private readonly TourAttendanceRepository _tourAttendanceRepository;
        public AppointmentService()
        {
            _appointmentRepository = new AppointmentRepository();
            _reservationTourRepository = new ReservationTourRepository();
            _tourAttendanceRepository = new TourAttendanceRepository(); 
        }
        public List<Appointment> GetCompletedAppointmentByGuest2(User guest2)
        {
            List<ReservationTour> reservedTours = _reservationTourRepository.GetAll().FindAll(r => r.User.Id == guest2.Id);
            List<Appointment> appointments = _appointmentRepository.FindAll().FindAll(a => !a.IsRated);
            List<TourAttendance> attendances = _tourAttendanceRepository.GetAll().FindAll(a => a.Guest.User.Id == guest2.Id);

            var completedAppointments = GetAllCompletedAppointments(reservedTours, appointments);

            return GetVisitedAppointments(attendances, completedAppointments);
        }
        public List<Appointment> GetAllCompletedAppointments(List<ReservationTour> reservedTours, List<Appointment> appointments)
        {
            var completedAppointments = new List<Appointment>();

            foreach (var reservedTour in reservedTours)
            {
                completedAppointments.AddRange(appointments.FindAll(a => (reservedTour.Tour.Id == a.Tour.Id) && !a.Active));
                completedAppointments = completedAppointments.Distinct().ToList();
            }
            return completedAppointments;
        }
        public List<Appointment> GetVisitedAppointments(List<TourAttendance> attendances, List<Appointment> completedAppointments)
        {
            foreach (var attendance in attendances)
            {
                Appointment visitedAppointment = completedAppointments.Find(c => c.Tour.Id == attendance.Guest.Tour.Id);
                if ((visitedAppointment != null) && attendance.Appeared)
                    continue;
                completedAppointments.Remove(visitedAppointment);
            }
            return completedAppointments;
        }
    }
}

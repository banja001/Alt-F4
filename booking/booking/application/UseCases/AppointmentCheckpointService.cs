using booking.Model;
using booking.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace application.UseCases
{
    
    public class AppointmentCheckpointService
    {
        private readonly AppointmentCheckPointRepository _appointmentCheckpointRepository;
        public AppointmentCheckpointService()
        {
            _appointmentCheckpointRepository = new AppointmentCheckPointRepository();
        }
        public AppointmentCheckPoint GetCurrentCheckpointFor(Appointment appointment)
        {
            return _appointmentCheckpointRepository.FindAll().FindLast(a => a.AppointmentId == appointment.Id && a.Active);
        }
    }
}

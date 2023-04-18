using System;
using System.Collections.Generic;
using System.Text;
using booking.Model;

namespace Domain.RepositoryInterfaces
{
    public interface IAppointmentRepository
    {
        public List<Appointment> FindAll();
        public void Save(List<Appointment> appointmentsForSave);
        public void Add(Appointment appointment);
        public int MakeID();
        public void Upadte(Appointment appointment);
    }
}

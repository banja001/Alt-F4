using booking.Model;
using booking.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.RepositoryInterfaces;

namespace booking.Repository
{
    internal class AppointmentRepository:IAppointmentRepository
    {
        private List<Appointment> appointments;
        private Serializer<Appointment> serializer;

        private readonly string fileName = "../../../Resources/Data/appointment.csv";

        public AppointmentRepository()
        {
            serializer = new Serializer<Appointment>();
            appointments = serializer.FromCSV(fileName);
        }
        public List<Appointment> FindAll()
        {
            appointments = serializer.FromCSV(fileName);
            return appointments;
        }
        public void Save(List<Appointment> appointmentsForSave)
        {
            serializer.ToCSV(fileName, appointmentsForSave);
        }
        public void Add(Appointment appointment)
        {
            appointments.Add(appointment);
            serializer.ToCSV(fileName, appointments);
        }

        public int MakeID()
        {
            if(appointments.Count!=0)
                return appointments[appointments.Count - 1].Id + 1;
            return 1;
        }
        public int FindLast()
        {
            return 1;
        }
        public void Upadte(Appointment appointment)
        {
            int idx = appointments.FindIndex(a => a.Id == appointment.Id);
            appointments[idx] = appointment;
            serializer.ToCSV(fileName, appointments);
        }
    }
}

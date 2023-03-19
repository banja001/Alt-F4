using booking.Model;
using booking.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace booking.Repository
{
    internal class AppointmentRepository
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
            return appointments;
        }
        public void Add(Appointment appointment)
        {
            appointments.Add(appointment);
            serializer.ToCSV(fileName, appointments);
        }

        public int MakeID()
        {
            return appointments[appointments.Count - 1].Id + 1;
        }
    }
}

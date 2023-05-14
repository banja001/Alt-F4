using booking.Model;
using booking.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace booking.Repository
{
    public class AppointmentCheckPointRepository
    {
        private List<AppointmentCheckPoint> appointmenCheckPoints;
        private Serializer<AppointmentCheckPoint> serializer;

        private readonly string fileName = "../../../Resources/Data/appointmentcheckpoint.csv";

        public AppointmentCheckPointRepository()
        {
            serializer = new Serializer<AppointmentCheckPoint>();
            appointmenCheckPoints = serializer.FromCSV(fileName);
        }
        public List<AppointmentCheckPoint> FindAll()
        {
            return appointmenCheckPoints;
        }
        public void Add(AppointmentCheckPoint appointmentCheckPoint)
        {
            appointmenCheckPoints.Add(appointmentCheckPoint);
            serializer.ToCSV(fileName, appointmenCheckPoints);
        }
        public void AddRange(List<AppointmentCheckPoint> appointmentCheckPointFromListBox)
        {
            appointmenCheckPoints.AddRange(appointmentCheckPointFromListBox);
            serializer.ToCSV(fileName, appointmenCheckPoints);
        }

        public int MakeID()
        {
            if (appointmenCheckPoints.Count > 0)
                return appointmenCheckPoints[appointmenCheckPoints.Count - 1].Id + 1;
            else
                return 1;
        }

        public void SaveOneInFile(AppointmentCheckPoint appointmentCheckPoint)
        {
           AppointmentCheckPoint ap= appointmenCheckPoints.Find(a => a.Id == appointmentCheckPoint.Id);
           ap.Active = appointmentCheckPoint.Active;
           ap.NotChecked=appointmentCheckPoint.NotChecked;
            serializer.ToCSV(fileName, appointmenCheckPoints);
        }
    }
}

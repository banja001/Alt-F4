using booking.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace booking.Model
{
    public class TourAttendance:ISerializable
    {
        public int Id { get; set; }
        public ReservationTour Guest { get; set; }
        public AppointmentCheckPoint StartedCheckPoint { get; set; }
        public bool Appeared { get; set; }

        public TourAttendance()
        {
            this.Guest = new ReservationTour();
            this.StartedCheckPoint = new AppointmentCheckPoint();
        }

        public TourAttendance(int id, int guestId, int checkPointId, bool appeared)
        {
            this.StartedCheckPoint = new AppointmentCheckPoint();
            this.Guest = new ReservationTour();
            this.Id = id;
            this.Guest.Id= guestId;
            this.StartedCheckPoint.Id = checkPointId;
            this.Appeared = appeared;
        }

        public void FromCSV(string[] values)
        {
            this.Id = int.Parse(values[0]);
            this.Guest.Id= int.Parse(values[1]);
            this.StartedCheckPoint.Id = int.Parse(values[2]);
            this.Appeared = bool.Parse(values[3]);
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Guest.Id.ToString(), StartedCheckPoint.Id.ToString(),Appeared.ToString() };
            return csvValues;
        }
    }
}

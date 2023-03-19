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
        public Tour Tour { get; set; }
        public User User { get; set; }
        public int NumberOfGuests { get; set; }
        public CheckPoint StartedCheckPoint { get; set; }


        public TourAttendance()
        {
            this.User = new User();
            this.Tour = new Tour();
            this.StartedCheckPoint = new CheckPoint();
        }

        public TourAttendance(int id, int tourId, int userId, int numberOfGuests,int checkPointId)
        {
            this.User = new User();
            this.Tour = new Tour();
            this.StartedCheckPoint = new CheckPoint();
            this.Id = id;
            this.Tour.Id = tourId;
            this.User.Id = userId;
            this.NumberOfGuests = numberOfGuests;
            this.StartedCheckPoint.Id = checkPointId;
        }

        public void FromCSV(string[] values)
        {
            this.Id = int.Parse(values[0]);
            this.Tour.Id = int.Parse(values[1]);
            this.User.Id = int.Parse(values[2]);
            this.NumberOfGuests = int.Parse(values[3]);
            this.StartedCheckPoint.Id = int.Parse(values[4]);
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Tour.Id.ToString(), User.Id.ToString(), NumberOfGuests.ToString(), StartedCheckPoint.Id.ToString() };
            return csvValues;
        }
    }
}

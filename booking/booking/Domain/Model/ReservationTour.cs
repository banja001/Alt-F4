using booking.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace booking.Model
{
    public class ReservationTour : ISerializable
    {
        public int Id { get; set; }
        public Tour Tour { get; set; }
        public User User { get; set; }
        public int NumberOfGuests { get; set; }

        public int AverageGuestAge { get; set; }
        public int VoucherId { get; set; }


        public ReservationTour() 
        { 
            this.User = new User();
            this.Tour = new Tour();
        }

        public ReservationTour(int id, int tourId, int userId, int numberOfGuests, int averageGuestAge, int voucherId)
        {
            this.User = new User();
            this.Tour = new Tour();

            this.Id = id;
            this.Tour.Id = tourId;
            this.User.Id = userId;
            this.NumberOfGuests = numberOfGuests;
            this.AverageGuestAge = averageGuestAge;
            this.VoucherId = voucherId;
        }   

        public void FromCSV(string[] values)
        {
            this.Id = int.Parse(values[0]);
            this.Tour.Id = int.Parse(values[1]);
            this.User.Id = int.Parse(values[2]);
            this.NumberOfGuests = int.Parse(values[3]);
            this.AverageGuestAge = int.Parse(values[4]);
            this.VoucherId = int.Parse(values[5]);
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Tour.Id.ToString(), User.Id.ToString(), NumberOfGuests.ToString(), AverageGuestAge.ToString(), VoucherId.ToString() };
            return csvValues;
        }
    }
}

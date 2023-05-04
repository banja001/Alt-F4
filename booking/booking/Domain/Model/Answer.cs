using booking.Serializer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace booking.Model
{
    public class Answer: ISerializable
    {
        public int Id { get; set; }
        public TourAttendance User { get; set; }
        public AppointmentCheckPoint AppointmentCheckPoint { get; set; }
        public bool IsChecked { get; set; }
        public bool HaveToAnswer { get; set; }

        public Answer() 
        {
            User = new TourAttendance();
            AppointmentCheckPoint = new AppointmentCheckPoint();
        }

        public Answer(int id, TourAttendance user, AppointmentCheckPoint appointmentcheckPoint, bool isChecked, bool haveToAnswer)
        {
            Id = id;
            User = user;
            AppointmentCheckPoint = appointmentcheckPoint;
            IsChecked = isChecked;
            HaveToAnswer = haveToAnswer;
        }
        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), User.Id.ToString(), AppointmentCheckPoint.Id.ToString(),IsChecked.ToString(),HaveToAnswer.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            this.Id = int.Parse(values[0]);
            this.User.Id =int.Parse( values[1]);
            this.AppointmentCheckPoint.Id = int.Parse(values[2]);
            this.IsChecked = bool.Parse(values[3]);
            this.HaveToAnswer= bool.Parse(values[4]);

        }
    }
}

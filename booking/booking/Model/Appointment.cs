using booking.Serializer;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace booking.Model
{
    internal class Appointment: ISerializable
    {
        public int Id { get; set; }
        public DateAndTime Start { get; set; }
        public DateAndTime End { get; set;}
        public Tour Tour { get; set; }
        public User Guide { get; set; }

        public Appointment(int id, DateAndTime start, DateAndTime end)
        {
            Id = id;
            Start = start;
            End = end;
        }

        public Appointment()
        {
            Tour = new Tour();
            Guide = new User();
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Start.ToString(), End.ToString(), Tour.Id.ToString(), Guide.Id.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            this.Id = int.Parse(values[0]);
            string[] dateAndTime = Convert.ToString(values[1]).Split(" ");
            Start.Date = Convert.ToDateTime(dateAndTime[0], CultureInfo.GetCultureInfo("es-ES"));
            Start.Time = dateAndTime[1];
            dateAndTime = Convert.ToString(values[2]).Split(" ");
            Start.Date = Convert.ToDateTime(dateAndTime[0], CultureInfo.GetCultureInfo("es-ES"));
            Start.Time = dateAndTime[1];
            this.Tour.Id = int.Parse(values[3]);
            this.Guide.Id= int.Parse(values[4]);
        }
        
        
    }
}

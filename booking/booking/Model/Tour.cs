using booking.Serializer;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace booking.Model
{
    public class Tour : ISerializable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Location Location { get; set; }
        public string Description { get; set; }
        public string Language { get; set; }
        public int MaxGuests { get; set; }
        public DateAndTime StartTime { get; set; }
        public double Duration { get; set; }

        public Tour()
        {
            Location = new Location();  
            StartTime = new DateAndTime();
        }

        public Tour(int id, string name, string description, string language, int maxGuests, DateAndTime startTime, double duration, Location location)
        {
            Id = id;
            Name = name;
            Description = description;
            Language = language;
            MaxGuests = maxGuests;
            StartTime = startTime;
            Duration = duration;
            Location = location;
        }

        public string[] ToCSV()
        {
            string[] csvValues = {  Id.ToString(),
                                    Name.ToString(),
                                    Location.Id.ToString(),
                                    Description.ToString(),
                                    Language.ToString(),
                                    MaxGuests.ToString(),
                                    StartTime.ToString(),
                                    Duration.ToString()
                                 };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Name = Convert.ToString(values[1]);
            Location.Id = int.Parse(values[2]);
            Description = Convert.ToString(values[3]);
            Language = Convert.ToString(values[4]);
            MaxGuests = Convert.ToInt32(values[5]);
            string[] dateAndTime = Convert.ToString(values[6]).Split(" ");
            StartTime.Date = Convert.ToDateTime(dateAndTime[0], CultureInfo.GetCultureInfo("es-ES"));
            StartTime.Time = dateAndTime[1];
            Duration = Convert.ToInt32(values[7]);
        }
    }
}

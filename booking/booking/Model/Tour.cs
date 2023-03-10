using booking.Serializer;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace booking.Model
{
    internal class Tour : ISerializable
    {
        int Id {  get; set; }
        string Name { get; set; }
        int LocationID { get; set; }
        string Description { get; set; }
        string Language { get; set; }
        int MaxGuests { get; set; }
        DateTime StartTime { get; set; }
        double Duration { get; set; }

        public Tour()
        { }

        public Tour(int id, string name, string description, string language, int maxGuests,DateTime startTime, double duration, int location)
        {
            Id = id;
            Name = name;   
            Description = description;
            Language = language;
            MaxGuests = maxGuests;
            StartTime = startTime;
            Duration = duration;
            LocationID=location;
        }

        public string[] ToCSV()
        {
            string[] csvValues = {  Id.ToString(),
                                    Name.ToString(),
                                    LocationID.ToString(), 
                                    Description.ToString(),
                                    Language.ToString(),
                                    MaxGuests.ToString(),
                                    StartTime.ToString("g", CultureInfo.GetCultureInfo("es-ES")),
                                    Duration.ToString()
                                 };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Name = Convert.ToString(values[1]);
            LocationID = Convert.ToInt32(values[2]);
            Description = Convert.ToString(values[3]);
            Language = Convert.ToString(values[4]);
            MaxGuests= Convert.ToInt32(values[5]);
            StartTime = Convert.ToDateTime(values[6], CultureInfo.GetCultureInfo("es-ES"));
            Duration = Convert.ToInt32(values[7]);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace booking.Model
{
    internal class Tour
    {
        int Id {  get; set; }
        string Name { get; set; };
        string Location { get; set; }
        string Description { get; set; }
        string Language { get; set; }
        int MaxGuests { get; set; }
        DateTime StartTime { get; set; }
        double Duration { get; set; }

        public Tour()
        { }

        public Tour(int id, string name, string description, string language, int maxGuests,DateTime startTime, double duration)
        {
            Id = id;
            Name = name;   
            Description = description;
            Language = language;
            MaxGuests = maxGuests;
            StartTime = startTime;
            Duration = duration;
        }
    }
}

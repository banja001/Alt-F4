using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xaml.Schema;

namespace booking.Model
{
    internal class Guest2
    {
        int Id { get; set; }
        int TouristCount { get; set; }
        string Language { get; set; }
        String location { get; set; }
        double duration { get; set; }
        public Guest2(int id, int touristCount, String location, double duration, string language) 
        {
            this.Id = id;
            this.TouristCount = touristCount;
            this.location = location;
            this.Language = language;
            this.duration = duration;
        }


 
    }
}

using booking.Model;
using booking.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace booking.DTO
{
    public class TourLocationDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string Language { get; set; }
        public int MaxGuests { get; set; }
        public DateTime StartTime { get; set; }
        public double Duration { get; set; }
        public List<TourImage> Images { get; set; }

        public TourLocationDTO() { }
        public TourLocationDTO(int id, string name, string description, string location, string language, int maxGuests, DateTime startTime, double duration, List<TourImage> tourImages)
        {
            this.Location = location;
            this.Name = name;
            this.Description = description;
            this.Id = id;
            this.Language = language;
            this.StartTime = startTime;
            this.Duration = duration;
            this.MaxGuests = maxGuests;
            this.Images = tourImages;
        }
    }
}

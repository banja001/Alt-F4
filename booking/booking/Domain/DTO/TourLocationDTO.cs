using booking.Model;
using booking.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace booking.DTO
{
    public class TourLocationDTO : INotifyPropertyChanged
    {
        public int Id { get; set; }
        private string name;
        private string description;
        private string location;
        private string language;
        private int maxGuests;
        private DateTime startTime;
        private double duration;
        public User Guide;
        public string Name 
        {
            get => name;
            set
            {
                if (value != name)
                {
                    name = value;
                    OnPropertyChanged("Name");
                }
            }
        }
        public string Description 
        {
            get => description;
            set
            {
                if (value != description)
                {
                    description = value;
                    OnPropertyChanged("Description");
                }
            }
        }
        public string Location 
        {
            get => location;
            set
            {
                if (value != location)
                {
                    location = value;
                    OnPropertyChanged("Location");
                }
            }
        }
        public string Language 
        {
            get => language;
            set
            {
                if (value != language)
                {
                    language = value;
                    OnPropertyChanged("Language");
                }
            }
        }
        public int MaxGuests 
        {
            get => maxGuests;
            set
            {
                if (value != maxGuests)
                {
                    maxGuests = value;
                    OnPropertyChanged("MaxGuests");
                }
            }
        }
        public DateTime StartTime 
        {
            get => startTime;
            set
            {
                if (value != startTime)
                {
                    startTime = value;
                    OnPropertyChanged("StartTime");
                }
            }
        }
        public double Duration 
        {
            get => duration;
            set
            {
                if (value != duration)
                {
                    duration = value;
                    OnPropertyChanged("Duration");
                }
            }
        }
        public List<TourImage> Images { get; set; }

        public TourLocationDTO() 
        {
            Images = new List<TourImage>();
            startTime = new DateTime();
            Guide = new User();
        }
        public TourLocationDTO(Appointment appointment, Tour tour, Location location)
        {
            Id = appointment.Tour.Id;
            Name = tour.Name;
            Description = tour.Description;
            Language = tour.Language;
            Location = location.State + "," + location.City;
            MaxGuests = tour.MaxGuests;
            Duration = tour.Duration;
            startTime = appointment.Start.Date;
            Guide.Id = appointment.Guide.Id;
        }
        public TourLocationDTO(int id, string name, string description, string location, string language, int maxGuests, DateTime startTime, double duration, List<TourImage> tourImages, int GuideId)
        {
            Guide = new User();
            this.Location = location;
            this.Name = name;
            this.Description = description;
            this.Id = id;
            this.Language = language;
            this.StartTime = startTime;
            this.Duration = duration;
            this.MaxGuests = maxGuests;
            this.Images = tourImages;
            this.Guide.Id=GuideId;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

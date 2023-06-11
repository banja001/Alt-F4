using booking.Serializer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace booking.Model
{
    public class Tour : ISerializable, INotifyPropertyChanged
    {
        private int id;
        private string name;
        private string description;
        private string language;
        private int maxGuests;
        private double duration;
        public User Guide;
        public int Id
        {
            get => id;
            set
            {
                if (value != id)
                {
                    id = value;
                    OnPropertyChanged(nameof(Id));
                }
            }
        }
        public string Name
        {
            get => name;
            set
            {
                if (value != name)
                {
                    name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }
        public Location Location { get; set; }
        public string Description
        {
            get => description;
            set
            {
                if (value != description)
                {
                    description = value;
                    OnPropertyChanged(nameof(Description));
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
                    language= value;
                    OnPropertyChanged(nameof(Language));
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
                    OnPropertyChanged(nameof(MaxGuests));
                }
            }
        }
        private DateAndTime startTime;
        public DateAndTime StartTime 
        {
            get => startTime;
            set 
            {
                if (value != startTime)
                {
                    startTime = value;
                    OnPropertyChanged(nameof(StartTime));
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
                    OnPropertyChanged(nameof(Duration));
                }
            }
        }

        public Tour()
        {
            Location = new Location();  
            StartTime = new DateAndTime();
            Guide = new User();
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
            Guide = new User();
            Guide.Id = 4;
        }
        public Tour(int id, string name, string description, string language, int maxGuests, DateAndTime startTime, double duration, Location location, User guide)
        {
            Id = id;
            Name = name;
            Description = description;
            Language = language;
            MaxGuests = maxGuests;
            StartTime = startTime;
            Duration = duration;
            Location = location;
            Guide = guide;
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
                                    Duration.ToString(),
                                    Guide.Id.ToString()
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
            Duration = Convert.ToDouble(values[7]);
            Guide.Id= int.Parse(values[8]);
        }
        public string Error => null;

        private Regex _nameRegex = new Regex("^[A-ZČĆŠĐŽa-zčćšđž -][a-zčćšđž0-9 -]*|[A-ZČĆŠĐŽa-zčćšđž -]*$");
        private Regex _numberRegex = new Regex("^(([1-9][0-9]*)|([0-9]+[.][0-9]+))$");
        private Regex _timeRegex = new Regex("^([0-1][0-9]|[0-2][0-3])[:][0-5][0-9]$");
        private Regex _locationRegex = new Regex("^[A-ZČĆŠĐŽ ][A-ZČĆŠĐŽa-zčćšđž ]*$");

        public event PropertyChangedEventHandler PropertyChanged;
        private readonly string[] _validatedProperties = { "Name", "MaxGuests", "StartTime" };

        //public bool IsValid
        //{
        //    get
        //    {
        //        foreach (var property in _validatedProperties)
        //        {
        //            if (this[property] != null)
        //                return false;
        //        }

        //        return true;
        //    }
        //}

        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
        //protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}
        //public string this[string columnName]
        //{
        //    get 
        //    {
        //        if (columnName == "Name")
        //        {
        //            if (string.IsNullOrEmpty(Name))
        //                return "*name";
        //            Match match = _nameRegex.Match(Name);
        //            if (!match.Success)
        //                return "example: Name";
        //        }
        //        else if (columnName == "MaxGuests")
        //        {
        //            if (MaxGuests == 0)
        //                return "*maxGuests";
        //            Match match = _numberRegex.Match(MaxGuests.ToString());
        //            if (!match.Success)
        //                return "example: MaxGuests";
        //        }
        //        else if (columnName == "StartTime")
        //        {
        //            if (string.IsNullOrEmpty(StartTime.Time))
        //                return "*startTime";
        //            Match match=_timeRegex.Match(StartTime.Time.ToString());
        //            if (!match.Success)
        //                return "example: StartTime.Time";
        //        }
        //        return null;
        //    }
        //}
    }
}

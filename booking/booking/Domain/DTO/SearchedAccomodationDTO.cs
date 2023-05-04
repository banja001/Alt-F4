using booking.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace booking.DTO
{
    public class SearchedAccomodationDTO : INotifyPropertyChanged
    {

        int numOfGuests;
        int numOfDays;

        public string Name { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public List<string> Type { get; set; }

        public int NumOfGuests 
        { 
            get => numOfDays;
            set
            {
                if(value != numOfDays)
                {
                    numOfDays = value;
                    OnPropertyChanged();
                }
            }
        }

        public int NumOfDays
        {
            get => numOfGuests;
            set
            {
                if (value != numOfGuests)
                {
                    numOfGuests = value;
                    OnPropertyChanged();
                }
            }
        }

        public SearchedAccomodationDTO() 
        { 
            Type = new List<string>();
        }

        public SearchedAccomodationDTO(string name, string city, string country, List<string> type, int numOfGuests, int numOfDays)
        {
            Name = name;
            City = city;
            Country = country;
            Type = type;
            NumOfGuests = numOfGuests;
            NumOfDays = numOfDays;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private Regex _numbers = new Regex("^[0-9]+$");

        public string this[string columnName]
        {
            get
            {
                if (columnName == "NumOfGuests")
                {
                    Match match = _numbers.Match(NumOfGuests.ToString());
                    if (!match.Success)
                        return "example: 1";
                }
                else if (columnName == "NumOfDays")
                {
                    Match match = _numbers.Match(NumOfDays.ToString());
                    if (!match.Success)
                        return "example: 1";
                }
                return null;
            }
        }

        private readonly string[] _validatedProperties = { "NumOfGuests", "NumOfDays" };

        public bool IsValid
        {
            get
            {
                foreach (var property in _validatedProperties)
                {
                    if (this[property] != null)
                        return false;
                }

                return true;
            }
        }
    }
}

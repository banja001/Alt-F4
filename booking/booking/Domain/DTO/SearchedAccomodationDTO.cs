using booking.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace booking.DTO
{
    public class SearchedAccomodationDTO : INotifyPropertyChanged, IDataErrorInfo
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

        public override string ToString()
        {
            return $"{NumOfGuests} {NumOfDays}";
        }

        public string Error => null;

        private Regex _numOfGuests = new Regex("^([1-9]|10)$");
        private Regex _numOfDays = new Regex("^[1-9]+$");

        public string this[string columnName]
        {
            get
            {
                if (columnName == "NumOfGuests")
                {
                    Match match = _numOfGuests.Match(NumOfGuests.ToString());

                    if (!match.Success)
                        return "format: numbers between 1 - 10";
                }
                else if (columnName == "NumOfDays")
                {
                    Match match = _numOfDays.Match(NumOfDays.ToString());
                    if (!match.Success)
                        return "format: numbers greater than 1";
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

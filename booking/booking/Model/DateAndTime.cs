using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Windows.Automation;
using System.Xml.Linq;

namespace booking.Model
{
    public class DateAndTime
    {
        public DateTime Date { get; set; }
        private string time;
        public string Time 
        {
            get => time;
            set
            {
                if (value != time)
                {
                    time = value;
                    OnPropertyChanged();
                }
            }
        }
        public DateAndTime(DateTime date, string time)
        {
            Date = date;
            Time = time;
        }
        public DateAndTime() { }

        public override string ToString()
        {
            return Date.ToString("d", CultureInfo.GetCultureInfo("es-ES"))  + " " + Time.ToString();
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly string[] _validatedProperties = { "Time" };

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
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private Regex _timeRegex = new Regex("^([0-1][0-9]|[0-2][0-3])[:][0-5][0-9]$");
        public string this[string columnName]
        {
            get
            {
                if (columnName == "Time")
                {
                    if (string.IsNullOrEmpty(Time))
                        return "*name";
                    Match match = _timeRegex.Match(Time);
                    if (!match.Success)
                        return "example: Time";
                }
                return null;
            }
        }
        public bool CheckTime()
        {
            if (!IsValid) return false;
            return true;
        }
    }
}

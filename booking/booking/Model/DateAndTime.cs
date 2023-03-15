using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace booking.Model
{
    public class DateAndTime
    {
        public DateTime Date { get; set; }
        public string Time { get; set; }
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
    }
}

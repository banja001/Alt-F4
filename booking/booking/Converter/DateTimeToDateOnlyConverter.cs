using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace booking.Converter
{
    public class DateTimeToDateOnlyConverter : MarkupExtension, IValueConverter
    {
        public DateTimeToDateOnlyConverter()
        {
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var dateOnly = (DateTime)value;
            return dateOnly.ToShortDateString();
        }

        //ConvertBack method gets called when target updates source object.
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var v = value as string;
            DateTime date = DateTime.ParseExact("1/1/1000", "dd/MM/yyyy", CultureInfo.InvariantCulture);
            if (DateTime.TryParse(v, out date))
            {
                return date;
            }
            else
            {
                return value;
            }
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}

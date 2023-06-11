using Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media.Imaging;

namespace Converter
{
    public class SimpleRequestStatusToImageSourceConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string status;
            switch (value)
            {
                case SimpleRequestStatus.APPROVED:
                    {
                        status = "../../../Resources/Icons/approved.png";
                        break;
                    }
                case SimpleRequestStatus.ON_HOLD:
                    {
                        status = "../../../Resources/Icons/onhold.png";
                        break;
                    }
                case SimpleRequestStatus.INVALID:
                    {
                        status = "../../../Resources/Icons/invalid.png";
                        break;
                    }
                default:
                    status = "../../../Resources/Icons/unknown.png";
                    break;
            }
            return new BitmapImage(new Uri(status, UriKind.Relative));
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}

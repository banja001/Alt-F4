using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace ValidationRules
{
    public class ImageValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            Uri outUri;
            
            if (Uri.TryCreate(value.ToString(), UriKind.Absolute, out outUri) && (outUri.Scheme == Uri.UriSchemeHttp || outUri.Scheme == Uri.UriSchemeHttps))
            {
                return ValidationResult.ValidResult;
            }
            return new ValidationResult(false, "Not a valid url!");

        }
    }
}

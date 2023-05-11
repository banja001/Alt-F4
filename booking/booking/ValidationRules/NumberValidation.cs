using Syncfusion.Windows.Shared;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Controls;

namespace ValidationRules
{
    public class NumberValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if(value==null) return new ValidationResult(false, "");
            if (int.TryParse(value.ToString(),out int Number))
            {
                return ValidationResult.ValidResult;
            }
            return new ValidationResult(false, "Not a valid number!");
           
        }
    }
}

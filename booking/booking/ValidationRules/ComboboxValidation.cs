using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace ValidationRules
{
    public class ComboboxValidation:ValidationRule
    {
       
public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null) return new ValidationResult(false, "");
            
            return ValidationResult.ValidResult;
            
        }
    
           
}
}

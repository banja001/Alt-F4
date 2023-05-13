using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace ValidationRules
{
    public class NameValidation : ValidationRule
    {
        private Regex NameReg = new Regex("^[a-zA-Z]+$");
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null) return new ValidationResult(false, "");
            Match match = NameReg.Match(value.ToString());
            if(match.Success)
            {
                return ValidationResult.ValidResult;
            }
            return new ValidationResult(false, "Not a valid name!");
        }
    }
}

using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace laboratory_1.sources.mvvm.util
{
    public class NumberValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            bool isInteger = int.TryParse(value as string, out var res);

            return new ValidationResult(isInteger, "Not a valid bin number");
        }
    }
}
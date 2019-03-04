using System.Windows.Controls;

namespace laboratory_1.sources.mvvm.util
{
    class DesKeyValidationRules : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            var key = value as string;
            var bitCount = key.Length * 2 * 8;

            var isCorrect = bitCount <= 64;

            return new ValidationResult(isCorrect, "key must be less than 64");

        }
    }
}

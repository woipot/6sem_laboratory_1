using System.Windows.Controls;

namespace laboratory_1.sources.mvvm.util
{
    class DesKeyValidationRules : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            var key = value as string;
            var bitCount = key.Length * 2 * 8;

            var isCorrect =key.Length != 7;

            return new ValidationResult(isCorrect, "key must have length 7");

        }
    }
}

using System.Windows.Controls;

namespace laboratory_1.sources.mvvm.util
{
    class RC4KeyValidationRules : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            var key = value as string;
            if (key != null)
            {
                var bitCount = key.Length * 2 * 8;

                var isCorrect = bitCount>=40 && bitCount <= 256;

                return new ValidationResult(isCorrect, "key must be in 40 <= x < 256");
            }

            return new ValidationResult(false, "Bad input");
        }
    }
}

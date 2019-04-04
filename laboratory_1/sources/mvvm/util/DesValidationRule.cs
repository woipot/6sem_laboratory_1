using System.Windows.Controls;

namespace laboratory_1.sources.mvvm.util
{
    public class DesValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            var key = value as string;
            if (key != null)
            {
                var input = key.Replace(" ", string.Empty).ToLower().Trim();

                var isValid = IsValidHex(input);

                var isCorrect = isValid && input.Length == 16;

                return new ValidationResult(isCorrect, "Input must be 64-bit Hexadecimal format");
            }

            return new ValidationResult(false, "Bad input");
        }

        private bool IsValidHex(string hex)
        {
            foreach (char c in hex)
            {
                if (!((c >= '0' && c <= '9') || (c >= 'a' && c <= 'f')))
                    return false;
            }
            return true;
        }
    }
}

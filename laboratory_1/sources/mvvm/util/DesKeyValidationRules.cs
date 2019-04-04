using System.Windows.Controls;

namespace laboratory_1.sources.mvvm.util
{
    class DesKeyValidationRules : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            var key = value as string;

            var isCorrect = IsValidHex(key) && key.Length == 16;

            return new ValidationResult(isCorrect, "Key must be 64-bit Hexadecimal format");

        }

        private bool IsValidHex(string hex)
        {
            foreach (char c in hex)
            {
                if (!((c >= '0' && c <= '9') || (c >= 'a' && c <= 'f') || (c >= 'A' && c <= 'F')))
                    return false;
            }
            return true;
        }
    }
}

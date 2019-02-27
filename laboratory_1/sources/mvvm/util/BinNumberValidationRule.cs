using System.Windows.Controls;

namespace laboratory_1.sources.mvvm.util
{
    public class BinNumberValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            bool canConvert = true;

            foreach (var ch in value as string)
            {
                if (ch != '1' && ch != '0')
                {
                    canConvert = false;
                    break;
                }
            }

            return new ValidationResult(canConvert, "Not a valid bin number");
        }
    }
}
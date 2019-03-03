using System;
using System.Windows.Controls;

namespace laboratory_1.sources.mvvm.util
{
    class PermutationsValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            var splitedStr = (value as string)?.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);

            var isCorrect = true;

            foreach (var s in splitedStr)
            {
                isCorrect = int.TryParse(s, out var num);
                if(!isCorrect)
                    break;
            }

            return new ValidationResult(isCorrect, "Not a valid Permutations (1 2 3 ...)");
        }
    }
}

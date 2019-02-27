using System;

namespace laboratory_1.sources.mvvm
{

    public class TabSecondModel
    {
        public string InputToMaxDivider { get; set; }

        public string MaxDivider
        {
            get
            {
                int.TryParse(InputToMaxDivider, out var res);

                if (res == 0)
                    return "∞";

                var maxPower = (int)Math.Log(res & -res, 2);

                return maxPower.ToString();
            }
        }

        public string InputToFindLimits { get; set; }
        public string Limits
        {
            get
            {
                int.TryParse(InputToFindLimits, out var res);

                if (res == 0)
                    return "∞";

                var nearestDegree = (int)Math.Log(res, 2);

                return $"2^{nearestDegree} <= x <= 2^{nearestDegree + 1}";
            }
        }
    }
}

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
    }
}

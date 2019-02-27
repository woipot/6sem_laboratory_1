using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;

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


        public string InputP { get; set; }

        public string NumP
        {
            get
            {
                var isNum = int.TryParse(InputP, out var result);
                var sbResult = new StringBuilder();

                if (isNum)
                {
                    var binForm = Convert.ToString(result, 2);

                    var appended = GetAppendedBits(result, binForm);

                    sbResult.Append(appended);
                }

                return sbResult.ToString();
            }
        }
        
        public string XorResult
        {
            get
            {
                var isNum = int.TryParse(InputP, out var result);

                if (isNum)
                {
                    var binForm = Convert.ToString(result, 2);
                    var appended = GetAppendedBits(result, binForm);

                    return XorItself(appended).ToString();
                }

                return "NaN";
            }
        }


        public int Offset { get; set; } = 1;

        public string LeftOffset
        {
            get
            {
                var isNum = int.TryParse(InputP, out var num);

                var nearestDegree = (int)Math.Log(num, 2);
                var bitCount = nearestDegree + 1;

                var maxNum = (int)Math.Pow(2, bitCount) - 1;

                var realOffset = Offset - Offset/bitCount * bitCount; 

                var res = 0;
                if (isNum)
                    res = ((num << realOffset) & (maxNum)) | (num >> (bitCount - realOffset));

                return Convert.ToString(res, 2);
            }
        }

        public string RightOffset
        {
            get
            {
                var isNum = int.TryParse(InputP, out var num);

                var nearestDegree = (int)Math.Log(num, 2);
                var bitCount = nearestDegree + 1;

                var maxNum = (int)Math.Pow(2, bitCount) - 1;

                var realOffset = Offset - Offset / bitCount * bitCount;

                var res = 0;
                if (isNum)
                    res = (num >> realOffset) | ((num << (bitCount - realOffset)) & (maxNum));

                return Convert.ToString(res, 2);
            }
        }


        private string GetAppendedBits(int num, string numBin)
        {
            var nearestDegree = (int)Math.Log(num, 2);

            var rightLimit = nearestDegree + 1;

            var additionalCount = rightLimit - numBin.Length;

            var sb = new StringBuilder();
            for (var i = 0; i < additionalCount; i++)
                sb.Append('0');

            sb.Append(numBin);
            return sb.ToString();
        }

        private bool XorItself(string binStr)
        {
            var boolList = FromString(binStr);


            if (boolList.Count == 1)
            {
                return boolList[0];
            }
            else if (boolList.Count == 2)
            {
                return boolList[0] ^ boolList[1];
            }
            else if (boolList.Count != 0)
            {
                var current = false; 
                for (var i = 0; i < boolList.Count; i++)
                {
                    if (i == 0)
                    {
                        current = boolList[i] ^ boolList[i + 1];
                        i++;
                    }
                    else
                    {
                        current = current ^ boolList[i];
                    }

                }
                return current;
            }


            return false;
        }

        private List<bool> FromString(string str)
        {
            var boolList = new List<bool>();

            foreach (var ch in str)
            {
                boolList.Add(ch == '1');
            }

            return boolList;
        }
    }
}

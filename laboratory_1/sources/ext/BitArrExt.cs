using System;
using System.Collections;

namespace laboratory_1.sources.ext
{
    public static class BitArrExt
    {
        public static int ToInt(this BitArray arr)
        {
            var res = 0;

            var i = 0; 
            foreach (bool b in arr)
            {
                if (b)
                    res += (int)Math.Pow(2, i);
                i++;
            }

            return res;
        }

        public static BitArray TruncateStart(this BitArray arr, int count)
        {
            var newLength = arr.Length - count;
            if (newLength < 0)
                throw new Exception("#Error: Bad Length");

            var res = new BitArray(newLength);

            for (var i = 0; i < newLength; i++)
            {
                res[i] = arr[i];
            }

            return res;
        }

        public static bool IsEqual(this BitArray sourceArr, BitArray secondArr)
        {
            if (sourceArr == secondArr)
                return true;

            if (sourceArr.Length != secondArr.Length)
                return false;

            for (var i = 0; i < sourceArr.Count; i++)
            {
                if (sourceArr[i] != secondArr[i])
                    return false;
            }

            return true;
        }

        public static BitArray Rotate(this BitArray arr)
        {
            var res = new BitArray(arr.Length);

            var startIndex = 0;
            for (var i = arr.Length - 1; i >= 0; i--)
            {
                res[startIndex] = arr[i];
                startIndex++;
            }

            return res;
        }
    }
}

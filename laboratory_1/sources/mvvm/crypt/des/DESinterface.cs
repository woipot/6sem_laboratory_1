using System;
using System.Text;

namespace laboratory_1.sources.mvvm.crypt.des
{
    public static class DESinterface
    {
        public static long[] startEncrypt(byte[] data, string keyStr, DESMode mode, bool decode = false)
        {
            var key = GetKey(keyStr);
            var IV = GetKey("123412");

            var blocks = SplitInputIntoBlocks(data);

            long[] result;

            switch (mode)
            {
                case DESMode.ECB:
                    result = RunEcb(blocks, key, decode);
                    break;
                case DESMode.CBC:
                    result = RunCbc(blocks, key, IV, decode);
                    break;
                default:
                    result = null;
                    break;
            }

            return result;
        }

        private static long[] RunCbc(long[] blocks, long key, long IV, bool decode = false)
        {
            var des = new DES();

            var result = decode ? 
                des.CBCDecrypt(blocks, key, IV) : des.CBCEncrypt(blocks, key, IV);

            return result;
        }

        private static long[] RunEcb(long[] blocks, long key, bool decode = false)
        {
            var des = new DES();
            var result = new long[blocks.Length];


            if (decode)
            {
                for (var i = 0; i < blocks.Length; i++)
                {
                    result[i] = des.decrypt(blocks[i], key);
                }
            }
            else
            {
                for (var i = 0; i < blocks.Length; i++)
                {
                    result[i] = des.encrypt(blocks[i], key);
                }
            }
            
            return result;
        }

        private static long[] SplitInputIntoBlocks(byte[] input)
        {
            var blocks = new long[input.Length / 8 + 1];

            for (int i = 0, j = -1; i < input.Length; i++)
            {
                if (i % 8 == 0)
                    j++;
                blocks[j] <<= 8;
                blocks[j] |= input[i];
            }

            return blocks;
        }

        private static long GetKey(string keyStr)
        {
            long key64 = 0;
            var keyBytes = Encoding.Default.GetBytes(keyStr);

            foreach (var keyByte in keyBytes)
            {
                key64 <<= 8;
                key64 |= keyByte;
            }
        
            return key64;
        }
    }

}

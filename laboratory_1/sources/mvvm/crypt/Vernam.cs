using System;

namespace laboratory_1.sources.mvvm.crypt
{
    public static class Vernam
    {
        public static byte[] GetCipher(byte[] key, byte[] data)
        {
            if (key.Length < data.Length)
            {
                throw new Exception("Small key");
            }

            var cipher = new byte[data.Length];


            for (var i = 0; i < data.Length; i++)
            {
                cipher[i] = (byte)(key[i] ^ data[i]);
            }

            return cipher;
        }
    }
}

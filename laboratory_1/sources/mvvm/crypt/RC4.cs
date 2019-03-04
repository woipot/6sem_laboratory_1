using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using laboratory_1.sources.ext;

namespace laboratory_1.sources.mvvm.crypt
{
    public class RC4
    {
        byte[] _s = new byte[256];
        int _x = 0;
        int _y = 0;

        public RC4(IReadOnlyList<byte> key)
        {
            Init(key);
        }

        public RC4(string sKey)
        {
            var byteArr = Encoding.Default.GetBytes(sKey);

            Init(byteArr);
        }

        public byte[] Encode(IEnumerable<byte> dataB, int size)
        {
            var data = dataB.Take(size).ToArray();

            var cipher = new byte[data.Length];

            for (var m = 0; m < data.Length; m++)
            {
                cipher[m] = (byte)(data[m] ^ KeyItem());
            }

            return cipher;
        }


        private void Init(IReadOnlyList<byte> key)
        {
            var keyLength = key.Count;

            for (var i = 0; i < 256; i++)
            {
                _s[i] = (byte)i;
            }

            var j = 0;
            for (var i = 0; i < 256; i++)
            {
                j = (j + _s[i] + key[i % keyLength]) % 256;
                _s.Swap(i, j);
            }
        }

        private byte KeyItem()
        {
            _x = (_x + 1) % 256;
            _y = (_y + _s[_x]) % 256;

            _s.Swap(_x, _y);

            return _s[(_s[_x] + _s[_y]) % 256];
        }

    }
}

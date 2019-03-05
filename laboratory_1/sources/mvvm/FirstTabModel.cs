using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Mvvm;

namespace laboratory_1.sources.mvvm
{
    public class FirstTabModel : BindableBase
    {
        private string _input32;
        private int _bitNum;

        private string _inputN;

        public string InputN
        {
            get => _inputN;
            set => _inputN = value;
        }

        public string Input32
        {
            get => _input32;
            set => _input32 = value;
        }

        public int BitNum
        {
            get => _bitNum;
            set => _bitNum = value;
        }

        public string SelectedBit
        {
            get
            {
                if (_bitNum <= _input32.Length && _bitNum != 0)
                    return _input32[_bitNum - 1].ToString();
                return "";
            }
        }

        public bool Checked
        {
            get
            {
                if (_bitNum <= _input32.Length && _bitNum != 0)
                    return _input32[_bitNum - 1] == '1';
                return false;
            }
        
            set
            {
                if (_bitNum <= _input32.Length && _bitNum != 0)
                { 
                    var sb = new StringBuilder(Input32);
                    sb[_bitNum - 1] = value ? '1' : '0';
                    Input32 = sb.ToString();
                }
            }
        }

        public int SwapLeft { get; set; }
        public int SwapRight { get; set; }

        public int ToZeroNum { get; set; }

        public int LeftTrim
        {
            get;
            set;
        }

        public int RightTrim
        {
            get;
            set;
        }

        public string EndsTrim
        {
            get
            {
                var sb = new StringBuilder(InputN);

                if (LeftTrim + RightTrim <= sb.Length)
                {

                    var resultSb = new StringBuilder();

                    for (var i = 0; i < LeftTrim; i++)
                        resultSb.Append(sb[i]);

                    for (var i = sb.Length - RightTrim; i < sb.Length; i++)
                        resultSb.Append(sb[i]);

                    return resultSb.ToString();
                }

                return "";
            }
        }

        public string MidleTrim
        {
            get
            {
                var sb = new StringBuilder(InputN);

                if ( sb.Length - LeftTrim - RightTrim > 0 )
                {
                    var resultSb = new StringBuilder();

                    for (var i = LeftTrim; i < sb.Length - RightTrim; i++)
                        resultSb.Append(sb[i]);

                    return resultSb.ToString();
                }

                return "";
            }
        }

        public FirstTabModel()
        {
            _bitNum = 0;
            _input32 = "";

            SwapLeft = 1;
            SwapRight = 1;
            ToZeroNum = 1;

            LeftTrim = 1;
            RightTrim = 1;
        }

        public bool Swap()
        {
            if (SwapLeft == SwapRight)
                return false;

            if (SwapLeft > _input32.Length || SwapRight > _input32.Length)
                return false;

            var sb = new StringBuilder(_input32);
            var tmpChar = sb[SwapLeft - 1];
            sb[SwapLeft - 1] = sb[SwapRight - 1];
            sb[SwapRight - 1] = tmpChar;

            _input32 = sb.ToString();

            return true;
        }

        public void ToZero()
        {
            var sb = new StringBuilder(_input32);

            var counterCopy = ToZeroNum;

            for (var i = sb.Length - 1; i >= 0 && counterCopy > 0; counterCopy--, i--)
            {
                sb[i] = '0';
            }

            Input32 = sb.ToString();
        }

        public string Number { get; set; }

        public int Left { get; set; }

        public int Right { get; set; }

        public string ResultNum => SwapBytes(Left, Right);

        public String SwapBytes(int i, int j)
        {
            try
            {
                while (Number.Length < i * 8 || Number.Length < j * 8)
                {
                    Number = Number.Insert(0, "0");
                }
                return string.Join("", Swap<String>(new List<string>(Split(Number, 8)), i - 1, j - 1));
            }
            catch (Exception)
            {
                return "#Error";
            }
        }
        private static IEnumerable<string> Split(string str, int chunkSize)
        {
            return Enumerable.Range(0, str.Length / chunkSize)
                .Select(i => str.Substring(i * chunkSize, chunkSize));
        }
        private static IEnumerable<T> Swap<T>(IList<T> list, int indexA, int indexB)
        {
            T tmp = list[indexA];
            list[indexA] = list[indexB];
            list[indexB] = tmp;
            return list;
        }
    }
}

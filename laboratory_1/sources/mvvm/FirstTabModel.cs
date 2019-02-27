using System.Text;
using Microsoft.Practices.Prism.Mvvm;

namespace laboratory_1.sources.mvvm
{
    class FirstTabModel : BindableBase
    {
        private string _input32;
        private int _bitNum;

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

        public FirstTabModel()
        {
            _bitNum = 0;
            _input32 = "";

            SwapLeft = 1;
            SwapRight = 1;
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

            for (var i = sb.Length - 1; i > 0 && counterCopy > 0; counterCopy--, i--)
            {
                sb[i] = '0';
            }

            Input32 = sb.ToString();
        }
    }
}

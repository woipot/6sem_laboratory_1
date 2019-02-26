﻿using Microsoft.Practices.Prism.Mvvm;

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

        public FirstTabModel()
        {
            _bitNum = 0;
            _input32 = "";
        }
    }
}

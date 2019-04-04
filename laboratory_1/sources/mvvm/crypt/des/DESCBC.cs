using System.Collections.Generic;
using System.Linq;

namespace laboratory_1.sources.mvvm.crypt.des
{
    public class DesCbc : DesMain 
    {
        private readonly string _iv;
        private string _cbcCipherText;
        private string _cbcDecryptText;
        private List<int[]> _block;

        public string CbcCipherText => _cbcCipherText;
        public string CbcDecryptText => _cbcDecryptText;

        public DesCbc(string key, string iv) : base(key)
        {
            this._iv = iv;
            _cbcCipherText = "";
            _cbcDecryptText = "";
            _block = new List<int[]>();
        }

        private void CbcSplit(string hex)
        {
            _block = new List<int[]>();
            int[] input = Modules.HexStringToBinArray(hex);
            for (int i = 0; i < (input.Length / 64); i++)
            {
                _block.Add(Modules.SubArray(input,i * 64, (i + 1) * 64 - 1));
            }
        }

        public void EncryptCbc(string hex)
        {
            CbcSplit(hex);
            int[] cbcRoundInput = _block[0];
            int[] iv = Modules.HexStringToBinArray(_iv);
            cbcRoundInput = Modules.Xor(cbcRoundInput, iv);
            EncryptRound(Modules.BinArrayToHex(cbcRoundInput, 16));
            _cbcCipherText += CipherText;
            for(int i = 1; i < _block.Count(); i++)
            {
                int[] prevOutput = Modules.HexStringToBinArray(CipherText);
                cbcRoundInput = Modules.Xor(_block[i], prevOutput);
                EncryptRound(Modules.BinArrayToHex(cbcRoundInput, 16));
                _cbcCipherText += CipherText;
            }
        }
        
        public void DecryptCbc(string hex)
        {
            CbcSplit(hex);
            int[] cbcRoundOutput;
            int[] iv = Modules.HexStringToBinArray(_iv);
            Decrypt(Modules.BinArrayToHex(_block[0], 16));
            cbcRoundOutput = Modules.HexStringToBinArray(DecryptText);
            cbcRoundOutput = Modules.Xor(cbcRoundOutput, iv);
            _cbcDecryptText += Modules.BinArrayToHex(cbcRoundOutput,16);
            for (int i = 1; i < _block.Count(); i++)
            {
                int[] prevInput = _block[i - 1];
                Decrypt(Modules.BinArrayToHex(_block[i], 16));
                cbcRoundOutput = Modules.HexStringToBinArray(DecryptText);
                cbcRoundOutput = Modules.Xor(cbcRoundOutput, prevInput);
                _cbcDecryptText += Modules.BinArrayToHex(cbcRoundOutput,16);
            }
        }
    }
}

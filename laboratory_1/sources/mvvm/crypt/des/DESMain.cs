using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace laboratory_1.sources.mvvm.crypt.des
{
    public class DesMain
    {
        protected DesModules Modules;
        protected string Key;
        private string _cipherText;
        private string _decryptedText;
        private int[][] _roundKeys;
        private string _traceInit;
        private string _traceFinal;
        private string[][] _traceRound;
        private string _cipherKey;
        private int[] _finalCipherArr;
        private int[] _finalDecryptedArr;

        public string CipherText => _cipherText;
        public string DecryptText => _decryptedText;
        public string TraceInit => _traceInit;
        public string TraceFinal => _traceFinal;
        public string CipherKey => _cipherKey;
        public string[][] TraceRound => _traceRound;

        public DesMain(string key)
        {
            this.Key = key;
            _cipherText = "";
            _decryptedText = "";
            _roundKeys = new int[16][];
            _traceInit = "";
            _traceFinal = "";
            _cipherKey = "";
            _traceRound = new string[16][];
        }

        public void Create()
        {
            Modules = new DesModules();
            _roundKeys = Modules.GenerateRoundKey(Modules.HexStringToBinArray(Key));
            _cipherKey = Modules.CipherKey;
        }

        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                .Where(x => x % 2 == 0)
                .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                .ToArray();
        }

        public void EncryptFile(string filePathFrom, string filePathTo)
        {
            using (var reader = new FileStream(filePathFrom, FileMode.Open))
            {
                using (var writer = new BinaryWriter(File.Open(filePathTo, FileMode.OpenOrCreate)))
                {
                    Int64 hexIn;
                    String hex = "";

                    var counter = 0;
                    for (int i = 0; (hexIn = reader.ReadByte()) != -1; i++)
                    {
                        counter++;
                        hex += $"{hexIn:X2}";
                        if (counter == 8)
                        {
                            counter = 0;
                            EncryptRound(hex);
                            writer.Write(StringToByteArray(CipherText));
                            hex = "";
                        }
                    }

                    if (counter != 8 && counter != 0)
                    {
                        for(var i = counter; i < 8; i++)
                            hex += $"{(byte)0:X2}";
                        EncryptRound(hex);
                        writer.Write(StringToByteArray(CipherText));
                    }
                }
            }
        }

        public void EncryptRound(string hexString)
        {
            int[] binArray = Modules.HexStringToBinArray(hexString);
            EncryptRound(binArray);
        }

        public void EncryptRound(int[] binArray)
        {
            
            Modules.InitialPermutation(ref binArray);
            _traceInit = Modules.BinArrayToHex(binArray,0);
            
            int[] left = Modules.SubArray(binArray, 0, 31);
            int[] right = Modules.SubArray(binArray, 32, 63);
            
            for(int i = 0; i < 16; i++)
            {
                int[] outputF = new int[32];

                Modules.Function(right, ref outputF, _roundKeys[i]);

                left = Modules.Xor(outputF, left);

                if (i < 15)
                    Modules.Swap(ref left, ref right);

                _traceRound[i] = new string[]{Modules.BinArrayToHex(left,8),
                                             Modules.BinArrayToHex(right,8),
                                             Modules.BinArrayToHex(_roundKeys[i],12)};
            }
            
            int[] final = new int[64];
            left.CopyTo(final, 0);
            right.CopyTo(final, 32);
            _traceFinal = Modules.BinArrayToHex(final,0);

            Modules.FinalPermutation(ref final);

            _finalCipherArr = final;
            _cipherText = Modules.BinArrayToHex(final,0);
        }

        public void DecryptFile(string filePathFrom, string filePathTo)
        {
            using (var reader = new FileStream(filePathFrom, FileMode.Open))
            {
                using (var writer = new BinaryWriter(File.Open(filePathTo, FileMode.OpenOrCreate)))
                {
                    Int64 hexIn;
                    String hex = "";

                    var counter = 0;
                    for (int i = 0; (hexIn = reader.ReadByte()) != -1; i++)
                    {
                        counter++;
                        hex += $"{hexIn:X2}";
                        if (counter == 8)
                        {
                            counter = 0;
                            Decrypt(hex);
                            writer.Write(StringToByteArray(DecryptText));
                            hex = "";
                        }
                    }

                    if (counter != 8 && counter != 0)
                    {
                        for (var i = counter; i < 8; i++)
                            hex += $"{(byte)0:X2}";
                        Decrypt(hex);
                        writer.Write(StringToByteArray(DecryptText));
                    }
                }
            }
        }




        public void Decrypt(string hexString)
        {
            int[] binArray = Modules.HexStringToBinArray(hexString);
            Decrypt(binArray);
        }


        public void Decrypt(int[] binArray)
        {
            Modules.InitialPermutation(ref binArray);
            _traceInit = Modules.BinArrayToHex(binArray, 0);

            int[] left = Modules.SubArray(binArray, 0, 31);
            int[] right = Modules.SubArray(binArray, 32, 63);
            for (int i = 15; i >= 0; i--)
            {
                int[] outputF = new int[32];
                Modules.Function(right, ref outputF, _roundKeys[i]);
                left = Modules.Xor(outputF, left);

                if (i > 0)
                    Modules.Swap(ref left, ref right);

                _traceRound[Math.Abs(i-15)] = new string[]{Modules.BinArrayToHex(left,8),
                                             Modules.BinArrayToHex(right,8),
                                             Modules.BinArrayToHex(_roundKeys[i],12)};
            }

            int[] final = new int[64];
            left.CopyTo(final, 0);
            right.CopyTo(final, 32);
            _traceFinal = Modules.BinArrayToHex(final, 0);

            Modules.FinalPermutation(ref final);
            _finalDecryptedArr = final;

            _decryptedText = Modules.BinArrayToHex(final,0);
        }
    }
}

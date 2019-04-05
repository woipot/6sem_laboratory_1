using System;
using System.IO;

namespace laboratory_1.sources.mvvm.crypt.des
{
    public class SmartDesCbc : DesMain
    {
        private string _iv;

        private string _lastEncrypted = "";

        public SmartDesCbc(string key, string iv) : base(key)
        {
            _iv = iv;
        }

        public new void EncryptFile(string fromFile, string toFile)
        {
            using (var reader = new FileStream(fromFile, FileMode.Open))
            {
                using (var writer = new BinaryWriter(File.Open(toFile, FileMode.OpenOrCreate)))
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
                            CbcEncryptRound(hex);
                            writer.Write(StringToByteArray(CipherText));
                            hex = "";
                        }
                    }

                    if (counter != 8 && counter != 0)
                    {
                        for (var i = counter; i < 8; i++)
                            hex += $"{(byte)0:X2}";
                        CbcEncryptRound(hex);
                        writer.Write(StringToByteArray(CipherText));
                    }
                }
            }
        }

        private void CbcEncryptRound(string hex)
        {
        //    if (string.IsNullOrEmpty(CipherText))
        //    {
        //        _cipherText = _iv;
        //    }

            var input = Convert.ToInt64(hex, 16) ^ Convert.ToInt64(_iv, 16);
            EncryptRound($"{input:X16}");

            _iv = CipherText;
        }

        public void DecodeFile(string fromFile, string toFile)
        {
            using (var reader = new FileStream(fromFile, FileMode.Open))
            {
                using (var writer = new BinaryWriter(File.Open(toFile, FileMode.OpenOrCreate)))
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
                            CbcDecodeRound(hex);
                            writer.Write(StringToByteArray(DecryptText));
                            hex = "";
                        }
                    }

                    if (counter != 8 && counter != 0)
                    {
                        for (var i = counter; i < 8; i++)
                            hex += $"{(byte)0:X2}";
                        CbcDecodeRound(hex);
                        writer.Write(StringToByteArray(DecryptText));
                    }
                }
            }
        }

        private void CbcDecodeRound(string hex)
        {
            if (string.IsNullOrEmpty(_lastEncrypted))
            {
                _lastEncrypted = _iv;
            }

            Decrypt(hex);

            var output = Convert.ToInt64(_lastEncrypted, 16) ^ Convert.ToInt64(DecryptText, 16);
            _lastEncrypted = hex;
            _decryptedText = $"{output:X16}";

        }
    }
}

using System;
using System.IO;

namespace laboratory_1.sources.mvvm.crypt.des
{
    public class DesCFB : Des
    {
        private string _iv;

        private string _lastEncrypted = "";

        public DesCFB(string key, string iv) : base(key)
        {
            _iv = iv;
        }

        public void EncryptFile(string fromFile, string toFile)
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
                            CfbEncryptRound(hex);
                            writer.Write(StringToByteArray(CipherText));
                            hex = "";
                        }
                    }

                    if (counter != 8 && counter != 0)
                    {
                        for (var i = counter; i < 8; i++)
                            hex += $"{(byte)0:X2}";
                        CfbEncryptRound(hex);
                        writer.Write(StringToByteArray(CipherText));
                    }
                }
            }
        }

        private void CfbEncryptRound(string hex)
        {
            EncryptRound(_iv);

            var output = Convert.ToInt64(hex, 16) ^ Convert.ToInt64(CipherText, 16);

            _cipherText = $"{output:X16}";
            _iv = _cipherText;
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
                            CfbDecodeRound(hex);
                            writer.Write(StringToByteArray(DecryptText));
                            hex = "";
                        }
                    }

                    if (counter != 8 && counter != 0)
                    {
                        for (var i = counter; i < 8; i++)
                            hex += $"{(byte)0:X2}";
                        CfbDecodeRound(hex);
                        writer.Write(StringToByteArray(DecryptText));
                    }
                }
            }
        }

        private void CfbDecodeRound(string hex)
        {
            EncryptRound(_iv);

            var output = Convert.ToInt64(hex, 16) ^ Convert.ToInt64(CipherText, 16);

            _decryptedText = $"{output:X16}";
            _iv = hex;
        }
    }
}

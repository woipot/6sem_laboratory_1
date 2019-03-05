using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using laboratory_1.sources.mvvm.crypt;
using laboratory_1.sources.mvvm.crypt.des;

namespace laboratory_1.sources.mvvm
{
    public class TabThirdModel
    {
        public static int RC4BlockSize = 1024;

        public void MyEncrypt(string filePatch)
        {
            try
            {
                using (var reader = new BinaryReader(File.Open(filePatch, FileMode.Open)))
                {
                    using (var writer = new BinaryWriter(File.Open(filePatch + "tmp", FileMode.OpenOrCreate)))
                    {
                        var byteList = new List<byte>();
                        while (true)
                        {
                            try
                            {
                                var area = reader.ReadByte();
                                var newByte = MyCipher.EncryptByte(area);
                                byteList.Add(newByte);
                            }
                            catch (EndOfStreamException e)
                            {
                                break;
                            }
                        }
                        writer.Write(byteList.ToArray());
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            File.Delete(filePatch);
            File.Move(filePatch + "tmp", filePatch);
        }

        public void MyDecrypt(string filePatch)
        {
            try
            {
                using (var reader = new BinaryReader(File.Open(filePatch, FileMode.Open)))
                {
                    using (var writer = new BinaryWriter(File.Open(filePatch + "tmp", FileMode.OpenOrCreate)))
                    {
                        var byteList = new List<byte>();
                        while (true)
                        {
                            try
                            {
                                var area = reader.ReadByte();
                                var newByte = MyCipher.DecryptByte(area);
                                byteList.Add(newByte);
                            }
                            catch (EndOfStreamException e)
                            {
                                break;
                            }
                        }
                        writer.Write(byteList.ToArray());
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            File.Delete(filePatch);
            File.Move(filePatch + "tmp", filePatch);
        }


        public string RC4Key { get; set; } = "";

        public void RC4(string filePatch)
        {
            var encoder = new RC4(RC4Key);
           
            try
            {
                using (var reader = new BinaryReader(File.Open(filePatch, FileMode.Open)))
                {
                    using (var writer = new BinaryWriter(File.Open(filePatch + "tmp", FileMode.OpenOrCreate)))
                    {
                        while (true)
                        {
                            var bytes = reader.ReadBytes(RC4BlockSize);
                            if(bytes.Length == 0)
                                break;
                            
                            var result = encoder.Encode(bytes, bytes.Length);
                            writer.Write(result);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            File.Delete(filePatch);
            File.Move(filePatch + "tmp", filePatch);
        }


        public string VernamKey { get; set; } = "";

        public void StartVernam(string filePatch)
        {
            try
            {
                using (var reader = new BinaryReader(File.Open(filePatch, FileMode.Open)))
                {
                    using (var writer = new BinaryWriter(File.Open(filePatch + "tmp", FileMode.OpenOrCreate)))
                    {
                        var key = Encoding.Default.GetBytes(VernamKey);
                        var blockSize = key.Length;
                        while (true)
                        {
                            var bytes = reader.ReadBytes(blockSize);
                            if (bytes.Length == 0)
                                break;

                            var result = Vernam.GetCipher(key, bytes);
                            writer.Write(result);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            File.Delete(filePatch);
            File.Move(filePatch + "tmp", filePatch);
        }


        public string DESKey { get; set; }

        public bool ECBMode { get; set; } = true;

        public bool CBCMode { get; set; }

        public void DESEncode(string filePatch, bool decode = false)
        {
            try
            {
                using (var writer = new BinaryWriter(File.Open(filePatch + "tmp", FileMode.OpenOrCreate)))
                {
                    var mod = ECBMode ? DESMode.ECB : DESMode.CBC;

                    var area = File.ReadAllBytes(filePatch);
                    var newBytes = DESinterface.startEncrypt(area, DESKey, mod, decode);
                    foreach (var b in newBytes)
                    {
                        writer.Write(b);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            File.Delete(filePatch);
            File.Move(filePatch + "tmp", filePatch);
        }
    }
}

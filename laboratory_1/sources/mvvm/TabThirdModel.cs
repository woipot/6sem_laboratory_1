using System;
using System.IO;
using System.Text;
using laboratory_1.sources.mvvm.crypt;

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
                        while (true)
                        {
                            try
                            {
                                var area = reader.ReadByte();
                                var newByte = MyCrypt.EncryptByte(area);
                                writer.Write(newByte);
                            }
                            catch (EndOfStreamException e)
                            {
                                break;
                            }
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

        public void MyDecrypt(string filePatch)
        {
            try
            {
                using (var reader = new BinaryReader(File.Open(filePatch, FileMode.Open)))
                {
                    using (var writer = new BinaryWriter(File.Open(filePatch + "tmp", FileMode.OpenOrCreate)))
                    {
                        while (true)
                        {
                            try
                            {
                                var area = reader.ReadByte();
                                var newByte = MyCrypt.DecryptByte(area);
                                writer.Write(newByte);
                            }
                            catch (EndOfStreamException e)
                            {
                                break;
                            }
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
    }
}

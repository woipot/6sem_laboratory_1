using System;
using System.IO;
using laboratory_1.sources.mvvm.crypt;

namespace laboratory_1.sources.mvvm
{
    public class TabThirdModel
    {
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

    }
}

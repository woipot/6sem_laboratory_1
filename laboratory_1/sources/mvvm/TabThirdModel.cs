using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using laboratory_1.sources.mvvm.crypt;
using laboratory_1.sources.mvvm.crypt.des;
using laboratory_1.sources.mvvm.util;

namespace laboratory_1.sources.mvvm
{
    public class TabThirdModel
    {
        public static int RC4BlockSize = 1024;


        public void MyEncrypt(string filePath)
        {
            try
            {
                using (var reader = new BinaryReader(File.Open(filePath, FileMode.Open)))
                {
                    using (var writer = new BinaryWriter(File.Open(filePath + "tmp", FileMode.OpenOrCreate)))
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

            File.Delete(filePath);
            File.Move(filePath + "tmp", filePath);
        }

        public void MyDecrypt(string filePath)
        {
            try
            {
                using (var reader = new BinaryReader(File.Open(filePath, FileMode.Open)))
                {
                    using (var writer = new BinaryWriter(File.Open(filePath + "tmp", FileMode.OpenOrCreate)))
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

            File.Delete(filePath);
            File.Move(filePath + "tmp", filePath);
        }


        public string RC4Key { get; set; } = "";

        public void RC4(string filePath)
        {
            var encoder = new RC4(RC4Key);
           
            try
            {
                using (var reader = new BinaryReader(File.Open(filePath, FileMode.Open)))
                {
                    using (var writer = new BinaryWriter(File.Open(filePath + "tmp", FileMode.OpenOrCreate)))
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

            File.Delete(filePath);
            File.Move(filePath + "tmp", filePath);
        }


        public string VernamKey { get; set; } = "";

        public void StartVernam(string filePath)
        {
            try
            {
                using (var reader = new BinaryReader(File.Open(filePath, FileMode.Open)))
                {
                    using (var writer = new BinaryWriter(File.Open(filePath + "tmp", FileMode.OpenOrCreate)))
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

            File.Delete(filePath);
            File.Move(filePath + "tmp", filePath);
        }


        public string DESKey { get; set; } = "AABB09182736CCDD";

        public string DESIV { get; set; } = "AABB09182736CCDD";

        public bool ECBMode { get; set; } = true;

        public bool CBCMode { get; set; }

        public bool CFBMode { get; set; }

        public bool OFBMode { get; set; }

        public void DESEncode(string filePath, bool decode = false)
        {
            try
            {
                if (decode)
                {
                    Decrypt(filePath);
                    
                }
                else
                {
                    Encrypt(filePath);
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }


        public void Encrypt(string fileName)
        {
            if (ECBMode)
            {
                var des = new Des(DESKey);
                des.Create();
                des.EncryptFile(fileName, fileName + "destmp");
            }else if (CBCMode)
            {
                var des = new DesCbc(DESKey, DESIV);
                des.Create();
                des.EncryptFile(fileName, fileName + "destmp");
            }
            else if (CFBMode)
            {
                var des = new DesCFB(DESKey, DESIV);
                des.Create();
                des.EncryptFile(fileName, fileName + "destmp");
            }
            else if (OFBMode)
            {
                var des = new DesOFB(DESKey, DESIV);
                des.Create();
                des.EncryptFile(fileName, fileName + "destmp");
            }

            File.Delete(fileName);
            File.Move(fileName + "destmp", fileName);
        }

        public void Decrypt(string fileName)
        {
            if (ECBMode)
            {
                var des = new Des(DESKey);
                des.Create();
                des.DecryptFile(fileName, fileName + "destmp");
            }
            else if (CBCMode)
            {
                var des = new DesCbc(DESKey, DESIV);
                des.Create();
                des.DecodeFile(fileName, fileName + "destmp");
            }
            else if (CFBMode)
            {
                var des = new DesCFB(DESKey, DESIV);
                des.Create();
                des.DecodeFile(fileName, fileName + "destmp");
            }
            else if (OFBMode)
            {
                var des = new DesOFB(DESKey, DESIV);
                des.Create();
                des.DecodeFile(fileName, fileName + "destmp");
            }
            File.Delete(fileName);
            File.Move(fileName + "destmp", fileName);
        }
    }

}

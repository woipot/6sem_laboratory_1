using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cryptography.Algorithms.Symmetric;
using laboratory_1.sources.mvvm.crypt;
using laboratory_1.sources.mvvm.crypt.des;

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


        public string DESKey { get; set; }

        public bool ECBMode { get; set; } = true;

        public bool CBCMode { get; set; }

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
            var des = new DES
            {
                Key = Encoding.Default.GetBytes(DESKey)
            };
            using (var reader = new BinaryReader(new FileStream(fileName, FileMode.Open, FileAccess.Read), Encoding.Default))
            {
                if (reader.BaseStream.Length == 0)
                {
                    return;
                }
                var countOfBytesToDeleteAtDecryption = (int)(8 - reader.BaseStream.Length % 8) % 8;
                var countOfBytesToDeleteAtDecryptionBytes = BitConverter.GetBytes((ulong)countOfBytesToDeleteAtDecryption);
                using (var writer = new BinaryWriter(new FileStream(fileName+"tmp", FileMode.Create, FileAccess.Write), Encoding.Default))
                {
                    writer.Write(des.Encrypt(countOfBytesToDeleteAtDecryptionBytes));
                    while (true)
                    {
                        var readBytes = reader.ReadBytes(128);
                        if (readBytes.Length == 0)
                        {
                            break;
                        }
                        if (readBytes.Length != 128)
                        {
                            Array.Resize(ref readBytes, readBytes.Length + countOfBytesToDeleteAtDecryption);
                        }
                        var blocks = Enumerable.Range(0, readBytes.Length / 8).Select((i) => readBytes.Skip(i * 8).Take(8).ToArray()).ToArray();
                        var encryptedBlocks = new byte[blocks.Length][];
                        Parallel.For(0, blocks.Length, (i) =>
                        {
                            encryptedBlocks[i] = des.Encrypt(blocks[i]);
                        });
                        foreach (var encryptedBlock in encryptedBlocks)
                        {
                            writer.Write(encryptedBlock);
                        }
                        var cipherOperationProgress = (int)((float)reader.BaseStream.Position / reader.BaseStream.Length * 100);
                    }
                }
            }
            File.Delete(fileName);
            File.Move(fileName + "tmp", fileName);
        }

        public void Decrypt(string fileName)
        {
            var des = new DES()
            {
                Key = Encoding.Default.GetBytes(DESKey)
            };
            using (var reader = new BinaryReader(new FileStream(fileName, FileMode.Open, FileAccess.Read), Encoding.Default))
            {
                if (reader.BaseStream.Length % 8 != 0 || reader.BaseStream.Length == 8)
                {
                    return ;
                }
                using (var writer = new BinaryWriter(new FileStream(fileName + "tmp", FileMode.Create, FileAccess.Write), Encoding.Default))
                {
                    if (reader.BaseStream.Length == 0)
                    {
                        return ;
                    }
                    var countOfBytesToDelete = BitConverter.ToUInt64(des.Decrypt(reader.ReadBytes(8)), 0);
                    if (countOfBytesToDelete >= 8)
                    {
                        countOfBytesToDelete = 0;
                    }
                    while (true)
                    {
                        var readBytes = reader.ReadBytes(1024);
                        if (readBytes.Length == 0)
                        {
                            break;
                        }
                        var blocks = Enumerable.Range(0, readBytes.Length / 8).Select((i) => readBytes.Skip(i * 8).Take(8).ToArray()).ToArray();
                        var decryptedBlocks = new byte[blocks.Length][];
                        Parallel.For(0, blocks.Length, (i) =>
                        {
                            decryptedBlocks[i] = des.Decrypt(blocks[i]);
                        });
                        if (reader.BaseStream.Position == reader.BaseStream.Length && countOfBytesToDelete != 0)
                        {
                            Array.Resize(ref decryptedBlocks[decryptedBlocks.Length - 1], (int)(8 - countOfBytesToDelete));
                        }
                        foreach (var decryptedBlock in decryptedBlocks)
                        {
                            writer.Write(decryptedBlock);
                        }
                    }
                }
            }
            File.Delete(fileName);
            File.Move(fileName+"tmp", fileName);
                    
        }
    }

}

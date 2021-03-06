﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using laboratory_1.sources.ext;

namespace laboratory_1.sources.mvvm.crypt
{
    public class MyCipher : IEncryption
    {
        private int _currentProgress = 0;
        private static List<BitArray> _myCryptArr = new List<BitArray>
        {
            new BitArray(new []{false, false, true, true}),
            new BitArray(new []{true, false, false, true}),
            new BitArray(new []{true, true, true, true}),
            new BitArray(new []{true, true, true, false}),
          
            new BitArray(new []{false, true, true, false}),
            new BitArray(new []{false, true, false, false}) ,
            new BitArray(new []{true, false, true, false} ),
            new BitArray(new []{true, true, false, true} ),
           
            new BitArray(new []{false, true, false, true}),
            new BitArray(new []{false, false, true, false}),
            new BitArray(new []{true, false, false, false}),
            new BitArray(new []{true, true, false, false}  ),
      
            new BitArray(new []{true, false, true, true}  ),
            new BitArray(new []{false, false, false, true} ),
            new BitArray(new []{false, false, false, false}),
            new BitArray(new []{false, true, true, true}   )
        };

        public static byte EncryptByte(byte b)
        {
            var bitArr = new BitArray(new[] { b });

            var maxWord = new BitArray(new []{bitArr[4], bitArr[5],
                bitArr[6], bitArr[7]});

            var minWord = new BitArray(new[]{bitArr[0], bitArr[1],
                bitArr[2], bitArr[3]});

            var newMaxWord = GetEncrypted(maxWord);
            var newMinWord = GetEncrypted(minWord);

            var resultArr = new BitArray(new[]{ newMinWord[0], newMinWord[1], newMinWord[2], newMinWord[3], newMaxWord[0], newMaxWord[1], newMaxWord[2], newMaxWord[3] });

            return (byte)resultArr.ToInt();
        }

        private static BitArray GetEncrypted(BitArray inputArr)
        {
            var num = inputArr.ToInt();
            return _myCryptArr[num].Rotate();
        }

        private static BitArray GetDecrypted(BitArray inputArr)
        {
            //var counter = _myCryptArr.TakeWhile(bitArray => bitArray != inputArr).Count();

            var counter = (byte)0;
            foreach (var bitArray in _myCryptArr)
            {
                if(bitArray.IsEqual(inputArr.Rotate()))
                    break;
                counter++;
            }

            return new BitArray(new[]{counter}).TruncateStart(4);
        }

        public static byte DecryptByte(byte b)
        {
            var bitArr = new BitArray(new[] { b });

            var maxWord = new BitArray(new[]{bitArr[4], bitArr[5],
                bitArr[6], bitArr[7]});

            var minWord = new BitArray(new[]{bitArr[0], bitArr[1],
                bitArr[2], bitArr[3]});

            var newMaxWord = GetDecrypted(maxWord);
            var newMinWord = GetDecrypted(minWord);

            var resultArr = new BitArray(new[] { newMinWord[0], newMinWord[1], newMinWord[2], newMinWord[3], newMaxWord[0], newMaxWord[1], newMaxWord[2], newMaxWord[3] });

            return (byte)resultArr.ToInt();
        }


        public void Decrypt(string fromFile, string toFile)
        {
            _currentProgress = 0;
            try
            {
                using (var reader = new BinaryReader(File.Open(fromFile, FileMode.Open)))
                {
                    using (var writer = new BinaryWriter(File.Open(toFile + "tmp", FileMode.OpenOrCreate)))
                    {
                        var byteList = new List<byte>();
                        while (true)
                        {
                            try
                            {
                                var area = reader.ReadByte();
                                var newByte = DecryptByte(area);
                                _currentProgress++;
                                ProgreeUpdated?.Invoke(this, _currentProgress);
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
                File.Delete(fromFile);
                File.Move(toFile, fromFile);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void Encrypt(string fromFile, string toFile)
        {
            _currentProgress = 0;
            try
            {
                using (var reader = new BinaryReader(File.Open(fromFile, FileMode.Open)))
                {
                    using (var writer = new BinaryWriter(File.Open(toFile, FileMode.OpenOrCreate)))
                    {
                        var byteList = new List<byte>();
                        while (true)
                        {
                            try
                            {
                                var area = reader.ReadByte();
                                var newByte = MyCipher.EncryptByte(area);
                                byteList.Add(newByte);
                                _currentProgress++;
                                ProgreeUpdated?.Invoke(this, _currentProgress);
                            }
                            catch (EndOfStreamException e)
                            {
                                break;
                            }
                        }
                        writer.Write(byteList.ToArray());
                    }
                }
                File.Delete(fromFile);
                File.Move(toFile, fromFile);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

        public long GetCurrentProgressBytes()
        {
            return _currentProgress;
        }

        public long GetMaximum(string fileName)
        {
            return  new FileInfo(fileName).Length;
        }

        public event EventHandler<int> ProgreeUpdated;
    }
}

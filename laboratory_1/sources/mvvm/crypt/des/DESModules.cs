using System;
using System.Collections.Generic;
using System.Linq;

namespace laboratory_1.sources.mvvm.crypt.des
{
    public class DesModules
    {
        private string _cipherKey;
        public string CipherKey => _cipherKey;

        public bool[] Xor(bool[] arg1, bool[] arg2)
        {
            bool[] re = new bool[arg1.Length];
            for (int i = 0; i < arg1.Length; i++)
            {
                if (arg1[i] ^ arg2[i])
                    re[i] = true;
                else
                    re[i] = false;
            }

            return re;
        }

        private string HexStringToBinString(string hex)
        {
            string result = "";
            result = string.Join(string.Empty, hex.Select(h =>
                Convert.ToString(
                    Convert.ToInt32(h.ToString(), 16), 2
                ).PadLeft(4, '0')));
            return result;
        }

        public bool[] BinStringToBinArray(string bin)
        {
            List<bool> result = new List<bool>();
            for (int i = 0; i < bin.Length; i++)
            {
                result.Add(bin[i] != '0');
            }

            return result.ToArray();
        }

        public bool[] HexStringToBinArray(string hex)
        {
            return BinStringToBinArray(HexStringToBinString(hex));
        }

        public string BinArrayToHex(bool[] bin, int padding)
        {
            string re = string.Empty;
            for (int i = 0; i < bin.Length; i++)
            {
                re += Convert.ToInt16(bin[i]);
            }

            if (padding == 0)
            {
                return Convert.ToUInt64(re, 2).ToString("X16");
            }
            else
            {
                return Convert.ToUInt64(re, 2).ToString($"X{padding}");
            }

            //{
            //    return Convert.ToUInt64(re, 2).ToString("X8");
            //}
            //if(padding == 12)
            //{
            //    return Convert.ToUInt64(re, 2).ToString("X12");
            //}
            //if(padding == 14)
            //{
            //    return Convert.ToUInt64(re, 2).ToString("X14");
            //}
            //else
            //{
            //    return Convert.ToUInt64(re, 2).ToString("X16");
            //}
        }

        public bool[] SubArray(bool[] src, int start, int end)
        {
            bool[] temp = new bool[end - start + 1];
            int index = 0;
            for (int i = start; i <= end; i++)
            {
                temp[index] = src[i];
                index++;
            }

            return temp;
        }

        public bool[][] GenerateRoundKey(bool[] keys)
        {
            bool[][] result = new bool[16][];
            bool[] tempkey = new bool[56];
            bool[] leftTempKey = new bool[28];
            bool[] rightTempKey = new bool[28];
            bool[] roundkey;
            Permute(keys, ref tempkey, DesConstants.ParityBitDrop);
            _cipherKey = BinArrayToHex(tempkey, 14);
            for (int i = 0; i < 16; i++)
            {
                roundkey = new bool[48];
                leftTempKey = SubArray(tempkey, 0, 27);
                rightTempKey = SubArray(tempkey, 28, 55);
                ShiftLeft(ref leftTempKey, DesConstants.ScheduleBitShift[i]);
                ShiftLeft(ref rightTempKey, DesConstants.ScheduleBitShift[i]);
                tempkey = new bool[56];
                leftTempKey.CopyTo(tempkey, 0);
                rightTempKey.CopyTo(tempkey, 28);
                Permute(tempkey, ref roundkey, DesConstants.KeyCompressionTable);
                result[i] = roundkey;
            }

            return result;
        }

        public void ShiftLeft(ref bool[] block, int numberOfShifts)
        {
            bool temp;
            for (int i = 0; i < numberOfShifts; i++)
            {
                temp = block[0];
                for (int j = 1; j < block.Length; j++)
                {
                    block[j - 1] = block[j];
                }

                block[block.Length - 1] = temp;
            }
        }

        public void Swap(ref bool[] leftBlock, ref bool[] rightBlock)
        {
            bool[] temp = leftBlock;
            leftBlock = rightBlock;
            rightBlock = temp;
        }

        public void Permute(bool[] input, ref bool[] output, int[] ptable)
        {
            for (int i = 0; i < ptable.Length; i++)
            {
                output[i] = input[ptable[i] - 1];
            }
        }

        private void SubstituteRound(ref bool[] inBlock, int[,] sbox)
        {
            int temp = 3;
            int row = 0;
            int column = 0;
            for (int i = 1; i <= 4; i++)
            {
                column += Convert.ToInt16(inBlock[i]) * (int)Math.Pow(2, temp);
                temp--;
            }

            row = 2 * Convert.ToInt16(inBlock[0]) + Convert.ToInt16(inBlock[5]);
            inBlock = HexStringToBinArray(Convert.ToString(sbox[row, column], 16));
        }

        private void Substitude(bool[] inputBlock, ref bool[] outputBlock)
        {
            bool[] temp;
            bool[] result = new bool[32];
            int[][,] sboxs = new int[][,]
            {
                DesConstants.Sbox1, DesConstants.Sbox2, DesConstants.Sbox3, DesConstants.Sbox4,
                DesConstants.Sbox5, DesConstants.Sbox6, DesConstants.Sbox7, DesConstants.Sbox8
            };
            for (int i = 0; i < 8; i++)
            {
                temp = SubArray(inputBlock, i * 6, (i + 1) * 6 - 1);
                SubstituteRound(ref temp, sboxs[i]);
                temp.CopyTo(result, i * 4);
            }

            outputBlock = result;
        }

        public void InitialPermutation(ref bool[] input)
        {
            bool[] temp = new bool[64];
            Permute(input, ref temp, DesConstants.InitialPermutation);
            input = temp;
        }

        public void FinalPermutation(ref bool[] input)
        {
            bool[] temp = new bool[64];
            Permute(input, ref temp, DesConstants.FinalPermutation);
            input = temp;
        }

        public void Function(bool[] rightInput, ref bool[] output, bool[] roundKey)
        {
            bool[] temp = new bool[48];
            bool[] temp2 = new bool[32];
            bool[] temp3 = new bool[32];
            Permute(rightInput, ref temp, DesConstants.ExpansionPermutation);
            temp = Xor(temp, roundKey);
            Substitude(temp, ref temp2);
            Permute(temp2, ref temp3, DesConstants.StraightPermutation);
            output = temp3;
        }
    }
}
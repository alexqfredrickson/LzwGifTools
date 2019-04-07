using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace LzwGifTools
{
    /// <summary>
    /// Unpacks a code stream (variable-width decoding).
    /// </summary>
    public class StreamUnpacker
    {
        byte LzwMinimumCodeSize { get; set; }
        int ClearCode { get; set; }
        int EndOfInformationCode { get; set; }

        public List<int> Unpack(List<byte> packedBytes)
        {
            List<int> codeStream = new List<int>();

            List<byte> tempPackedBytes = new List<byte>();
            tempPackedBytes.AddRange(packedBytes);

            List<bool> bitStream = new List<bool>();

            int currentCodeWidth = LzwMinimumCodeSize + 1;
            int codeWidthIncreaseThreshold = (int)Math.Pow(2, currentCodeWidth) - 1;

            for (int i = 0; i < tempPackedBytes.Count; i++)
            {
                tempPackedBytes[i] = tempPackedBytes[i].ReverseByte(); // big endian 
                List<bool> bits = GlobalUtilities.ConvertByteToBits(tempPackedBytes[i], 8);
                bits.Reverse();
                bitStream.AddRange(bits);
            }

            for (int i = 0; i < bitStream.Count; i += 0)
            {
                if (codeStream.Count >= codeWidthIncreaseThreshold)
                {
                    currentCodeWidth++;
                    codeWidthIncreaseThreshold = (int)Math.Pow(2, currentCodeWidth) - 1;
                }

                if (i + currentCodeWidth < bitStream.Count)
                {
                    List<bool> chunk = bitStream.GetRange(i, currentCodeWidth);
                    chunk.Reverse();
                    int code = GlobalUtilities.ConvertBitsToInt(chunk);
                    codeStream.Add(code);
                }

                i += currentCodeWidth;
            }

            return codeStream;
        }

        public StreamUnpacker(byte lzwMinimumCodeSize)
        {
            LzwMinimumCodeSize = lzwMinimumCodeSize;
            ClearCode = (int)Math.Pow(2, LzwMinimumCodeSize);
            EndOfInformationCode = ClearCode + 1;
        }
    }
}
namespace LzwGifTools
{
    /// <summary>
    /// Packs a code stream (variable-width encoding).
    /// </summary>
    public class StreamPacker
    {
        byte LzwMinimumCodeSize { get; set; }

        public List<byte> Pack(List<int> codeStream)
        {
            List<byte> packedBytes = new();

            List<bool> bits = new();

            int currentCodeWidth = LzwMinimumCodeSize + 1;
            int codeCount = 0;
            int codeWidthIncreaseThreshold = (int)Math.Pow(2, currentCodeWidth) - 1;

            foreach (int code in codeStream)
            {
                if (codeCount >= codeWidthIncreaseThreshold)
                {
                    currentCodeWidth++;
                    codeWidthIncreaseThreshold = (int)Math.Pow(2, currentCodeWidth) - 1;
                }

                List<bool> codeBits = GlobalUtilities.ConvertIntToBits(code, currentCodeWidth);
                bits.AddRange(codeBits);

                codeCount++;
            }

            int zeroesNeeded = 8 - (bits.Count % 8);

            for (int i = 0; i < zeroesNeeded; i++)
            {
                bits.Add(false);
            }

            for (int i = 0; i < bits.Count; i += 8)
            {
                List<bool> reversedByte = bits.GetRange(i, 8);
                reversedByte.Reverse();
                packedBytes.Add(GlobalUtilities.ConvertBitsToByte(reversedByte.ToArray()));
            }

            return packedBytes;
        }

        public StreamPacker(byte lzwMinimumCodeSize)
        {
            LzwMinimumCodeSize = lzwMinimumCodeSize;
        }
    }
}
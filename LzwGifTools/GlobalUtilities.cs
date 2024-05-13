namespace LzwGifTools
{
    public static class GlobalUtilities
    {
        public static List<bool> ConvertByteToBits(byte i, int arrayLength)
        {
            List<bool> bits = new List<bool>();

            int temp = i;

            while (temp > 0)
            {
                if ((temp & 1) == 1)
                {
                    bits.Add(true);
                }
                else
                {
                    bits.Add(false);
                }

                temp >>= 1;
            }

            while (bits.Count < arrayLength)
            {
                bits.Add(false);
            }

            return bits;
        }

        public static int ConvertBitsToInt(List<bool> bits)
        {
            int result = 0;

            int index = 0; // todo this is broken

            foreach (bool b in bits)
            {
                if (b)
                {
                    result |= 1 << (bits.Count - index - 1);
                }

                index++;
            }

            return result;
        }

        public static List<bool> ConvertIntToBits(int i, int arrayLength)
        {
            List<bool> bits = new List<bool>();

            int temp = i;

            while (temp > 0)
            {
                if ((temp & 1) == 1)
                {
                    bits.Add(true);
                }
                else
                {
                    bits.Add(false);
                }

                temp >>= 1;
            }

            while (bits.Count < arrayLength)
            {
                bits.Add(false);
            }

            return bits;
        }

        public static byte ConvertBitsToByte(bool[] bits)
        {
            byte result = 0;

            int index = 8 - bits.Length;

            foreach (bool b in bits)
            {
                if (b)
                {
                    result |= (byte)(1 << (7 - index));
                }

                index++;
            }

            return result;
        }
    }
}

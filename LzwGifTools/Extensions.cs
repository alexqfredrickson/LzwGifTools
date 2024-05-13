namespace LzwGifTools
{
    public static class Extensions
    {
        public static byte GetBitsCount(this int i)
        {
            byte count = 0;

            while (i >> 1 > 0)
            {
                i >>= 1;
                count++;
            }

            return count;
        }

        public static byte ReverseByte(this byte val)
        {
            byte result = 0x00;

            for (byte mask = 0x80; Convert.ToInt32(mask) > 0; mask >>= 1)
            {
                // shift right current result
                result = (byte)(result >> 1);

                // tempbyte = 1 if there is a 1 in the current position
                var tempbyte = (byte)(val & mask);
                if (tempbyte != 0x00)
                {
                    // Insert a 1 in the left
                    result = (byte)(result | 0x80);
                }
            }

            return result;
        }
    }
}

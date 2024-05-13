namespace LzwGifTools
{
    /// <summary>
    /// Unpacks and decompresses a code stream.
    /// </summary>
    public class Decoder
    {
        LzwDecompressor LzwDecompressor { get; set; }
        StreamUnpacker StreamUnpacker { get; set; }

        public List<int> Decode(List<byte> packedBytes)
        {
            List<int> codeStream = StreamUnpacker.Unpack(packedBytes);
            return LzwDecompressor.Decompress(codeStream);
        }

        public Decoder(byte lzwMinimumCodeSize)
        {
            LzwDecompressor = new LzwDecompressor(lzwMinimumCodeSize);
            StreamUnpacker = new StreamUnpacker(lzwMinimumCodeSize);
        }
    }
}

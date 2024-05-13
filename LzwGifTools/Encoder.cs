namespace LzwGifTools
{
    /// <summary>
    /// Compresses and packs an index stream.
    /// </summary>
    public class Encoder
    {
        LzwCompressor LzwCompressor { get; set; }
        StreamPacker StreamPacker { get; set;}

        public List<byte> Encode(List<int> indexStream)
        {
            List<int> codeStream = LzwCompressor.Compress(indexStream);
            return StreamPacker.Pack(codeStream);
        }

        public Encoder(byte lzwMinimumCodeSize)
        {
            LzwCompressor = new LzwCompressor(lzwMinimumCodeSize);
            StreamPacker = new StreamPacker(lzwMinimumCodeSize);
        }
    }
}

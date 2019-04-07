using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LzwGifTools
{
    /// <summary>
    /// Unpacks and decompresses a code stream.
    /// </summary>
    public class Decoder
    {
        LzwDecompressor lzwDecompressor { get; set; }
        StreamUnpacker streamUnpacker { get; set; }

        public List<int> Decode(List<byte> packedBytes)
        {
            List<int> codeStream = streamUnpacker.Unpack(packedBytes);
            return lzwDecompressor.Decompress(codeStream);
        }

        public Decoder(byte lzwMinimumCodeSize)
        {
            lzwDecompressor = new LzwDecompressor(lzwMinimumCodeSize);
            streamUnpacker = new StreamUnpacker(lzwMinimumCodeSize);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LzwGifTools
{
    /// <summary>
    /// Compresses and packs an index stream.
    /// </summary>
    public class Encoder
    {
        LzwCompressor lzwCompressor { get; set; }
        StreamPacker streamPacker { get; set;}

        public List<byte> Encode(List<int> indexStream)
        {
            List<int> codeStream = lzwCompressor.Compress(indexStream);
            return streamPacker.Pack(codeStream);
        }

        public Encoder(byte lzwMinimumCodeSize)
        {
            lzwCompressor = new LzwCompressor(lzwMinimumCodeSize);
            streamPacker = new StreamPacker(lzwMinimumCodeSize);
        }
    }
}

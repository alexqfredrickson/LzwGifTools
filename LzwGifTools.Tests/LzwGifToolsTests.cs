using NUnit.Framework;

namespace LzwGifTools.Tests
{
    public class LzwGifToolsTestCase
    {
        public byte LzwMinimumCodeSize;
        public Dictionary<int, List<int>> GlobalColorTableExample = new Dictionary<int, List<int>>();
        public List<int> LzwCompressedCodeStream = new List<int>();
        public List<int> CodeStream = new List<int>();  // not lzw compressed; the codestream represents a simple bitmap with three colors 
        public List<byte> PackedBytes = new List<byte>();

        public LzwGifToolsTestCase() { }
    }

    /// <summary>
    /// The test data and sample code table here are graciously borrowed from http://giflib.sourceforge.net/whatsinagif/lzw_image_data.html.
    /// </summary>
    [TestFixture]
    public class LzwGifToolsTests
    {
        public static IEnumerable<LzwGifToolsTestCase> LzwGifToolsTestCases
        {
            get
            {
                yield return new LzwGifToolsTestCase()
                {
                    LzwMinimumCodeSize = 8,
                    GlobalColorTableExample = new Dictionary<int, List<int>>()
                    {
                        {0, new List<int>() { 0,   0,   255 }}, // an example of the global color table used below
                        {1, new List<int>() { 255, 0,   0   }},
                        {2, new List<int>() { 0,   255, 0   }},
                        {3, new List<int>() { 255, 255, 255 }}
                    },
                    PackedBytes = new List<byte>() // this is how GIMP compresses/encodes the same bitmap
                    { 
                        0x00, 0x03, 0x08, 0x14, 0x08, 0xa0, 0x60, 0xc1, // 00000000 00000011 00001000 00010100 00001000 10100000 01100000 11000001 
                        0x81, 0x04, 0x0d, 0x02, 0x40, 0x18, 0x40, 0xe1, // 10000001 00000100 00001101 00000010 01000000 00011000 01000000 11100001 
                        0x42, 0x81, 0x02, 0x22, 0x0a, 0x30, 0x38, 0x50, // 01000010 10000001 00000010 00100010 00001010 00110000 00111000 01010000 
                        0xe2, 0x44, 0x87, 0x16, 0x07, 0x1a, 0xcc, 0x98, // 11100010 01000100 10000111 00010110 00000111 00011010 11001100 10011000 
                        0x90, 0x22, 0x42, 0x87, 0x0c, 0x41, 0x22, 0x0c, // 10010000 00100010 01000010 10000111 00001100 01000001 00100010 00001100 
                        0x08                                            // 00001000 
                    },
                    CodeStream = new List<int>() 
                    { 
                         1,1,1,1,1,0,0,0,0,0, //  R R R R R B B B B B   
                         1,1,1,1,1,0,0,0,0,0, //  R R R R R B B B B B   
                         1,1,1,1,1,0,0,0,0,0, //  R R R R R B B B B B  
                         1,1,1,2,2,2,2,0,0,0, //  R R R G G G G B B B   
                         1,1,1,2,2,2,2,0,0,0, //  R R R G G G G B B B  
                         0,0,0,2,2,2,2,1,1,1, //  B B B G G G G R R R   
                         0,0,0,2,2,2,2,1,1,1, //  B B B G G G G R R R
                         0,0,0,0,0,1,1,1,1,1, //  B B B B B R R R R R
                         0,0,0,0,0,1,1,1,1,1, //  B B B B B R R R R R
                         0,0,0,0,0,1,1,1,1,1  //  B B B B B R R R R R
                    },
                    LzwCompressedCodeStream = new List<int>()
                    {
                        256,    1,      258,   258,     0,      261,    261,    259,
                        260,    262,    0,     264,     1,      266,    267,    258,
                        2,      273,    2,     262,     259,    274,    275,    270,
                        278,    259,    262,   281,     265,    276,    264,    270,
                        268,    288,    264,   257
                    }
                };
            }
        }
                
        #region Test Methods

        [Test]
        [TestCaseSource("LzwGifToolsTestCases")]
        public void AllMethodsWork(LzwGifToolsTestCase testCase)
        {
            LzwCompressor compressor = new LzwCompressor(testCase.LzwMinimumCodeSize);
            StreamPacker packer = new StreamPacker(testCase.LzwMinimumCodeSize);
            StreamUnpacker unpacker = new StreamUnpacker(testCase.LzwMinimumCodeSize);
            LzwDecompressor decompressor = new LzwDecompressor(testCase.LzwMinimumCodeSize);

            List<int> lzwEncodedCodeStream = compressor.Compress(testCase.CodeStream);
            List<byte> packedBytes = packer.Pack(lzwEncodedCodeStream);
            List<int> lzwEncodedCodeStream2 = unpacker.Unpack(packedBytes);
            List<int> codeStream = decompressor.Decompress(lzwEncodedCodeStream2);
            CollectionAssert.AreEqual(testCase.CodeStream, codeStream);
        }

        [Test]
        [TestCaseSource("LzwGifToolsTestCases")]
        public void LzwEncodingIsReversible(LzwGifToolsTestCase testCase) // and decoding
        {
            LzwCompressor compressor = new LzwCompressor(testCase.LzwMinimumCodeSize);
            LzwDecompressor decompressor = new LzwDecompressor(testCase.LzwMinimumCodeSize);

            List<int> lzwCompressedStream = compressor.Compress(testCase.CodeStream);
            List<int> lzwDecompressedStream = decompressor.Decompress(lzwCompressedStream);

            CollectionAssert.AreEqual(testCase.CodeStream, lzwDecompressedStream);
        }

        [Test]
        [TestCaseSource("LzwGifToolsTestCases")]
        public void EncodingWorks(LzwGifToolsTestCase testCase)
        {
            Encoder encoder = new Encoder(testCase.LzwMinimumCodeSize);
            List<byte> packedBytes = encoder.Encode(testCase.CodeStream);
            CollectionAssert.AreEqual(testCase.PackedBytes, packedBytes);
        }

        [Test]
        [TestCaseSource("LzwGifToolsTestCases")]
        public void DecodingWorks(LzwGifToolsTestCase testCase)
        {
            Decoder decoder = new Decoder(testCase.LzwMinimumCodeSize);
            List<int> codeStream = decoder.Decode(testCase.PackedBytes);
            CollectionAssert.AreEqual(testCase.CodeStream, codeStream);
        }

        [Test]
        [TestCaseSource("LzwGifToolsTestCases")]
        public void StreamPackingIsReversible(LzwGifToolsTestCase testCase)
        {
            StreamUnpacker unpacker = new StreamUnpacker(testCase.LzwMinimumCodeSize);
            StreamPacker packer = new StreamPacker(testCase.LzwMinimumCodeSize);

            List<int> lzwCompressedCodeStream = unpacker.Unpack(testCase.PackedBytes);
            List<byte> packedBytes = new List<byte>();
            packedBytes = packer.Pack(lzwCompressedCodeStream);

            CollectionAssert.AreEqual(testCase.PackedBytes, packedBytes);
        }

        [Test]
        [TestCaseSource("LzwGifToolsTestCases")]
        public void PackingWorks(LzwGifToolsTestCase testCase)
        {
            StreamPacker packer = new StreamPacker(testCase.LzwMinimumCodeSize);
            List<byte> packedBytes = packer.Pack(testCase.LzwCompressedCodeStream);
            CollectionAssert.AreEqual(testCase.PackedBytes, packedBytes);
        }

        [Test]
        [TestCaseSource("LzwGifToolsTestCases")]
        public void UnpackingWorks(LzwGifToolsTestCase testCase)
        {
            StreamUnpacker unpacker = new StreamUnpacker(testCase.LzwMinimumCodeSize);
            List<int> lzwCompressedCodeStream = unpacker.Unpack(testCase.PackedBytes);
            CollectionAssert.AreEqual(testCase.LzwCompressedCodeStream, lzwCompressedCodeStream);
        }

        [Test]
        [TestCaseSource("LzwGifToolsTestCases")]
        public void ExportLzwCompressedCodeStreamToTextFile(LzwGifToolsTestCase testCase)
        {
            LzwCompressor compressor = new LzwCompressor(testCase.LzwMinimumCodeSize);
            List<int> lzwCompressedStream = compressor.Compress(testCase.CodeStream);

            using (StreamWriter sw = File.CreateText(Config.WorkingDirectory + @"/" + "lzw-compressed-code-stream.txt"))
            {
                int index = 0;

                foreach (int i in lzwCompressedStream)
                {
                    index++;

                    sw.Write(string.Format("{0} ", i));

                    if (index % 8 == 0)
                    {
                        sw.WriteLine("");
                    }

                }
            }
        }

        public void ExportBytesToTextFileAsBinaryString(List<byte> bytes)
        {
            string binaryString = string.Concat(bytes.Select(b => Convert.ToString(b, 2).PadLeft(8, '0')));

            using (StreamWriter writer = new StreamWriter(Config.WorkingDirectory + "binary-string-packed-bytes.txt"))
            {
                for (int i = 0; i < binaryString.Length; i += 8)
                {
                    if (i % 64 == 0)
                    {
                        writer.WriteLine("");
                    }

                    writer.Write(string.Format("{0} ", binaryString.Substring(i, 8)));
                }
            }
        }

        #endregion

        #region Utility Methods

        public void ExportBytesToTextFile(List<byte> bytes)
        {
            File.WriteAllBytes(Config.WorkingDirectory + @"/" + "packed-bytes.txt", bytes.ToArray());
        }

        public void ExportBytestreamToTextFileAsBinaryString(List<byte> bytes)
        {
            using (StreamWriter sw = File.CreateText(Config.WorkingDirectory + @"/" + "packed-bytes-binary.txt"))
            {
                int index = 0;

                foreach (byte b in bytes)
                {
                    index++;

                    sw.Write(string.Format("{0} ", Convert.ToString(b, 2).PadLeft(8, '0')));

                    if (index % 8 == 0)
                    {
                        sw.WriteLine("");
                    }

                }
            }
        }

        /// <summary>
        /// Saves a string of hex values to a text file at the specified path.
        /// </summary>
        /// <param name="hex">A string of hex values (e.g. 02 16 8C 2D 99 87 2A 1C).</param>
        /// <param name="path">A file system path.</param>
        public static void ExportHextoTextFile(string hexVals, string filePath)
        {
            hexVals = hexVals.Replace(" ", "");

            byte[] bytes = Enumerable.Range(0, hexVals.Length)
                .Where(x => x % 2 == 0)
                .Select(x => Convert.ToByte(hexVals.Substring(x, 2), 16))
                .ToArray();

            File.WriteAllBytes(filePath, bytes);
        }

        #endregion
    }
}
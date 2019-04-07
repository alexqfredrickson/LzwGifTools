using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LzwGifTools
{
    /// <summary>
    /// Decompresses a code stream.
    /// </summary>
    public class LzwDecompressor
    {
        Dictionary<int, List<int>> CodeTable = new Dictionary<int, List<int>>();

        int ClearCode { get; set; }
        int EndOfInformationCode { get; set; }
        byte LzwMinimumCodeSize { get; set; }

        public List<int> Decompress(List<int> codeStream)
        {
            InitializeCodeTable();

            List<int> indexStream = new List<int>() { };

            int index = 1; // codeStream[0] is assumed to be a Clear code.
            indexStream.AddRange(CodeTable[index]);

            while (index + 1 < codeStream.Count - 1) // codeStream[codeStream.Count() - 1] is assumed to be an End of Information code.
            {
                if (CodeTable.Keys.Contains(codeStream[index + 1]))
                {
                    indexStream.AddRange(CodeTable[codeStream[index + 1]]);
                    int k = CodeTable[codeStream[index + 1]][0];
                    CodeTable.Add(CodeTable.Count, CodeTable[codeStream[index]].Concat(new List<int>() { k }).ToList());
                }
                else
                {
                    int k = CodeTable[codeStream[index]][0];
                    indexStream.AddRange(CodeTable[codeStream[index]].Concat(new List<int>() { k }).ToList());
                    CodeTable.Add(CodeTable.Count, CodeTable[codeStream[index]].Concat(new List<int>() { k }).ToList());
                }

                index++;
            }

            return indexStream;
        }

        private void InitializeCodeTable()
        {
            int lzwMaximumCodeSize = (int)Math.Pow(2, LzwMinimumCodeSize);

            for (int i = 0; i < lzwMaximumCodeSize; i++)
            {
                CodeTable.Add(i, new List<int>() { i });
            }

            ClearCode = lzwMaximumCodeSize;
            CodeTable.Add(lzwMaximumCodeSize, new List<int>() { lzwMaximumCodeSize });

            EndOfInformationCode = lzwMaximumCodeSize + 1;
            CodeTable.Add(lzwMaximumCodeSize + 1, new List<int>() { lzwMaximumCodeSize + 1 });
        }

        public LzwDecompressor(byte lzwMinimumCodeSize)
        {
            LzwMinimumCodeSize = lzwMinimumCodeSize;
        }
    }
}

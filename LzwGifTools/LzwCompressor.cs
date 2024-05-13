namespace LzwGifTools
{
    /// <summary>
    /// Compresses an index stream.
    /// </summary>
    public class LzwCompressor
    {
        Dictionary<int, List<int>> CodeTable = new Dictionary<int, List<int>>();

        int ClearCode { get; set; }
        int EndOfInformationCode { get; set; }

        byte LzwMinimumCodeSize { get; set; }

        public List<int> Compress(List<int> indexStream)
        {
            InitializeCodeTable();

            List<int> codeStream = new List<int>() { ClearCode };

            int index = 0;
            List<int> buffer = new List<int>() { indexStream[index] };

            while (index + 1 < indexStream.Count)
            {
                List<int> k = new List<int>() { indexStream[index + 1] };
                List<int> bufferPlusK = buffer.Concat(k).ToList();

                if (CodeTable.Values.Any(list => list.SequenceEqual(bufferPlusK)))
                {
                    buffer = bufferPlusK;
                }
                else
                {
                    CodeTable.Add(CodeTable.Count(), bufferPlusK);
                    codeStream.Add(GetCodeTableCodeByValue(buffer));
                    buffer = k;
                }

                index++;
            }

            codeStream.Add(GetCodeTableCodeByValue(buffer));
            codeStream.Add(EndOfInformationCode);

            return codeStream;
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

        public int GetCodeTableCodeByValue(List<int> val)
        {
            int result = 0;

            foreach (int key in CodeTable.Keys)
            {
                if (CodeTable[key].SequenceEqual(val))
                {
                    result = key;
                    break;
                }
            }

            return result;
        }

        public LzwCompressor(byte lzwMinimumCodeSize)
        {
            LzwMinimumCodeSize = lzwMinimumCodeSize;
        }
    }
}

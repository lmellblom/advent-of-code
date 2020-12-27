using System;
using System.Collections.Generic;
using System.Linq;

namespace adventOfCode
{
    public class December9 : AoCSolver
    {
        public December9() : base(9)
        {
        }

        public override bool Test()
        {
            string filename = GetTestFilename();
            List<long> XMASdata = System.IO.File.ReadAllLines(filename).Select(long.Parse).ToList();
            long firstNr = FirstNumberNotSumOfPreviousPairs(XMASdata, 5);
            bool testSucceeded = firstNr == 127;
            return testSucceeded;
        }

        public override string First()
        {
            string filename = GetFilename();
            List<long> XMASdata = System.IO.File.ReadAllLines(filename).Select(long.Parse).ToList();
            long firstNr = FirstNumberNotSumOfPreviousPairs(XMASdata, 25);
            return firstNr.ToString();
        }

        public override bool Test2()
        {
            string filename = GetTestFilename();
            List<long> XMASdata = System.IO.File.ReadAllLines(filename).Select(long.Parse).ToList();
            long nrToSearchFor = FirstNumberNotSumOfPreviousPairs(XMASdata, 5);

            // use the previous data as the input to the next puzzle
            long result = FindEncryptionWeakness(nrToSearchFor, XMASdata);
            bool testSucceeded = result == 62;
            return testSucceeded;
        }

        public override string Second()
        {
            string filename = GetFilename();
            List<long> XMASdata = System.IO.File.ReadAllLines(filename).Select(long.Parse).ToList();
            long nrToSearchFor = FirstNumberNotSumOfPreviousPairs(XMASdata, 25);

            // use the previous data as the input to the next puzzle
            long weakness = FindEncryptionWeakness(nrToSearchFor, XMASdata);
            return weakness.ToString();
        }

        private long FirstNumberNotSumOfPreviousPairs(List<long> XMASdata, int preamble)
        {
            for (int i = preamble; i < XMASdata.Count; i++)
            {
                var input = XMASdata.GetRange(i - preamble, preamble);
                long nrToFind = XMASdata[i];
                if (!ContainsSum(nrToFind, input))
                {
                    return nrToFind;
                }
            }
            return -1;
        }

        private bool ContainsSum(long sum, IList<long> input)
        {
            for (int x = 0; x < input.Count; x++)
            {
                for (int y = x + 1; y < input.Count; y++)
                {
                    if (input[x] + input[y] == sum)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public long FindEncryptionWeakness(long nrToFind, List<long> XMASdata)
        {
            for (int start = 0; start < XMASdata.Count; start++)
            {
                for (int end = start + 1; end < XMASdata.Count; end++)
                {
                    List<long> currentRange = XMASdata.GetRange(start, end - start);
                    long currentRangeSum = currentRange.Sum();
                    if (currentRangeSum == nrToFind)
                    {
                        return currentRange.Min() + currentRange.Max();
                    }
                    if (currentRangeSum > nrToFind)
                    {
                        break; // no need to continue adding
                    }
                }
            }
            return -1;
        }
    }
}

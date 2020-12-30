using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace adventOfCode.Y2020.D01
{
    [CodeName("Report Repair")]
    public class AoC : IAdventOfCodeWithTest
    {
        protected const int FOUND_NUMBER = 2020;

        public Result First(List<string> input)
        {
            var intInput = input.Select(Int32.Parse).ToList();
            int foundSum = GetPairsResult(intInput);
            return new Result(foundSum);
        }

        public Result Second(List<string> input)
        {
            var intInput = input.Select(Int32.Parse).ToList();
            int foundSum = GetTripletsResult(intInput);
            return new Result(foundSum);
        }

        public TestResult Test(List<string> input)
        {
            var intInput = input.Select(Int32.Parse).ToList();
            int foundSum = GetPairsResult(intInput);
            var expected = 514579;
            bool succeded = foundSum == expected;
            return new TestResult(succeded, expected, foundSum);
        }

        public TestResult Test2(List<string> input)
        {
            var intInput = input.Select(Int32.Parse).ToList();
            int foundSum = GetTripletsResult(intInput);
            var expected = 241861950;
            bool succeded = foundSum == expected;
            return new TestResult(succeded, expected, foundSum);
        }

        private int GetPairsResult(IList<int> input)
        {
            for (int x = 0; x < input.Count; x++)
            {
                for (int y = x + 1; y < input.Count; y++)
                {
                    if (input[x] + input[y] == FOUND_NUMBER)
                    {
                        return input[x] * input[y];
                    }
                }
            }

            return -1;
        }

        private int GetTripletsResult(IList<int> input)
        {
            for (int x = 0; x < input.Count; x++)
            {
                for (int y = x + 1; y < input.Count; y++)
                {
                    for (int z = y + 1; z < input.Count; z++)
                    {
                        if (input[x] + input[y] + input[z] == FOUND_NUMBER)
                        {
                            return input[x] * input[y] * input[z];
                        }
                    }
                }
            }
            return -1;
        }
    }
}
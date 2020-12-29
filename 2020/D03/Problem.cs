using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace adventOfCode.Y2020.D03 
{
    [CodeName("Toboggan Trajectory")]      
    public class AoC : IAdventOfCode 
    {
        public Result First(List<string> input) 
        {
            int foundTrees = CountNumberOfTrees(input.ToArray(), (3, 1));
            return new Result(foundTrees);
        }

        public Result Second(List<string> input) 
        {
            var sum = SumSeveralTrees(input.ToArray());
            return new Result(sum);
        }

        public TestResult Test(List<string> input) 
        {
            int foundTrees = CountNumberOfTrees(input.ToArray(), (3, 1));
            var expected = 7;
            bool succeded = foundTrees == expected;
            return new TestResult(succeded, expected, foundTrees);
        }

        public TestResult Test2(List<string> input) 
        {
            var sum = SumSeveralTrees(input.ToArray());
            var expected = 336;
            bool succeded = sum == expected;
            return new TestResult(succeded, expected, sum);
        }

        private int CountNumberOfTrees(string[] input, (int right, int down) traverse)
        {
            int rowIndex = 0; // | - number of list
            int colIndex = 0; // - check letter

            int foundTrees = 0;
            var isTree = '#';

            // until the end
            rowIndex += traverse.down;
            colIndex += traverse.right;
            while (rowIndex < input.Length)
            {
                var row = input[rowIndex]; // all letters
                if (colIndex >= row.Length)
                {
                    colIndex -= row.Length;
                }
                var letter = row[colIndex];
                foundTrees += (letter == isTree) ? 1 : 0;
                rowIndex += traverse.down;
                colIndex += traverse.right;
            }

            return foundTrees;
        }

        private long SumSeveralTrees(string[] input)
        {
            List<(int right, int down)> traverse = new List<(int right, int down)>()
            {
                (1,1),
                (3,1),
                (5,1),
                (7,1),
                (1,2)
            };

            var allResults = new List<long>();
            foreach (var item in traverse)
            {
                long foundTrees = CountNumberOfTrees(input, item);
                allResults.Add(foundTrees);
            }

            var sum = allResults.Aggregate((a, y) => a * y);
            return sum;
        }
    }
}
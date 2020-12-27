using System;
using System.Collections.Generic;
using System.Linq;

namespace adventOfCode
{
    public class December3 : AoCSolver
    {
        public December3() : base(3)
        {
        }

        public override bool Test()
        {
            string fileName = GetTestFilename();
            int foundTrees = CountNumberOfTrees(fileName, 3, 1);
            bool testSucceeded = foundTrees == 7;
            return testSucceeded;
        }

        public override string First()
        {
            string fileName = GetFilename();
            int foundTrees = CountNumberOfTrees(fileName, 3, 1);
            return foundTrees.ToString();
        }

        public override bool Test2()
        {
            string fileName = GetTestFilename();
            var sum = SumSeveralTrees(fileName);
            bool testSucceeded = sum == 336;
            return testSucceeded;
        }

        public override string Second()
        {
            string fileName = GetFilename();
            var sum = SumSeveralTrees(fileName);
            return sum.ToString();
        }

        private int CountNumberOfTrees(string inputfile, int right, int down)
        {
            string[] input = System.IO.File.ReadAllLines(inputfile).ToArray();

            int rowIndex = 0; // | - number of list
            int colIndex = 0; // - check letter

            int foundTrees = 0;
            var isTree = '#';

            // until the end
            rowIndex += down;
            colIndex += right;
            while (rowIndex < input.Count())
            {
                var row = input[rowIndex]; // all letters
                if (colIndex >= row.Count())
                {
                    colIndex -= row.Count();
                }
                var letter = row[colIndex];
                foundTrees += (letter == isTree) ? 1 : 0;
                rowIndex += down;
                colIndex += right;
            }

            return foundTrees;
        }
        private long SumSeveralTrees(string fileName)
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
                long foundTrees = CountNumberOfTrees(fileName, item.right, item.down);
                allResults.Add(foundTrees);
            }

            var sum = allResults.Aggregate((a, y) => a * y);
            return sum;
        }
    }
}

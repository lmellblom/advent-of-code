using System;
using System.Collections.Generic;
using System.Linq;

namespace adventOfCode
{
    public class December1 : AoCSolver
    {
        public const int FOUND_NUMBER = 2020;
        private List<int> TestInput = new List<int>() {
            1721,
            979,
            366,
            299,
            675,
            1456
        };

        public December1() : base(1)
        {
        }

        public override bool Test()
        {
            int foundSum = GetPairsResult(TestInput);
            bool testSucceeded = 514579 == foundSum;
            return testSucceeded;
        }

        public override string First()
        {
            string fileName = GetFilename();
            List<int> input = System.IO.File.ReadAllLines(fileName).Select(int.Parse).ToList();
            int foundSum = GetPairsResult(input);
            return foundSum.ToString();
        }

        public override bool Test2()
        {
            int foundSum = GetTripletsResult(TestInput);
            bool testSucceeded = 241861950 == foundSum;
            return testSucceeded;
        }

        public override string Second()
        {
            string fileName = GetFilename();
            List<int> input = System.IO.File.ReadAllLines(fileName).Select(int.Parse).ToList();
            int foundSum = GetTripletsResult(input);
            return foundSum.ToString();
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

        #region Testing different functions in the beginning

        private int EntriesFound(params int[] args)
        {
            var sum = args.Sum();
            if (sum == FOUND_NUMBER)
            {
                int prod = args.Aggregate(1, (a, b) => a * b);
                return prod;
            }
            else
            {
                return -1;
            }
        }

        private int FindSum(List<int> input)
        {
            int foundSum = -1;
            foreach ((int x, int y) in GetAllPairs(input))
            {
                foundSum = EntriesFound(x, y);
                if (foundSum != -1)
                {
                    break;
                }
            }
            return foundSum;
        }

        private int FindSum2(List<int> input)
        {
            int foundSum = -1;
            foreach ((int x, int y, int z) in GetTriplets(input))
            {
                foundSum = EntriesFound(x, y, z);
                if (foundSum != -1)
                {
                    break;
                }
            }
            return foundSum;
        }

        private IEnumerable<(int, int, int)> GetTriplets(IList<int> list)
        {
            var combinations = from item in list
                               from item2 in list
                               from item3 in list
                               where item < item2 && item2 < item3
                               select (item, item2, item3);
            return combinations;
        }

        private IEnumerable<(int, int)> GetPairs(IList<int> list)
        {
            var combinations = from item in list
                               from item2 in list
                               where item < item2
                               select (item, item2);
            return combinations;
        }

        private IEnumerable<(int, int)> GetAllPairs(IList<int> source)
        {
            return source.SelectMany((_, i) => source.Where((_, j) => i < j), (x, y) => (x, y));
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;

namespace adventOfCode2020
{
    public class December10 : AdventOfCode
    {
        public December10() : base(10)
        {
        }

        public static List<int> JOLTAGE_RANGE = Enumerable.Range(1, 3).ToList();
        public class JoltageAdapter
        {
            public int RatedOutputJoltage { get; set; }
            public bool Connected { get; set; }
            public JoltageAdapter(int joltage)
            {
                RatedOutputJoltage = joltage;
                Connected = false;
            }
            public List<int> AdaptersThanCanConnect()
            {
                return JOLTAGE_RANGE.Select(range => range + RatedOutputJoltage).ToList();
            }
        }

        public class ChainOfAdapters
        {
            private List<JoltageAdapter> Adapters { get; set; }
            private List<int> Joltages { get; set; }
            public int Difference1Jolt { get; set; }
            public int Difference3Jolt { get; set; }

            public ChainOfAdapters(List<int> adapters)
            {
                adapters.Add(0);                    // charing outlet
                adapters.Add(adapters.Max() + 3);   // device joltage
                adapters.Sort();

                Joltages = adapters;

                // add adapters
                Adapters = adapters.Select(joltage => new JoltageAdapter(joltage)).ToList();

                Difference1Jolt = 0;
                Difference3Jolt = 0;

                CACHE = new Dictionary<int, long>();
                TEST = new Dictionary<string, long>();
            }

            public void BuildChainDistribution_v2()
            {
                List<int> difference = new List<int>();
                for (int i = 1; i < Joltages.Count; i++)
                {
                    difference.Add(Joltages[i] - Joltages[i - 1]);
                }

                Difference1Jolt = difference.Where(item => item == 1).Count();
                Difference3Jolt = difference.Where(item => item == 3).Count();
            }

            public void BuildChainDistribution()
            {
                List<JoltageAdapter> chain = new List<JoltageAdapter>();

                JoltageAdapter current = Adapters.FirstOrDefault();
                current.Connected = true;

                while (Adapters.Any(adapter => !adapter.Connected))
                {
                    List<int> adaptersThatCanConnect = current.AdaptersThanCanConnect();
                    JoltageAdapter adapterFound = Adapters.FirstOrDefault(adapter => adaptersThatCanConnect.Contains(adapter.RatedOutputJoltage));
                    adapterFound.Connected = true;

                    AddDifference(adapterFound.RatedOutputJoltage - current.RatedOutputJoltage);
                    current = adapterFound;
                }
            }

            public Dictionary<string, long> TEST { get; set; }
            public Dictionary<int, long> CACHE { get; set; }
            public long DistinctChains(List<int> joltages, int currentIndex)
            {
                if (CACHE.ContainsKey(currentIndex))
                {
                    return CACHE[currentIndex];
                }

                // leaf found, add one
                if (currentIndex == joltages.Count() - 1)
                {
                    CACHE.Add(currentIndex, 1);
                    return 1;
                }

                long sum = 0;
                for (int i = currentIndex + 1; i < joltages.Count(); i++)
                {

                    if (joltages[i] <= joltages[currentIndex] + 3)
                    {
                        sum += DistinctChains(joltages, i);
                    }
                    else
                    {
                        break;
                    }
                }

                CACHE.Add(currentIndex, sum);
                return sum;
            }


            public long DistinctChains2(string difference)
            {
                string diffKey = difference;
                if (TEST.ContainsKey(diffKey))
                {
                    Console.WriteLine(diffKey);
                    return TEST[diffKey];
                }

                long sum = 0;
                string newDifference = "";
                for (int i = 0; i < difference.Count() - 1; i++)
                {
                    int current = Int32.Parse(difference[i].ToString());
                    int next = Int32.Parse(difference[i + 1].ToString());
                    if (current + next <= 3)
                    {
                        string newNr = (current + next).ToString();
                        string inputToNext = newDifference + newNr + difference.Substring(i + 2);
                        var newSum = DistinctChains2(inputToNext);
                        sum += (newSum);
                        continue;
                    }
                    newDifference += difference[i];
                }

                // leaf found
                sum += 1;
                TEST.Add(diffKey, sum);
                return sum; // done
            }
            public long DistinctChainsOld(List<int> difference)
            {
                long sum = 0;
                var newDifferenceList = new List<int>();
                for (int i = 0; i < difference.Count() - 1; i++)
                {
                    var current = difference[i];
                    var next = difference[i + 1];
                    if (current + next <= 3)
                    {
                        var list2 = new List<int>() { };
                        list2.AddRange(newDifferenceList);
                        list2.Add(current + next);
                        list2.AddRange(difference.Skip(i + 2));
                        var newSum = DistinctChainsOld(list2);
                        sum += (newSum);
                        continue;
                    }

                    newDifferenceList.Add(difference[i]);
                }

                sum += 1;
                return sum; // done
            }

            public long AllDistinctChainDistributions()
            {
                var tests = DistinctChains(Joltages, 0);
                return tests;
            }

            public long AllDistinctChainDistributionOld()
            {
                List<int> difference = new List<int>();
                for (int i = 0; i < Joltages.Count - 1; i++)
                {
                    int diff = Joltages[i + 1] - Joltages[i];
                    difference.Add(diff);
                }

                var tests = DistinctChainsOld(difference);
                return tests;
            }

            private void AddDifference(int diffJoltage)
            {
                if (diffJoltage == 1)
                {
                    Difference1Jolt += 1;
                }
                else if (diffJoltage == 3)
                {
                    Difference3Jolt += 1;
                }
                else
                {
                    Console.WriteLine("unexpected..");
                }
            }
        }

        public override bool Test()
        {
            List<int> input1 = new List<int>(){
                16,
                10,
                15,
                5,
                1,
                11,
                7,
                19,
                6,
                12,
                4
            };

            // all adapters in my bag
            var adapters = new ChainOfAdapters(input1);
            adapters.BuildChainDistribution_v2();
            bool firstAdaptersResult = adapters.Difference1Jolt == 7 && adapters.Difference3Jolt == 5;

            string filename = GetTestFilename();
            List<int> input2 = System.IO.File.ReadAllLines(filename).Select(int.Parse).ToList();

            // all adapters in my bag
            var adapters2 = new ChainOfAdapters(input2);
            adapters2.BuildChainDistribution_v2();
            bool secondAdaptersResult = adapters2.Difference1Jolt == 22 && adapters2.Difference3Jolt == 10;

            bool testSucceeded = firstAdaptersResult && secondAdaptersResult;
            return testSucceeded;
        }

        public override string First()
        {
            string filename = GetFilename();
            List<int> input = System.IO.File.ReadAllLines(filename).Select(int.Parse).ToList();

            // all adapters in my bag
            var adapters = new ChainOfAdapters(input);
            adapters.BuildChainDistribution_v2();
            int result = adapters.Difference1Jolt * adapters.Difference3Jolt;
            return result.ToString();
        }

        public override bool Test2()
        {
            List<int> input1 = new List<int>(){
                16,
                10,
                15,
                5,
                1,
                11,
                7,
                19,
                6,
                12,
                4
            };

            // all adapters in my bag
            var adapters = new ChainOfAdapters(input1);
            var result1 = adapters.AllDistinctChainDistributions();
            bool firstAdaptersResult = result1 == 8;

            string filename = GetTestFilename();
            List<int> input2 = System.IO.File.ReadAllLines(filename).Select(int.Parse).ToList();

            // all adapters in my bag
            var adapters2 = new ChainOfAdapters(input2);
            var result2 = adapters2.AllDistinctChainDistributions();
            bool secondAdaptersResult = result2 == 19208;

            bool testSucceeded = firstAdaptersResult && secondAdaptersResult;
            return testSucceeded;
        }

        public override string Second()
        {
            string filename = GetFilename();
            List<int> input = System.IO.File.ReadAllLines(filename).Select(int.Parse).ToList();
            // all adapters in my bag
            var adapters = new ChainOfAdapters(input);
            var result = adapters.AllDistinctChainDistributions();
            return result.ToString();
        }
    }
}

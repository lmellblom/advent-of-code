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
                adapters.Add(0);                    // charingOutlet
                adapters.Add(adapters.Max() + 3);   // device joltage
                adapters.Sort();

                Joltages = adapters;

                // add adapters
                Adapters = adapters.Select(joltage => new JoltageAdapter(joltage)).ToList();

                Difference1Jolt = 0;
                Difference3Jolt = 0;
            }

            public void BuildChainDistribution_v2()
            {
                List<int> difference = new List<int>();
                for (int i = 1; i < Joltages.Count; i++)
                {
                    difference.Add(Joltages[i] - Joltages[i - 1]);
                }

                var s = difference.Sum();

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

            public List<List<int>> DistinctChains(List<int> difference)
            {
                var newList2 = new List<List<int>>();

                var newList = new List<int>();
                for (int i = 0; i < difference.Count() - 1; i++)
                {
                    var current = difference[i];
                    var next = difference[i + 1];
                    if (current + next <= 3)
                    {
                        var list2 = new List<int>() { };
                        list2.AddRange(newList);
                        list2.Add(current + next);
                        list2.AddRange(difference.Skip(i + 2));
                        var newLists = DistinctChains(list2);
                        newList2.AddRange(newLists);
                        continue;
                    }

                    newList.Add(difference[i]);
                }

                newList2.Add(newList);

                return newList2; // done
            }
            public int AllDistinctChainDistributions()
            {
                List<int> difference = new List<int>();
                for (int i = 0; i < Joltages.Count - 1; i++)
                {
                    int diff = Joltages[i + 1] - Joltages[i];
                    difference.Add(diff);
                }

                var tests = DistinctChains(difference);

                // List<List<int>> chain = new List<List<int>>();

                // int sum = 0;
                // for (int i = 0; i < Adapters.Count; i++)
                // {
                //     var current = Adapters[i];
                //     List<int> adaptersThatCanConnect = current.AdaptersThanCanConnect();
                //     List<JoltageAdapter> adaptersFound = Adapters.Where(adapter => adaptersThatCanConnect.Contains(adapter.RatedOutputJoltage)).ToList();
                //     sum += adaptersFound.Count();
                //     // how many connection can we go?

                //     // for each possible connection, add a new with all possible connections so far?
                //     // possible connections, +1, +2, +3
                // }

                // while there are any adjecent 1 or 2s togehter, sum and make a new sequence

                // all ones and twos together,


                return tests.Count();
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
            int result1 = adapters.AllDistinctChainDistributions();
            bool firstAdaptersResult = result1 == 8;

            string filename = GetTestFilename();
            List<int> input2 = System.IO.File.ReadAllLines(filename).Select(int.Parse).ToList();

            // all adapters in my bag
            var adapters2 = new ChainOfAdapters(input2);
            int result2 = adapters2.AllDistinctChainDistributions();
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
            // int result = adapters.AllDistinctChainDistributions();
            return "not working"; // result.ToString();
        }
    }
}

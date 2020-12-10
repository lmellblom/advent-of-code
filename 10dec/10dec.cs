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
            public static int ChargingOutletJolts = 0;
            List<JoltageAdapter> Adapters { get; set; }

            public int Difference1Jolt { get; set; }
            public int Difference3Jolt { get; set; }


            public List<List<JoltageAdapter>> Chains {get;set;}

            public ChainOfAdapters(List<int> adapters)
            {
                adapters.Sort();

                List<JoltageAdapter> adaptersInBag = adapters.Select(joltage => new JoltageAdapter(joltage)).ToList();
                Adapters = adaptersInBag;

                var deviceBuiltInJoltage = adaptersInBag.Max(adapter => adapter.RatedOutputJoltage) + JOLTAGE_RANGE.Max();
                var deviceBuiltInJoltageAdapter = new JoltageAdapter(deviceBuiltInJoltage);
                Adapters.Add(deviceBuiltInJoltageAdapter);

                Difference1Jolt = 0;
                Difference3Jolt = 0;

                Chains = new List<List<JoltageAdapter>>();
            }

            public void BuildChainDistribution()
            {
                List<JoltageAdapter> chain = new List<JoltageAdapter>();

                // find the first adapter that can connect to the charing outlet
                List<int> joltages = JOLTAGE_RANGE.Select(range => range + ChargingOutletJolts).ToList();

                JoltageAdapter currentAdapter = Adapters.FirstOrDefault(adapter => joltages.Contains(adapter.RatedOutputJoltage));
                currentAdapter.Connected = true;
                int diffBetweenOutledAndAdapter = currentAdapter.RatedOutputJoltage - ChargingOutletJolts;
                AddDifference(diffBetweenOutledAndAdapter);
                chain.Add(currentAdapter);

                // get the next adapter that can connect
                while (Adapters.Any(adapter => !adapter.Connected))
                {
                    List<int> adaptersThatCanConnect = currentAdapter.AdaptersThanCanConnect();
                    JoltageAdapter adapterFound = Adapters.FirstOrDefault(adapter => adaptersThatCanConnect.Contains(adapter.RatedOutputJoltage));
                    adapterFound.Connected = true;
                    int diffBetweenAdapters = adapterFound.RatedOutputJoltage - currentAdapter.RatedOutputJoltage;
                    AddDifference(diffBetweenAdapters);
                    chain.Add(adapterFound);

                    currentAdapter = adapterFound;
                }

                Chains.Add(chain);
            }

            public int AllDistinctChainDistributions()
            {
                List<List<JoltageAdapter>> chain = new List<List<JoltageAdapter>>();

                // find the first adapter that can connect to the charing outlet
                List<int> joltages = JOLTAGE_RANGE.Select(range => range + ChargingOutletJolts).ToList();
                List<JoltageAdapter> adaptersThatCanConnect = Adapters.Where(adapter => joltages.Contains(adapter.RatedOutputJoltage)).ToList();

                return 0;
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
            adapters.BuildChainDistribution();
            bool firstAdaptersResult = adapters.Difference1Jolt == 7 && adapters.Difference3Jolt == 5;

            string filename = GetTestFilename();
            List<int> input2 = System.IO.File.ReadAllLines(filename).Select(int.Parse).ToList();
            // all adapters in my bag
            var adapters2 = new ChainOfAdapters(input2);
            adapters2.BuildChainDistribution();
            bool secondAdaptersResult = adapters2.Difference1Jolt == 22 && adapters2.Difference3Jolt == 10;

            bool testSucceeded = firstAdaptersResult;
            return testSucceeded;
        }

        public override string First()
        {
            string filename = GetFilename();
            List<int> input = System.IO.File.ReadAllLines(filename).Select(int.Parse).ToList();

            // all adapters in my bag
            var adapters = new ChainOfAdapters(input);
            adapters.BuildChainDistribution();
            int result = adapters.Difference1Jolt * adapters.Difference3Jolt;
            return result.ToString();
        }

        public override bool Test2()
        {
            string filename = GetTestFilename();
            List<string> input = System.IO.File.ReadAllLines(filename).ToList();
            bool testSucceeded = false;
            return testSucceeded;
        }

        public override string Second()
        {
            string filename = GetFilename();
            List<string> input = System.IO.File.ReadAllLines(filename).ToList();
            return "not implemented";
        }
    }
}

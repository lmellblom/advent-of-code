using System;
using System.Collections.Generic;
using System.Linq;

namespace adventOfCode2020
{
    public class December13 : AdventOfCode
    {
        public December13() : base(13)
        {
        }

        public class Bus
        {
            private int Id;
            private int EarliestTimestamp;

            public int GetTimeToWait() => Id - EarliestTimestamp % Id;

            public int GetResult() => GetTimeToWait() * Id;

            public Bus(int min, int earliestTimestamp)
            {
                Id = min;
                EarliestTimestamp = earliestTimestamp;
            }
        }

        public class Departure
        {
            public int Offset;
            public int Id;
            public Departure(int id, int offset)
            {
                Id = id;
                Offset = offset;
            }
        }

        private List<Bus> GetBuses(List<string> input)
        {
            int earliestTimestamp = Int32.Parse(input[0]);
            List<Bus> buses = input[1]
                .Split(',')
                .Where(input => input != "x")
                .Select(input => Int32.Parse(input))
                .Select(input => new Bus(input, earliestTimestamp))
                .ToList();
            return buses;
        }

        private List<Departure> GetDepartures(List<string> input)
        {
            List<Departure> output = new List<Departure>();
            var inputs = input[1].Split(',');
            for (int i = 0; i < inputs.Count(); i++)
            {
                if (Int32.TryParse(inputs[i], out int id))
                {
                    output.Add(new Departure(id, i));
                }
            }
            return output;
        }

        public long GetTimestamp(List<Departure> departures)
        {
            var bus = departures[0];
            long time = 0L;
            long inc = bus.Id;

            // Chinese remainder theorem(https://en.wikipedia.org/wiki/Chinese_remainder_theorem)
            for (var i = 1; i < departures.Count(); i++)
            {
                Console.WriteLine(i);
                bus = departures[i];
                long newTime = bus.Id;
                while (true)
                {
                    time += inc;
                    if ((time + bus.Offset) % newTime == 0)
                    {
                        inc *= newTime;
                        Console.WriteLine("inc: " + inc);
                        break;
                    }
                }
            }

            return time;
        }

        public Bus GetEarliestBus(List<string> input)
        {
            List<Bus> buses = GetBuses(input);
            var sortedBuses = buses.OrderBy(b => b.GetTimeToWait());
            Bus firstBus = sortedBuses.FirstOrDefault();
            return firstBus;
        }

        public override bool Test()
        {
            string filename = GetTestFilename();
            List<string> input = System.IO.File.ReadAllLines(filename).ToList();
            Bus firstBus = GetEarliestBus(input);
            bool testSucceeded = firstBus.GetResult() == 295;
            return testSucceeded;
        }

        public override string First()
        {
            string filename = GetFilename();
            List<string> input = System.IO.File.ReadAllLines(filename).ToList();
            Bus firstBus = GetEarliestBus(input);
            var result = firstBus.GetResult();
            return result.ToString();
        }


        public override bool Test2()
        {
            string filename = GetTestFilename();
            List<string> input = System.IO.File.ReadAllLines(filename).ToList();
            List<Departure> departures = GetDepartures(input);
            long result = GetTimestamp(departures);
            bool testSucceeded = result == 1068781;
            return testSucceeded;
        }

        public override string Second()
        {
            string filename = GetFilename();
            List<string> input = System.IO.File.ReadAllLines(filename).ToList();
            List<Departure> departures = GetDepartures(input);
            long result = GetTimestamp(departures);
            return result.ToString();
        }
    }
}

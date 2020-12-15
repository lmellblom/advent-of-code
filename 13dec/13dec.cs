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

        public bool WriteToConsole = false;

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

            // Id * i + (Id - Offset); // how to calculate an timestamp departure
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

        // Chinese remainder theorem(https://en.wikipedia.org/wiki/Chinese_remainder_theorem)
        public long GetTimestamp(List<Departure> input)
        {
            input = input.OrderBy(a => a.Offset).ToList();

            long n = 0L; // the value to look for
            long inc = input.First().Id;
            if (WriteToConsole) Console.WriteLine($"Startinc: {inc}");

            IEnumerable<Departure> departures = input.Skip(1);
            foreach (var bus in departures)
            {
                if (WriteToConsole) Console.WriteLine($"Modolus: {bus.Id}, Offset {bus.Offset}");
                while (true)
                {
                    if (WriteToConsole) Console.WriteLine($"{n} += {inc}");
                    n += inc;
                    if (WriteToConsole) Console.WriteLine($"{n} + {bus.Offset} % {bus.Id} == 0");
                    if ((n + bus.Offset) % bus.Id == 0)
                    {
                        if (WriteToConsole) Console.WriteLine("---  FOUND ---");
                        if (WriteToConsole) Console.WriteLine($"{inc} *= {bus.Id}");
                        inc *= bus.Id;
                        break;
                    }
                }
            }
            return n;
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
            WriteToConsole = true;
            List<string> input = System.IO.File.ReadAllLines(filename).ToList();
            Bus firstBus = GetEarliestBus(input);
            bool testSucceeded = firstBus.GetResult() == 295;
            WriteToConsole = false;
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



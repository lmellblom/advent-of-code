using System;
using System.Collections.Generic;
using System.Linq;

namespace adventOfCode
{
    public class December16 : AoCSolver
    {
        public December16() : base(16)
        {
        }


        public class TicketRule
        {
            public string Field { get; set; }
            public List<int> ValidNumbers { get; set; }

            private (int min, int max) Range1 { get; set; }
            private (int min, int max) Range2 { get; set; }

            public TicketRule(string input)
            {
                var rule = input.Split(':').Select(item => item.Trim()).ToArray();
                Field = rule[0];
                var ranges = rule[1].Split("or").Select(item => item.Trim()).ToArray();
                var firstRange = ranges[0].Split('-').Select(Int32.Parse).ToArray();
                Range1 = (firstRange[0], firstRange[1]);
                var secondRange = ranges[1].Split('-').Select(Int32.Parse).ToArray();
                Range2 = (secondRange[0], secondRange[1]);

                var firstR = Enumerable.Range(Range1.min, Range1.max - Range1.min + 1);
                var secondR = Enumerable.Range(Range2.min, Range2.max - Range2.min + 1);

                ValidNumbers = firstR.Concat(secondR).ToList();
            }
        }


        public class TicketTranslator
        {
            public List<TicketRule> Rules { get; set; }
            public List<int> MyTicket { get; set; }
            public List<List<int>> NearbyTickets { get; set; }
            public List<int> AllValidNumbers { get; set; }
            public TicketTranslator(List<string> input)
            {
                Rules = new List<TicketRule>();
                NearbyTickets = new List<List<int>>();
                AllValidNumbers = new List<int>();

                string currentAppending = "rules";
                foreach (var row in input)
                {
                    if (String.IsNullOrWhiteSpace(row))
                    {
                        currentAppending = SwitchAppending(currentAppending);
                    }
                    else
                    {
                        if (currentAppending == "rules")
                        {
                            // split line on : and then on or
                            var rule = new TicketRule(row);
                            Rules.Add(rule);
                            AllValidNumbers.AddRange(rule.ValidNumbers);
                        }
                        else if (currentAppending == "nearbyTicket" && row != "nearby tickets:")
                        {
                            var ticket = row.Split(',').Select(Int32.Parse).ToList();
                            NearbyTickets.Add(ticket);
                        }
                        else if (currentAppending == "yourTicket" && row != "your ticket:")
                        {
                            var myTicket = row.Split(',').Select(Int32.Parse).ToList();
                            MyTicket = myTicket;
                        }
                    }
                }
            }

            public long Result2()
            {
                // take the same position for every element in ALL tickets
                // check which range the letter can be in. 
                var validTickets = GetValidNearbyTickets();

                List<(int myNr, List<string> fields)> values = new List<(int, List<string>)>();

                for (int position = 0; position < MyTicket.Count(); position++)
                {
                    var myNr = MyTicket[position];

                    var allNrs = new List<int>() { myNr };
                    allNrs.AddRange(validTickets.Select(ticket => ticket[position]));

                    // find rules for the number
                    var validRules = Rules
                        .Where(rule => ContainsAll<int>(rule.ValidNumbers, allNrs));

                    values.Add((myNr, validRules.Select(r => r.Field).ToList()));
                }
 
                while (values.Any(val => val.fields.Count() != 1))
                {
                    // get the first value that is one and remove in other lists
                    var remove = values.Where(val => val.fields.Count() == 1).Select(val => val.fields).SelectMany(v => v).ToList();
                    var valuesMoreThen1 = values.Where(val => val.fields.Count() != 1);
                    foreach (var val in valuesMoreThen1)
                    {
                        val.fields.RemoveAll(x => remove.Contains(x));
                    }
                }

                long sum = 1;
                foreach (var item in values)
                {
                    if (item.fields.FirstOrDefault().StartsWith("departure"))
                    {
                        sum *= item.myNr;
                    }
                }
                return sum;
            }

            public static bool ContainsAll<T>(IEnumerable<T> source, IEnumerable<T> values)
            {
                return values.All(value => source.Contains(value));
            }

            public List<List<int>> GetValidNearbyTickets()
            {
                return NearbyTickets
                    .Where(ticket => ticket.Except(AllValidNumbers).Count() == 0)
                    .ToList();
            }

            public int TicketErrorScanningRate()
            {
                var notValidNrs = NearbyTickets
                    .Select(ticket => ticket.Except(AllValidNumbers))
                    .SelectMany(i => i);
                return notValidNrs.Sum();
            }

            private string SwitchAppending(string current)
            {
                if (current == "rules")
                {
                    return "yourTicket";
                }
                else if (current == "yourTicket")
                {
                    return "nearbyTicket";
                }
                return "";
            }

        }

        public override bool Test()
        {
            string filename = GetTestFilename();
            List<string> input = System.IO.File.ReadAllLines(filename).ToList();
            TicketTranslator ticketTranslator = new TicketTranslator(input);
            int errorRate = ticketTranslator.TicketErrorScanningRate();
            bool testSucceeded = errorRate == 71;
            return testSucceeded;
        }

        public override string First()
        {
            string filename = GetFilename();
            List<string> input = System.IO.File.ReadAllLines(filename).ToList();
            TicketTranslator ticketTranslator = new TicketTranslator(input);
            int errorRate = ticketTranslator.TicketErrorScanningRate();
            return errorRate.ToString();
        }

        public override bool Test2()
        {
            string filename = GetTest2Filename();
            List<string> input = System.IO.File.ReadAllLines(filename).ToList();
            TicketTranslator ticketTranslator = new TicketTranslator(input);
            var res = ticketTranslator.Result2();
            bool testSucceeded = true; // TEST does not exists for this one
            return testSucceeded;
        }

        public override string Second()
        {
            string filename = GetFilename();
            List<string> input = System.IO.File.ReadAllLines(filename).ToList();
            TicketTranslator ticketTranslator = new TicketTranslator(input);
            var res = ticketTranslator.Result2();
            return res.ToString();
        }
    }
}

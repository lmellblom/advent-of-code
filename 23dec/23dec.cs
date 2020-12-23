using System;
using System.Collections.Generic;
using System.Linq;

namespace adventOfCode2020
{
    public class December23 : AdventOfCode
    {
        public static bool PRINT = true;
        public December23() : base(23)
        {
        }

        public class Cup
        {
            public int Value { get; set; }
            public bool IsPicked { get; set; }
            public Cup(int value)
            {
                Value = value;
                IsPicked = false;
            }
            public override bool Equals(object obj)
            {
                return obj is Cup cup &&
                       Value == cup.Value;
            }
            public override int GetHashCode()
            {
                return HashCode.Combine(Value);
            }
        }

        public class CrabCups
        {
            public List<Cup> CupsInCircle { get; set; }
            public int CurrcentCupIndex { get; set; }
            public CrabCups(string cups)
            {
                CupsInCircle = cups.Select(c => c.ToString()).Select(Int32.Parse).Select(i => new Cup(i)).ToList();
                CurrcentCupIndex = 0;
            }

            public void Play(int until = 10)
            {
                for (int move = 1; move <= until; move++)
                {
                    if (PRINT) Console.WriteLine($"-- move {move} --");
                    if (PRINT) PrintCups();

                    // the current cup
                    Cup currentCup = CupsInCircle[CurrcentCupIndex];

                    // pick up the three cups that are immediately clockwise of the current cup
                    List<Cup> cupsInHand = PickupCups();

                    // select a destination cup. the cup with a label equal to the current cup's label minus one
                    // can only be on the board to search. if not found lowest, go up and search from the highest again
                    Cup destinationCup = SelectDestinationCup(currentCup.Value);

                    // insert the cup from the hands in the right position after the destination cup
                    var destinationCupIndex = CupsInCircle.IndexOf(destinationCup);
                    CupsInCircle.InsertRange(destinationCupIndex + 1, cupsInHand);

                    // make sure the holden cup is at the same index as before! otherwise shift everything 
                    while (currentCup.Value != CupsInCircle[CurrcentCupIndex].Value)
                    {
                        var firstCup = CupsInCircle.FirstOrDefault();
                        CupsInCircle.RemoveAt(0);
                        CupsInCircle.Add(firstCup);
                    }

                    // reset the picked status
                    CupsInCircle.ForEach(c => c.IsPicked = false);

                    // select a new current cup, the cup which is immediately clockwise of the current cup
                    CurrcentCupIndex = GetNextCupIndex(CurrcentCupIndex);
                }

                if (PRINT) Console.WriteLine($"-- final --");
                if (PRINT) PrintCups();
            }

            public string GetResult()
            {
                var res = "";
                var oneIndex = CupsInCircle.FindIndex(c => c.Value == 1);
                var lookIndex = GetNextCupIndex(oneIndex);
                while (lookIndex != oneIndex)
                {
                    res += CupsInCircle[lookIndex].Value;
                    lookIndex = GetNextCupIndex(lookIndex);
                }
                return res;
            }

            protected Cup SelectDestinationCup(int currentCupValue)
            {
                // not the currenc cup!
                var cupsToSearchIn = CupsInCircle.Where(c => c.Value != currentCupValue).ToList();

                // if not found, then go up and search from the highest
                var minValue = cupsToSearchIn.Min(c => c.Value);
                var maxValue = cupsToSearchIn.Max(c => c.Value);

                var valueToSearchFor = currentCupValue - 1;
                valueToSearchFor = valueToSearchFor < minValue ? maxValue : valueToSearchFor;

                var itemFound = cupsToSearchIn.FirstOrDefault(c => c.Value == valueToSearchFor);
                while (itemFound == null)
                {
                    valueToSearchFor--;
                    itemFound = cupsToSearchIn.FirstOrDefault(c => c.Value == valueToSearchFor);
                }

                if (PRINT) Console.WriteLine($"destination: {itemFound.Value}");

                return itemFound;
            }

            protected List<Cup> PickupCups()
            {
                var indexesToPick = new List<int>() { 0, 1, 2 }.Select(i => GetNextCupIndex(CurrcentCupIndex + i)).ToList();
                var cupsToPickUp = indexesToPick.Select(i => CupsInCircle[i]).ToList();
                indexesToPick.ForEach(i => CupsInCircle[i].IsPicked = true);
                if (PRINT) Console.WriteLine($"pick up: {String.Join(", ", cupsToPickUp.Select(s => s.Value))}");

                // remove the cups from the circle
                foreach (var cupToRemove in cupsToPickUp)
                {
                    var index = CupsInCircle.IndexOf(cupToRemove);
                    CupsInCircle.RemoveAt(index);
                }

                return cupsToPickUp;
            }

            protected void PrintCups()
            {
                var strings = new List<string>();
                for (int i = 0; i < CupsInCircle.Count(); i++)
                {
                    var cup = CurrcentCupIndex == i ? $"({CupsInCircle[i].Value})" : $"{CupsInCircle[i].Value}";
                    strings.Add(cup);
                }
                Console.WriteLine($"cups: {String.Join(" ", strings)}");
            }

            public int GetNextCupIndex(int currcentCupIndex)
            {
                var totalCups = CupsInCircle.Count();
                currcentCupIndex++;
                if (currcentCupIndex >= totalCups)
                {
                    currcentCupIndex -= totalCups;
                }
                return currcentCupIndex;
            }
        }

        public override bool Test()
        {
            PRINT = true;
            var game = new CrabCups("389125467");
            game.Play(100);
            var res = game.GetResult();
            bool testSucceeded = res == "67384529";
            return testSucceeded;
        }

        public override string First()
        {
            string filename = GetFilename();
            List<string> input = System.IO.File.ReadAllLines(filename).ToList();

            PRINT = true;
            var game = new CrabCups("712643589");
            game.Play(100);
            var res = game.GetResult();

            return res.ToString();
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

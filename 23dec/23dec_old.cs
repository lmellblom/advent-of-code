using System;
using System.Collections.Generic;
using System.Linq;

namespace adventOfCode2020
{
    public class December23_Old : AdventOfCode
    {
        public static bool PRINT = true;
        public December23_Old() : base(23)
        {
        }

        public class CrabCups
        {
            public List<int> Cups { get; set; }
            public int CurrcentCupIndex { get; set; }
            public CrabCups(string cups)
            {
                Cups = cups.Select(c => c.ToString()).Select(Int32.Parse).ToList();
                CurrcentCupIndex = 0;
            }

            public CrabCups(string cups, bool reachMillion)
            {
                Cups = cups.Select(c => c.ToString()).Select(Int32.Parse).ToList();
                CurrcentCupIndex = 0;

                var maxValue = Cups.Max();
                for (int i = maxValue + 1; i <= 1000000; i++)
                {
                    Cups.Add(i);
                }
            }

            public void Play(int until = 10)
            {
                for (int move = 1; move <= until; move++)
                {
                    if (PRINT) Console.WriteLine($"-- move {move} --");
                    if (PRINT) PrintCups();

                    // the current cup
                    int currentCup = Cups[CurrcentCupIndex];

                    // pick up the three cups that are immediately clockwise of the current cup
                    List<int> cupsInHand = PickupCups();

                    // select a destination cup. the cup with a label equal to the current cup's label minus one
                    // can only be on the board to search. if not found lowest, go up and search from the highest again
                    int destinationCup = SelectDestinationCup(currentCup);

                    // insert the cup from the hands in the right position after the destination cup
                    var destinationCupIndex = Cups.IndexOf(destinationCup);
                    Cups.InsertRange(destinationCupIndex + 1, cupsInHand);

                    // make sure the holden cup is at the same index as before! otherwise shift everything 
                    while (currentCup != Cups[CurrcentCupIndex])
                    {
                        var firstCup = Cups.FirstOrDefault();
                        Cups.RemoveAt(0);
                        Cups.Add(firstCup);
                    }

                    // select a new current cup, the cup which is immediately clockwise of the current cup
                    CurrcentCupIndex = GetNextCupIndex(CurrcentCupIndex);
                }

                if (PRINT) Console.WriteLine($"-- final --");
                if (PRINT) PrintCups();
            }


            public void Play2()
            {

            }

            public string GetResult()
            {
                var res = "";
                var oneIndex = Cups.FindIndex(c => c == 1);
                var lookIndex = GetNextCupIndex(oneIndex);
                while (lookIndex != oneIndex)
                {
                    res += Cups[lookIndex];
                    lookIndex = GetNextCupIndex(lookIndex);
                }
                return res;
            }

            protected int SelectDestinationCup(int currentCupValue)
            {
                // not the currenc cup!
                var cupsToSearchIn = Cups.Where(c => c != currentCupValue).ToList();

                // if not found, then go up and search from the highest
                var minValue = cupsToSearchIn.Min();
                var maxValue = cupsToSearchIn.Max();

                var valueToSearchFor = currentCupValue - 1;
                valueToSearchFor = valueToSearchFor < minValue ? maxValue : valueToSearchFor;

                var itemFoundIndex = cupsToSearchIn.FindIndex(c => c == valueToSearchFor);
                while (itemFoundIndex == -1)
                {
                    valueToSearchFor--;
                    itemFoundIndex = cupsToSearchIn.FindIndex(c => c == valueToSearchFor);
                }

                var itemFound = cupsToSearchIn[itemFoundIndex];

                if (PRINT) Console.WriteLine($"destination: {itemFound}");

                return itemFound;
            }

            protected List<int> PickupCups()
            {
                var indexesToPick = new List<int>() { 0, 1, 2 }.Select(i => GetNextCupIndex(CurrcentCupIndex + i)).ToList();
                var cupsToPickUp = indexesToPick.Select(i => Cups[i]).ToList();
                if (PRINT) Console.WriteLine($"pick up: {String.Join(", ", cupsToPickUp)}");

                // remove the cups from the circle
                foreach (var cupToRemove in cupsToPickUp)
                {
                    var index = Cups.IndexOf(cupToRemove);
                    Cups.RemoveAt(index);
                }

                return cupsToPickUp;
            }

            protected void PrintCups()
            {
                var strings = new List<string>();
                for (int i = 0; i < Cups.Count(); i++)
                {
                    var cup = CurrcentCupIndex == i ? $"({Cups[i]})" : $"{Cups[i]}";
                    strings.Add(cup);
                }
                Console.WriteLine($"cups: {String.Join(" ", strings)}");
            }

            public int GetNextCupIndex(int currcentCupIndex)
            {
                var totalCups = Cups.Count();
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
            var game = new CrabCups("712643589", true);
            PRINT = false;
            game.Play(10000000);

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

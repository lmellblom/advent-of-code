using System;
using System.Collections.Generic;
using System.Linq;

namespace adventOfCode2020
{
    public class December23 : AdventOfCode
    {
        public static bool PRINT = false;
        public December23() : base(23)
        {
        }

        public record Cup(int Value, Cup Next)
        {
            public Cup Next { get; set; } = Next;

            // ! IMPORTANT: to be able to debug and not be in a infinte loop...
            public override string ToString()
            {
                return $"Current: {Value}, Next: {Next?.Value}";
            }
        }

        public class CrabCups
        {
            public Dictionary<int, Cup> Cups { get; set; }
            public Cup Current;
            public int Max;
            public CrabCups(string input, int length)
            {
                Cups = new Dictionary<int, Cup>();

                var ints = input
                    .Select(c => c.ToString())
                    .Select(Int32.Parse);

                var maxValueOfInput = ints.Max();

                ints = ints
                    .Concat(Enumerable.Range(maxValueOfInput + 1, length - input.Length))
                    .Reverse();

                Max = ints.Max();

                Cup lastCup = null;
                Cup next = null;
                foreach (var value in ints)
                {
                    var newCup = new Cup(value, next);

                    Cups[value] = newCup;
                    if (next == null)
                    {
                        lastCup = newCup;
                    }

                    next = newCup;
                }

                // make the last one point to the first cup
                Current = Cups[lastCup.Value].Next  = next;
            }

            public void Play(int turns = 10)
            {
                for (int move = 1; move <= turns; move++)
                {
                    if (PRINT) Console.WriteLine($"-- move {move} --");

                    // find the current cup!
                    var (_, next) = Current;

                    if (PRINT) PrintCups();

                    // the next cup should be the third cup
                    var (_, (_, nextNextCup)) = next;

                    // select a destination cup. the cup with a label equal to the current cup's label minus one
                    // can only be on the board to search. if not found lowest, go up and search from the highest again
                    var destinationCup = SelectDestinationCup();

                    // tricky... 
                    Current.Next = nextNextCup.Next; // the current cup should point after the cups we have picked up
                    nextNextCup.Next = destinationCup.Next;
                    destinationCup.Next = next;

                    // move to the next cup
                    Current = Current.Next;
                }
            }

            public Cup SelectDestinationCup()
            {
                // pick up the three cups that are immediately clockwise of the current cup
                var firstCup = Current.Next;
                var secondCup = firstCup.Next;
                var thirdCup = secondCup.Next;

                if (PRINT) Console.WriteLine($"pick up: {firstCup.Value}, {secondCup.Value}, {thirdCup.Value},");

                // if 1, then use the max value
                var destVal = Current.Value == 1 ? Max : Current.Value - 1;

                // value should no be the ones we have picked up!!!
                while (destVal == firstCup.Value || destVal == secondCup.Value || destVal == thirdCup.Value)
                {
                    destVal--;
                    // have we not found any lower than 0, then check after max value
                    if (destVal <= 0)
                    {
                        destVal = Max;
                    }
                }

                if (PRINT) Console.WriteLine($"destination: {Cups[destVal].Value}");

                return Cups[destVal]; // have placed the cups so it is easy to access
            }

            public string GetResult()
            {
                var res = "";
                var oneCup = Cups[1];
                var cup = oneCup.Next;
                for (int i = 0; i < Cups.Count(); i++)
                {
                    if (cup.Value != oneCup.Value)
                    {
                        res += cup.Value;
                    }
                    cup = cup.Next;
                }
                return res;
            }

            public ulong GetResult2()
            {
                var oneCup = Cups[1];
                var next = oneCup.Next;
                var nextNext = next.Next;

                // REMEMBER ulong since large numbers...
                return (ulong)next.Value * (ulong)nextNext.Value;                
            }

            protected void PrintCups()
            {
                var strings = new List<string>();
                var curr = Current;
                for (int i = 0; i < Cups.Count(); i++)
                {
                    var cup = curr.Value == Current.Value ? $"({curr.Value})" : $"{curr.Value}";
                    strings.Add(cup);
                    curr = curr.Next;
                }
                Console.WriteLine($"cups: {String.Join(" ", strings)}");
            }
        }

        public override bool Test()
        {
            PRINT = false;
            string input = "389125467";
            CrabCups game = new CrabCups(input, input.Length);
            game.Play(100);
            string res = game.GetResult();
            bool testSucceeded = res == "67384529";
            return testSucceeded;
        }

        public override string First()
        {
            PRINT = false;
            string input = "712643589";
            CrabCups game = new CrabCups(input, input.Length);
            game.Play(100);
            string res = game.GetResult();

            return res.ToString();
        }

        public override bool Test2()
        {
            PRINT = false;
            string input = "389125467";
            CrabCups game = new CrabCups(input, 1000000);
            game.Play(10000000);
            var res = game.GetResult2();
            bool testSucceeded = res == 149245887792;
            return testSucceeded;
        }

        public override string Second()
        {
            PRINT = false;
            string input = "712643589";
            CrabCups game = new CrabCups(input, 1000000);
            game.Play(10000000);
            var res = game.GetResult2();
            return res.ToString();
        }
    }
}

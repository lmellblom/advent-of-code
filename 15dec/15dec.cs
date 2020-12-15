using System;
using System.Collections.Generic;
using System.Linq;

namespace adventOfCode2020
{
    public class December15 : AdventOfCode
    {
        public December15() : base(15)
        {
        }

        public class MemoryGame
        {
            public bool WriteConsole = false;
            private int[] StartNrs { get; set; }

            #region for version 1
            private Dictionary<int, List<int>> SpokenNumbers { get; set; } // number with the turs
            private int CurrentTurn { get; set; }
            #endregion

            public MemoryGame(params int[] startNrs)
            {
                StartNrs = startNrs;
            }

            public int Play(int untilTurn = 2020)
            {
                // in this version I saved the two last indexes and then saved the difference, SLOW
                SpokenNumbers = new Dictionary<int, List<int>>();
                CurrentTurn = 1;
                int lastSpokenNumber = 0;

                // init all start values
                foreach (var startNr in StartNrs)
                {
                    SpokenNumbers.Add(startNr, new List<int>() { CurrentTurn });
                    lastSpokenNumber = startNr;
                    if (WriteConsole)
                    {
                        Console.WriteLine($"Turn: {CurrentTurn}, spoken: {lastSpokenNumber}");
                    }
                    CurrentTurn++;
                }

                for (int i = CurrentTurn; i <= untilTurn; i++)
                {
                    bool contains = SpokenNumbers.ContainsKey(lastSpokenNumber);
                    int newSpoken;
                    if (contains && SpokenNumbers[lastSpokenNumber].Count() > 1)
                    {
                        // spoken before, get the difference
                        List<int> turns = SpokenNumbers[lastSpokenNumber];
                        var lastItems = turns.Skip(turns.Count() - 2).ToArray();
                        int diff = lastItems[1] - lastItems[0];
                        newSpoken = diff;
                    }
                    else
                    {
                        // not spoken before
                        newSpoken = 0;
                    }

                    AddSpoken(newSpoken, i);
                    lastSpokenNumber = newSpoken;

                    if (WriteConsole)
                    {
                        Console.WriteLine($"Turn: {i}, spoken: {lastSpokenNumber}");
                    }
                }

                return lastSpokenNumber;
            }

            public int Play2(int untilTurn = 2020)
            {
                int[] memory = new int[untilTurn];

                // init start numbers but not the last one
                for (int i = 0; i < StartNrs.Length - 1; i++)
                {
                    memory[StartNrs[i]] = i + 1;
                }

                int lastSpoken = StartNrs[^1];
                int turn = StartNrs.Length;

                while (turn < untilTurn)
                {
                    int newSpoken = memory[lastSpoken] == 0 ? 0 : turn - memory[lastSpoken];

                    // save 
                    memory[lastSpoken] = turn;
                    lastSpoken = newSpoken;
                    turn++;

                    if (WriteConsole)
                    {
                        Console.WriteLine($"Turn: {turn}, spoken: {lastSpoken}");
                    }
                }

                return lastSpoken;
            }

            #region Bad first version, but works for small nr
            private void AddSpoken(int lastSpoken, int turn)
            {
                if (SpokenNumbers.ContainsKey(lastSpoken))
                {
                    int lastTurn = SpokenNumbers[lastSpoken].Last();
                    SpokenNumbers[lastSpoken] = new List<int>() { lastTurn, turn };
                }
                else
                {
                    SpokenNumbers.Add(lastSpoken, new List<int>() { turn });
                }
            }
            #endregion

        }

        public override bool Test()
        {
            var lastSpoken = new MemoryGame(0, 3, 6).Play2() == 436;
            var lastSpoken1 = new MemoryGame(1, 3, 2).Play() == 1;
            var lastSpoken2 = new MemoryGame(2, 1, 3).Play() == 10;
            var lastSpoken3 = new MemoryGame(1, 2, 3).Play() == 27;
            var lastSpoken4 = new MemoryGame(2, 3, 1).Play() == 78;
            var lastSpoken5 = new MemoryGame(3, 2, 1).Play2() == 438;
            var lastSpoken6 = new MemoryGame(3, 1, 2).Play2() == 1836;

            bool testSucceeded = lastSpoken && lastSpoken1 && lastSpoken2
                && lastSpoken3 && lastSpoken4 && lastSpoken5 && lastSpoken6;
            return testSucceeded;
        }

        public override string First()
        {
            var game = new MemoryGame(5, 2, 8, 16, 18, 0, 1);
            var lastSpoken = game.Play2();

            return lastSpoken.ToString();
        }

        public override bool Test2()
        {
            int untilTurn = 30_000_000;

            var game = new MemoryGame(0, 3, 6);
            game.WriteConsole = false;
            var lastSpoken = game.Play2(untilTurn) == 175594;

            var lastSpoken1 = new MemoryGame(1, 3, 2).Play2(untilTurn) == 2578;
            var lastSpoken2 = new MemoryGame(2, 1, 3).Play2(untilTurn) == 3544142;
            var lastSpoken3 = new MemoryGame(1, 2, 3).Play2(untilTurn) == 261214;
            var lastSpoken4 = new MemoryGame(2, 3, 1).Play2(untilTurn) == 6895259;
            var lastSpoken5 = new MemoryGame(3, 2, 1).Play2(untilTurn) == 18;
            var lastSpoken6 = new MemoryGame(3, 1, 2).Play2(untilTurn) == 362;

            bool testSucceeded = lastSpoken && lastSpoken1
                && lastSpoken2 && lastSpoken3 && lastSpoken4 && lastSpoken5 && lastSpoken6;
            return testSucceeded;
        }

        public override string Second()
        {
            var game = new MemoryGame(5, 2, 8, 16, 18, 0, 1);
            var lastSpoken = game.Play2(30_000_000);

            return lastSpoken.ToString();
        }
    }
}

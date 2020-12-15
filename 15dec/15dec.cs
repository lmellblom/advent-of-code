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

            public MemoryGame(params int[] startNrs)
            {
                StartNrs = startNrs;
            }

            public int Play(int untilTurn = 2020)
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
        }

        public override bool Test()
        {
            var lastSpoken = new MemoryGame(0, 3, 6).Play() == 436;
            var lastSpoken1 = new MemoryGame(1, 3, 2).Play() == 1;
            var lastSpoken2 = new MemoryGame(2, 1, 3).Play() == 10;
            var lastSpoken3 = new MemoryGame(1, 2, 3).Play() == 27;
            var lastSpoken4 = new MemoryGame(2, 3, 1).Play() == 78;
            var lastSpoken5 = new MemoryGame(3, 2, 1).Play() == 438;
            var lastSpoken6 = new MemoryGame(3, 1, 2).Play() == 1836;

            bool testSucceeded = lastSpoken && lastSpoken1 && lastSpoken2
                && lastSpoken3 && lastSpoken4 && lastSpoken5 && lastSpoken6;
            return testSucceeded;
        }

        public override string First()
        {
            var game = new MemoryGame(5, 2, 8, 16, 18, 0, 1);
            var lastSpoken = game.Play();

            return lastSpoken.ToString();
        }

        public override bool Test2()
        {
            int untilTurn = 30_000_000;

            var lastSpoken = new MemoryGame(0, 3, 6).Play(untilTurn) == 175594;
            var lastSpoken1 = new MemoryGame(1, 3, 2).Play(untilTurn) == 2578;
            var lastSpoken2 = new MemoryGame(2, 1, 3).Play(untilTurn) == 3544142;
            var lastSpoken3 = new MemoryGame(1, 2, 3).Play(untilTurn) == 261214;
            var lastSpoken4 = new MemoryGame(2, 3, 1).Play(untilTurn) == 6895259;
            var lastSpoken5 = new MemoryGame(3, 2, 1).Play(untilTurn) == 18;
            var lastSpoken6 = new MemoryGame(3, 1, 2).Play(untilTurn) == 362;

            bool testSucceeded = lastSpoken && lastSpoken1
                && lastSpoken2 && lastSpoken3 && lastSpoken4 && lastSpoken5 && lastSpoken6;
            return testSucceeded;
        }

        public override string Second()
        {
            var game = new MemoryGame(5, 2, 8, 16, 18, 0, 1);
            var lastSpoken = game.Play(30_000_000);

            return lastSpoken.ToString();
        }
    }
}

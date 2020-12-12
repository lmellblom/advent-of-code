using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace adventOfCode2020
{
    public class December12 : AdventOfCode
    {
        public December12() : base(12)
        {
        }

        private static int ManhattanDistance(int x1, int x2, int y1, int y2)
        {
            var sum1 = x1 > x2 ? x1 - x2 : x2 - x1;
            var sum2 = y1 > y2 ? y1 - y2 : y2 - y1;
            return Math.Abs(sum1) + Math.Abs(sum2);
        }

        public class Navigation
        {
            public List<string> Instructions { get; set; }
            private int Facing { get; set; }
            private int North { get; set; }  // N
            private int West { get; set; }   // W
            private int South { get; set; }  // S
            private int East { get; set; }   // E
            public Navigation(List<string> instructions, (int north, int south, int west, int east, int facing) starts)
            {
                Instructions = instructions;

                // starts att
                Facing = starts.facing;
                North = starts.north;
                West = starts.west;
                South = starts.south;
                East = starts.east;
            }

            private void SetFacing(int degrees)
            {
                Facing += degrees;
                Facing = Facing % 360;
                if (Facing < 0)
                {
                    Facing += 360;
                }
            }

            public char GetFacing()
            {
                if (Facing == 0)
                {
                    return 'N';
                }
                else if (Facing == 90)
                {
                    return 'E';
                }
                else if (Facing == 180)
                {
                    return 'S';
                }
                else
                {
                    return 'W';
                }
            }

            public int ManhattanDistance()
            {
                var res = December12.ManhattanDistance(East, West, South, North);
                return res;
            }

            public void Move()
            {
                foreach (var instruction in Instructions)
                {
                    int value = Int32.Parse(Regex.Replace(instruction, @"\D", ""));
                    char action = instruction[0];

                    switch (action)
                    {
                        case 'F':
                            // go forward in the direction we are facing
                            GoForward(value);
                            break;
                        case 'L':
                            SetFacing(value * -1);
                            break;
                        case 'R':
                            SetFacing(value);
                            break;
                        case 'N':
                            North += value;
                            break;
                        case 'S':
                            South += value;
                            break;
                        case 'W':
                            West += value;
                            break;
                        case 'E':
                            East += value;
                            break;
                    }
                }
            }

            private void GoForward(int value)
            {
                var facingChar = GetFacing();
                switch (facingChar)
                {
                    case 'N':
                        North += value;
                        break;
                    case 'S':
                        South += value;
                        break;
                    case 'W':
                        West += value;
                        break;
                    case 'E':
                        East += value;
                        break;
                }
            }
        }

        public override bool Test()
        {
            string filename = GetTestFilename();
            List<string> input = System.IO.File.ReadAllLines(filename).ToList();
            var shipNavigation = new Navigation(input, (0, 0, 0, 0, 90));
            shipNavigation.Move();
            int result = shipNavigation.ManhattanDistance();
            bool testSucceeded = result == 25;
            return testSucceeded;
        }

        public override string First()
        {
            string filename = GetFilename();
            List<string> input = System.IO.File.ReadAllLines(filename).ToList();
            var navigation = new Navigation(input, (0, 0, 0, 0, 90));
            navigation.Move();
            int result = navigation.ManhattanDistance();
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

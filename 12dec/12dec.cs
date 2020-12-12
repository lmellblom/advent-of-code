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

        public class Position
        {
            public int Facing { get; set; }
            public int North { get; set; }  // N
            public int West { get; set; }   // W
            public int South { get; set; }  // S
            public int East { get; set; }   // E

            public Position((int north, int south, int west, int east, int facing) starts)
            {
                // starts att
                Facing = starts.facing;
                North = starts.north;
                West = starts.west;
                South = starts.south;
                East = starts.east;
            }

            public Position(Position p)
            {
                // starts att
                Facing = p.Facing;
                North = p.North;
                West = p.West;
                South = p.South;
                East = p.East;
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

            public void SetFacing(int degrees)
            {
                Facing += degrees;
                Facing = Facing % 360;
                if (Facing < 0)
                {
                    Facing += 360;
                }
            }

            public void Rotate(int degrees)
            {
                degrees = degrees % 360;
                if (degrees < 0)
                {
                    degrees += 360;
                }

                // flip everything depending on degrees
                var copiedPos = new Position(this);
                if (degrees == 90)
                {
                    West = copiedPos.North;
                    North = copiedPos.East;
                    East = copiedPos.South;
                    South = copiedPos.West;
                }
                else if (degrees == 180)
                {
                    South = copiedPos.North;
                    West = copiedPos.East;
                    North = copiedPos.South;
                    East = copiedPos.West;
                }
                else
                {
                    East = copiedPos.North;
                    South = copiedPos.East;
                    West = copiedPos.South;
                    North = copiedPos.West;
                }
            }

            public void GoForward(int value)
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

            public void Move(Position position, int value)
            {
                East += (position.East * value);
                North += (position.North * value);
                West += (position.West * value);
                South += (position.South * value);
            }
        }

        public class Navigation
        {
            public List<string> Instructions { get; set; }
            public Position WayPoint { get; set; }
            public Position Boat { get; set; }

            public Navigation(List<string> instructions, (int north, int south, int west, int east, int facing) starts)
            {
                Instructions = instructions;

                Boat = new Position(starts);
            }

            public void SetWayPointStart((int north, int south, int west, int east, int facing) starts)
            {
                WayPoint = new Position(starts);
            }

            public int ManhattanDistance(Position pos)
            {
                var res = December12.ManhattanDistance(pos.East, pos.West, pos.South, pos.North);
                return res;
            }

            public void MoveWaypoint()
            {
                SetWayPointStart((1, 0, 0, 10, Boat.Facing));
                foreach (var instruction in Instructions)
                {
                    int value = Int32.Parse(Regex.Replace(instruction, @"\D", ""));
                    char action = instruction[0];

                    switch (action)
                    {
                        case 'F':
                            // move forward to the waypoint a number of times equal to the given value.
                            Boat.Move(WayPoint, value);
                            break;
                        case 'L':
                            // rotate the waypoint around the ship left (counter-clockwise) the given number of degrees
                            WayPoint.Rotate(value);
                            break;
                        case 'R':
                            // rotate the waypoint around the ship right (clockwise) the given number of degrees
                            WayPoint.Rotate(value * -1);
                            break;
                        case 'N':
                            WayPoint.North += value;
                            break;
                        case 'S':
                            WayPoint.South += value;
                            break;
                        case 'W':
                            WayPoint.West += value;
                            break;
                        case 'E':
                            WayPoint.East += value;
                            break;
                    }
                }
            }

            public void MoveBoat()
            {
                foreach (var instruction in Instructions)
                {
                    int value = Int32.Parse(Regex.Replace(instruction, @"\D", ""));
                    char action = instruction[0];

                    switch (action)
                    {
                        case 'F':
                            // go forward in the direction we are facing
                            Boat.GoForward(value);
                            break;
                        case 'L':
                            Boat.SetFacing(value * -1);
                            break;
                        case 'R':
                            Boat.SetFacing(value);
                            break;
                        case 'N':
                            Boat.North += value;
                            break;
                        case 'S':
                            Boat.South += value;
                            break;
                        case 'W':
                            Boat.West += value;
                            break;
                        case 'E':
                            Boat.East += value;
                            break;
                    }
                }
            }
        }

        public override bool Test()
        {
            string filename = GetTestFilename();
            List<string> input = System.IO.File.ReadAllLines(filename).ToList();
            var shipNavigation = new Navigation(input, (0, 0, 0, 0, 90));
            shipNavigation.MoveBoat();
            int result = shipNavigation.ManhattanDistance(shipNavigation.Boat);
            bool testSucceeded = result == 25;
            return testSucceeded;
        }

        public override string First()
        {
            string filename = GetFilename();
            List<string> input = System.IO.File.ReadAllLines(filename).ToList();
            var navigation = new Navigation(input, (0, 0, 0, 0, 90));
            navigation.MoveBoat();
            int result = navigation.ManhattanDistance(navigation.Boat);
            return result.ToString();
        }

        public override bool Test2()
        {
            string filename = GetTestFilename();
            List<string> input = System.IO.File.ReadAllLines(filename).ToList();
            var shipNavigation = new Navigation(input, (0, 0, 0, 0, 90));
            shipNavigation.MoveWaypoint();
            int result = shipNavigation.ManhattanDistance(shipNavigation.Boat);
            bool testSucceeded = result == 286;
            return testSucceeded;
        }

        public override string Second()
        {
            string filename = GetFilename();
            List<string> input = System.IO.File.ReadAllLines(filename).ToList();
            var shipNavigation = new Navigation(input, (0, 0, 0, 0, 90));
            shipNavigation.MoveWaypoint();
            int result = shipNavigation.ManhattanDistance(shipNavigation.Boat);
            return result.ToString();
        }
    }
}

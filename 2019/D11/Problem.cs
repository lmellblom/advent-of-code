using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace adventOfCode.Y2019.D11
{
    [CodeName("Space Police")]
    public class AoC : IAdventOfCode
    {

        public Result First(List<string> input)
        {
            var robot = new HullPaintingRobot(input.First());
            var res = robot.CountPaintedTiles();
            return new Result(res);
        }

        public Result Second(List<string> input)
        {
            return new Result("not implemented");
        }


        public class HullPaintingRobot
        {
            public IntCodeComputer Brain { get; set; }
            public HullPaintingRobot(string input)
            {
                var ints = input
                    .Split(',')
                    .Select(long.Parse)
                    .ToList();

                Brain = new IntCodeComputer(ints);
            }

            enum Color
            {
                Black = 0,
                White = 1
            }

            enum Direction
            {
                Up = 0,
                Right = 90,
                Down = 180,
                Left = 270,
                Up2 = 360
            }

            public int CountPaintedTiles()
            {
                var path = new Dictionary<(int x, int y), long>();

                int x = 0;
                int y = 0;
                Direction dir = Direction.Up;

                while (!Brain.HasHalted())
                {
                    var input = 0L;
                    if (path.ContainsKey((x, y)))
                    {
                        input = path[(x, y)];
                    }

                    var outputs = Brain.Run(input);

                    path[(x, y)] = outputs[0];

                    var directionToTurn = outputs[1] == 0 ? -90 : 90;
                    dir = directionToTurn + dir;

                    // check which direction is now?
                    if (dir < 0)
                    {
                        dir += 360;
                    }
                    else if ((int)dir > 360)
                    {
                        dir -= 360;
                    }

                    // move in direction
                    var (xMov, yMov) = dir switch
                    {
                        Direction.Right => (1, 0),
                        Direction.Left => (-1, 0),
                        Direction.Down => (0, -1),
                        Direction.Up => (0, 1),
                        Direction.Up2 => (0, 1),
                        _ => throw new Exception("Oh ooh")
                    };

                    x += xMov;
                    y += yMov;
                }

                var painted = path.Count();
                return painted;
            }
        }
    }
}
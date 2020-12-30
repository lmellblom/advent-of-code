using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace adventOfCode.Y2019.D03
{
    [CodeName("Crossed Wires")]
    public class AoC : IAdventOfCode
    {

        public Result First(List<string> input)
        {
            var firstWire = input[0];
            var secondWire = input[1];

            var path1 = Traverse(firstWire);
            var path2 = Traverse(secondWire);

            // check if any dictionary is the same, then select manhattan distance!
            var samePos = 
                from positions in path1.Keys
                where path2.ContainsKey(positions)
                select Helpers.ManhattanDistance(positions.row, positions.col);
            
            // get the lowest distance
            var lowest = samePos.Min();

            return new Result(lowest);
        }

        public Result Second(List<string> input)
        {
            var firstWire = input[0];
            var secondWire = input[1];

            var path1 = Traverse(firstWire);
            var path2 = Traverse(secondWire);

            // check if any dictionary is the same, then select manhattan distance!
            var samePos = 
                from positions in path1.Keys
                where path2.ContainsKey(positions)
                select path1[positions] + path2[positions];
            
            // get the lowest distance
            var lowest = samePos.Min();

            return new Result(lowest);
        }

        private Dictionary<(int row, int col), int> Traverse(string path)
        {
            var wirePath = new Dictionary<(int row, int col), int>();
            
            var (row, col, steps) = (0, 0, 0);
            foreach (var wire in path.Split(','))
            {
                var instruction = wire[0];
                var number = Int32.Parse(wire.Substring(1));

                var (diffRow, diffCol) = instruction switch 
                {
                    'R' => (1,0),
                    'L' => (-1, 0),
                    'D' => (0, -1),
                    'U' => (0, 1), 
                    _ => throw new Exception("Oh ooh")
                };

                for (int step = 0; step < number; step++)
                {
                    (row, col, steps) = (row + diffRow, col + diffCol, steps + 1);

                    // check if in dictionary
                    if (!wirePath.ContainsKey((row,col)))
                    {
                        wirePath.Add((row, col), steps);
                    }
                }
            }
            return wirePath;
        }
    }
}
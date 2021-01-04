using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace adventOfCode.Y2019.D05
{
    [CodeName("Chance of Asteroids")]
    public class AoC : IAdventOfCode
    {
        public Result First(List<string> input)
        {
            var ints = input
                .First()
                .Split(',')
                .Select(long.Parse)
                .ToList();
            var computer = new IntCodeComputer(ints);
            var output = computer.Run(1);

            return new Result(output.Last());
        }

        public Result Second(List<string> input)
        {
           var ints = input
                .First()
                .Split(',')
                .Select(long.Parse)
                .ToList();
            var computer = new IntCodeComputer(ints);
            var output = computer.Run(5);

            return new Result(output.Last());
        }
    }
}
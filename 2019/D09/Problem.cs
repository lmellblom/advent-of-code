using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace adventOfCode.Y2019.D09
{
    [CodeName("Sensor Boost")]
    public class AoC : IAdventOfCode
    {

        public Result First(List<string> input)
        {
            var ints = input
                .First()
                .Split(',')
                .Select(Int32.Parse)
                .ToList();

            var computer = new IntCodeComputer(ints);
            var values = computer.Run(1);

            return new Result(values.Last());
        }

        public Result Second(List<string> input)
        {
             var ints = input
                .First()
                .Split(',')
                .Select(Int32.Parse)
                .ToList();

            var computer = new IntCodeComputer(ints);
            var values = computer.Run(2);

            return new Result(values.Last());
        }
    }
}
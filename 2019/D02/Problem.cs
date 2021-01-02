using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace adventOfCode.Y2019.D02
{
    [CodeName("1202 Program Alarm")]
    public class AoC : IAdventOfCode
    {

        public Result First(List<string> input)
        {
            var ints = input
                 .First()
                 .Split(',')
                 .Select(Int32.Parse)
                 .ToList();

            ints[1] = 12;
            ints[2] = 2;

            var computer = new IntCodeComputer(ints);
            computer.Run();

            return new Result(computer.Values[0]);
        }

        public Result Second(List<string> input)
        {
            var ints = input
               .First()
               .Split(',')
               .Select(Int32.Parse)
               .ToList();

            var res = FindNounAndVerb(ints);
            return new Result(res);
        }

        private int FindNounAndVerb(List<int> input)
        {
            var computer = new IntCodeComputer(input);

            for (var noun = 0; noun < 100; noun++)
            {
                for (var verb = 0; verb < 100; verb++)
                {
                    computer.Reset();
                    computer.Values[1] = noun;
                    computer.Values[2] = verb;
                    computer.Run();
                    if (computer.Values[0] == 19690720)
                    {
                        return 100 * noun + verb;
                    }
                }
            }

            return -1;
        }
    }
}
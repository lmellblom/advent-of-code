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
            List<long> ints = input
                 .First()
                 .Split(',')
                 .Select(long.Parse)
                 .ToList();

            ints[1] = 12;
            ints[2] = 2;

            var computer = new IntCodeComputer(ints);
            computer.Run();

            return new Result(computer.Memory[0]);
        }

        public Result Second(List<string> input)
        {
            var ints = input
               .First()
               .Split(',')
               .Select(long.Parse)
               .ToList();

            var res = FindNounAndVerb(ints);
            return new Result(res);
        }

        private int FindNounAndVerb(List<long> input)
        {
            var computer = new IntCodeComputer(input);
            

            for (var noun = 0; noun < 100; noun++)
            {
                for (var verb = 0; verb < 100; verb++)
                {
                    computer.Reset();
                    computer.Memory[1] = noun;
                    computer.Memory[2] = verb;
                    computer.Run();
                    if (computer.Memory[0] == 19690720)
                    {
                        return 100 * noun + verb;
                    }
                }
            }

            return -1;
        }
    }
}
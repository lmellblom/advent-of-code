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
            var intInput = input.First().Split(',').Select(Int32.Parse).ToList();
            intInput[1] = 12;
            intInput[2] = 2;
            var res = RunProgram(intInput);
            return new Result(res);
        }

        public Result Second(List<string> input)
        {
            var intInput = input.First().Split(',').Select(Int32.Parse).ToList();
            var res = FindNounAndVerb(intInput);
            return new Result(res);
        }

        public TestResult Test(List<string> input)
        {
            // not implemented
            var value = "-";
            var expected = "-";
            bool succeded = value == expected;
            return new TestResult(succeded, expected, value);
        }

        public TestResult Test2(List<string> input)
        {
            // not implemented
            var value = "-";
            var expected = "-";
            bool succeded = value == expected;
            return new TestResult(succeded, expected, value);
        }

        private int RunProgram(List<int> input)
        {
            for (int i = 0; i < input.Count(); i += 4)
            {
                int opcode = input[i];
                int index1 = input[i + 1];
                int index2 = input[i + 2];

                int positionToStore = input[i + 3];
                int first = input[index1];
                int second = input[index2];

                if (opcode == 99)
                {
                    break;
                }

                input[positionToStore] = opcode == 1 ? first + second : first * second;
            }

            return input[0];
        }

        private int FindNounAndVerb(List<int> input)
        {
            for (var noun = 0; noun < 100; noun++)
            {
                for (var verb = 0; verb < 100; verb++)
                {
                    var newInput = input.ToList();
                    newInput[1] = noun;
                    newInput[2] = verb;
                    if (RunProgram(newInput) == 19690720)
                    {
                        return 100 * noun + verb;
                    }
                }
            }

            return -1;
        }
    }
}
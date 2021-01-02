using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace adventOfCode.Y2019.D07
{
    [CodeName("Amplification Circuit")]
    public class AoC : IAdventOfCodeWithTest
    {

        public Result First(List<string> input)
        {
            var value = RunAmplifierController(input, false, new[] { 0, 1, 2, 3, 4 });
            return new Result(value);
        }

        public Result Second(List<string> input)
        {
            var value = RunAmplifierController(input, true, new[] { 5, 6, 7, 8, 9 });
            return new Result(value);
        }

        public TestResult Test(List<string> input)
        {
            var value = RunAmplifierController(input, false, new[] { 0, 1, 2, 3, 4 });
            var expected = 43210;
            bool succeded = value == expected;
            return new TestResult(succeded, expected, value);
        }

        public TestResult Test2(List<string> input)
        {
            var newinput = new List<string>();
            newinput.Add("3,26,1001,26,-4,26,3,27,1002,27,2,27,1,27,26,27,4,27,1001,28,-1,28,1005,28,6,99,0,0,5");
            var value = RunAmplifierController(newinput, true, new[] { 5, 6, 7, 8, 9 });
            var expected = 139629729;
            bool succeded = value == expected;
            return new TestResult(succeded, expected, value);
        }

        private long RunAmplifierController(List<string> input, bool loop, int[] phases)
        {
            var ints = input
                .First()
                .Split(',')
                .Select(Int32.Parse)
                .ToList();

            var amplifiers = Enumerable.Range(0, phases.Count()).Select(x => new IntCodeComputer(ints)).ToArray();
            var maxValue = 0L;

            var permutations = Helpers.GetPermutations<int>(phases.ToList());
            foreach (var permutation in permutations)
            {
                maxValue = Math.Max(maxValue, ExecuteAmplifier(amplifiers, permutation, loop));
            }
            return maxValue;
        }

        private long ExecuteAmplifier(IntCodeComputer[] amplifiers, int[] phaseSettings, bool loop)
        {
            // init each amplifier with the right phase settings
            for (int i = 0; i < amplifiers.Count(); i++)
            {
                amplifiers[i].Reset();
                amplifiers[i].Input.Enqueue(phaseSettings[i]);
            }

            long[] data = new[] { 0L };

            // to make a feedback loop, just loop until we find a stop!!
            while (true)
            {
                foreach (var amplifier in amplifiers)
                {
                    data = amplifier.Run(data).ToArray();
                }

                if (amplifiers.All(a => a.HasHalted()))
                {
                    return data.Last();
                }
            }
        }
    }
}
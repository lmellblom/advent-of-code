using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace adventOfCode.Y2019.D01
{
    [CodeName("The Tyranny of the Rocket Equation")]
    public class AoC : IAdventOfCode
    {

        public Result First(List<string> input)
        {
            var fuel = input
                .Select(Int32.Parse)
                .Select(mass => Math.Floor(mass / 3.0) - 2)
                .Sum();
            return new Result(fuel);
        }

        public Result Second(List<string> input)
        {
            var fuel = input
                .Select(Int32.Parse)
                .Select(mass => FuelReqForModuleMass(mass))
                .Sum();
            return new Result(fuel);
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

        private double FuelReqForModuleMass(double mass)
        {
            var fuel = Math.Floor((mass / 3.0)) - 2;
            if (fuel >= 0)
            {
                fuel += FuelReqForModuleMass(fuel);
            }
            return fuel >= 0 ? fuel : 0;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace adventOfCode.Y2019.D01
{
    [CodeName("Rocket Equation")]
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
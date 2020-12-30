using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace adventOfCode.Y2020.D22 
{
    [CodeName("Crab Combat")]      
    public class AoC : IAdventOfCodeWithTest 
    {

        public Result First(List<string> input) 
        {
            return new Result("not implemented");
        }

        public Result Second(List<string> input) 
        {
            return new Result("not implemented");
        }

        public TestResult Test(List<string> input) 
        {
            var value = "-";
            var expected = "";
            bool succeded = value == expected;
            return new TestResult(succeded, expected, value);
        }

        public TestResult Test2(List<string> input) 
        {
            var value = "-";
            var expected = "";
            bool succeded = value == expected;
            return new TestResult(succeded, expected, value);
        }
    }
}
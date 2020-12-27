namespace adventOfCode
{
    public static class TemplateAoCGenerator
    {
        public record Problem(string Title, int Day, int Year) { }

        public static string Generate(Problem problem)
        {
            return $@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace adventOfCode.Y{problem.Year}.D{problem.Day.ToString("00")} 
{{
    [CodeName(""{problem.Title}"")]      
    public class AoC : IAdventOfCode 
    {{

        public Result First(List<string> input) 
        {{
            return new Result(""not implemented"");
        }}

        public Result Second(List<string> input) 
        {{
            return new Result(""not implemented"");
        }}

        public TestResult Test(List<string> input) 
        {{
            var value = ""-"";
            var expected = """";
            bool succeded = value == expected;
            return new TestResult(succeded, expected, value);
        }}

        public TestResult Test2(List<string> input) 
        {{
            var value = ""-"";
            var expected = """";
            bool succeded = value == expected;
            return new TestResult(succeded, expected, value);
        }}
    }}
}}";
        }
    }

}
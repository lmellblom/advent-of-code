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

        public object First(List<string> input) 
        {{
            return ""not implemented"";
        }}

        public object Second(List<string> input) 
        {{
            return ""not implemented"";
        }}

        public bool Test(List<string> input) 
        {{
            return false;
        }}

        public bool Test2(List<string> input) 
        {{
            return false;
        }}
    }}
}}";
        }
    }

}
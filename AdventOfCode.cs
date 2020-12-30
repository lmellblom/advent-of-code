using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace adventOfCode
{
    public record TestResult(bool succeded, object expected, object value) { }
    public record Result(object value) { }

    public class CodeName : Attribute
    {
        public readonly string Name;
        public CodeName(string name)
        {
            this.Name = name;
        }
    }

    public interface IAdventOfCode
    {
        Result First(List<string> input);
        Result Second(List<string> input);
    }

    public interface IAdventOfCodeWithTest: IAdventOfCode
    {
        TestResult Test(List<string> input);
        TestResult Test2(List<string> input);
    }

    public static class AoCExtensions
    {
        public static string GetName(this IAdventOfCode aoc)
        {
            return (
                aoc
                    .GetType()
                    .GetCustomAttribute(typeof(CodeName)) as CodeName
            ).Name;
        }

        public static string DayName(this IAdventOfCode aoc)
        {
            return $"Dec {aoc.Day()}";
        }

        public static int Year(this IAdventOfCode aoc)
        {
            var t = aoc.GetType();
            return int.Parse(t.FullName.Split('.')[1].Substring(1));
        }

        public static int Day(this IAdventOfCode aoc)
        {
            var t = aoc.GetType();
            return int.Parse(t.FullName.Split('.')[2].Substring(1));
        }

        public static string WorkingDir(int year)
        {
            var yearDir = Path.Combine(year.ToString());
            return yearDir;
        }

        public static string WorkingDir(int year, int day)
        {
            var yearDir = Path.Combine(year.ToString());
            return Path.Combine(yearDir, "D" + day.ToString("00"));
        }

        public static string WorkingDir(this IAdventOfCode aoc)
        {
            return WorkingDir(aoc.Year(), aoc.Day());
        }

        public static List<string> ReadInputFile(this IAdventOfCode aoc, bool testfile = false)
        {
            var dir = aoc.WorkingDir();
            var filename = testfile ? "input_test.txt" : "input.txt";
            var path = Path.Combine(dir, filename);
            if (File.Exists(path))
            {
                return System.IO.File.ReadAllLines(path).ToList();
            }
            return new List<string>();
        }
    }

    #region For the first iteration of the AoC
    public interface IAoCSolver
    {
        bool Test();
        bool Test2();

        object First();
        object Second();

        int GetDay();

        void Run();

        void RunOnlyResult();

    }

    public abstract class AoCSolver : IAoCSolver
    {
        private int Day;

        public AoCSolver(int day)
        {
            Day = day;
        }

        public int GetDay()
        {
            return Day;
        }

        public void Run()
        {
            bool testResult1 = Test();
            bool testResult2 = Test2();

            if (testResult1 && testResult2)
            {
                RunOnlyResult();
                return;
            }

            ConsoleHelper.WriteHelloMessage(Day);

            // FIRST            
            ConsoleHelper.WriteTestResultMessage("Test1", testResult1);
            ConsoleHelper.WriteBreakLine();

            object result1 = First();
            ConsoleHelper.WriteResultMessage("FIRST", result1.ToString());
            ConsoleHelper.WriteBreakLine();

            // SECOND
            ConsoleHelper.WriteTestResultMessage("Test2", testResult2);
            ConsoleHelper.WriteBreakLine();

            object result2 = Second();
            ConsoleHelper.WriteResultMessage("SECOND", result2.ToString());
        }

        public void RunOnlyResult()
        {
            var result1 = First().ToString();
            var result2 = Second().ToString();
            ConsoleHelper.WriteHelloMessageAndResult(Day, result1, result2);
        }

        public abstract bool Test();
        public abstract bool Test2();

        public abstract object First();
        public abstract object Second();

        public string GetTestFilename()
        {
            return $"2020_first/{Day}dec/input_test.txt"; ;
        }

        public string GetTest2Filename()
        {
            return $"2020_first/{Day}dec/input_test_2.txt"; ;
        }

        public string GetFilename()
        {
            return $"2020_first/{Day}dec/input.txt"; ;
        }
    }
    #endregion
}
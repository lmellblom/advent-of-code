using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using static adventOfCode.TemplateAoCGenerator;

namespace adventOfCode
{
    class Program
    {
        static void Main(string[] args)
        {
            bool debug = false;
            if (debug)
            {
                // maybe run a specific function here to be able to debug nicely
                return;
            }

            var action = GetArgument(args.ToList(), "-a");
            var runPuzzle = HasFlag(args.ToList(), "-puzzle");

            if (action.ToLower() == "generate")
            {
                GenerateProblemAoC(args);
            }
            else if (runPuzzle)
            {
                RunPuzzles();
            }
            else
            {
                RunProblems(args);
            }
        }

        static bool HasFlag(List<string> arguments, string prefix)
        {
            if (arguments.Count() == 0)
            {
                return false;
            }
            return arguments.FindIndex(a => a == prefix) != -1;
        }

        static string GetArgument(List<string> arguments, string prefix)
        {
            if (arguments.Count() == 0)
            {
                return "";
            }

            var index = arguments.FindIndex(a => a == prefix);
            var value = arguments[index + 1];
            return value;
        }

        static void GenerateProblemAoC(string[] args)
        {
            var arguments = args.ToList();
            var dayValue = GetArgument(arguments, "-d");
            var yearValue = GetArgument(arguments, "-y");
            var nameValue = GetArgument(arguments, "-n");

            // check if file exists, if not create!
            var problem = new Problem(nameValue, Int32.Parse(dayValue), Int32.Parse(yearValue));

            var dir = AoCExtensions.WorkingDir(problem.Year, problem.Day);

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            var file = Path.Combine(dir, "Problem.cs");
            if (!File.Exists(file))
            {
                FileHelper.WriteFile(file, TemplateAoCGenerator.Generate(problem));
            }

            // generate input-files with empty stuff
            var inputFile = Path.Combine(dir, "input.txt");
            if (!File.Exists(inputFile))
            {
                FileHelper.WriteFile(inputFile, "");
            }
            var inputTestFile = Path.Combine(dir, "input_test.txt");
            if (!File.Exists(inputTestFile))
            {
                FileHelper.WriteFile(inputTestFile, "");
            }
        }

        static void RunProblems(string[] args)
        {
            // maybe certain arguments etc...
            var arguments = args.ToList();
            var dayValue = GetArgument(arguments, "-d");
            var yearValue = GetArgument(arguments, "-y");

            // get all IAdventOfCode
            var allClasses = Assembly.GetEntryAssembly().GetTypes()
                .Where(t => t.IsClass && typeof(IAdventOfCode).IsAssignableFrom(t))
                .OrderBy(t => t.FullName)
                .ToArray();

            var allProblems = allClasses
                .Select(t => Activator.CreateInstance(t) as IAdventOfCode)
                .ToArray();

            foreach (var item in allProblems)
            {
                Run(item);
            }
        }


        public static void Run(IAdventOfCode aoc)
        {
            var inputTest = aoc.ReadInputFile(true);
            bool testResult1 = aoc.Test(inputTest);
            bool testResult2 = aoc.Test2(inputTest);

            if (testResult1 && testResult2)
            {
                RunOnlyResult(aoc);
                return;
            }

            Console.WriteLine($"{aoc.GetName()}");
            ConsoleHelper.WriteBreakLine();

            ConsoleHelper.WriteHelloMessage(aoc.Day());

            // FIRST            
            ConsoleHelper.WriteTestResultMessage("Test1", testResult1);
            ConsoleHelper.WriteBreakLine();

            var input = aoc.ReadInputFile();

            object result1 = aoc.First(input);
            ConsoleHelper.WriteResultMessage("FIRST", result1.ToString());
            ConsoleHelper.WriteBreakLine();

            // SECOND
            ConsoleHelper.WriteTestResultMessage("Test2", testResult2);
            ConsoleHelper.WriteBreakLine();

            object result2 = aoc.Second(input);
            ConsoleHelper.WriteResultMessage("SECOND", result2.ToString());
        }

        public static void RunOnlyResult(IAdventOfCode aoc)
        {
            var input = aoc.ReadInputFile();
            var result1 = aoc.First(input).ToString();
            var result2 = aoc.Second(input).ToString();
            ConsoleHelper.WriteHelloMessageAndResult(aoc.Day(), result1, result2);
        }

        static void RunPuzzles()
        {
            Assembly asm = Assembly.GetEntryAssembly();
            var aoc = asm.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && typeof(IAoCSolver).IsAssignableFrom(t))
                .OrderBy(t => t.FullName)
                .ToArray();

            // List all puzzles in this assembly
            List<IAoCSolver> puzzles = aoc
                .Select(t => Activator.CreateInstance(t) as IAoCSolver)
                .ToList();

            DateTime thisDay = DateTime.Today;
            int day = thisDay.Day;

            // day can not be more than 25, if so then everyhting is finished!
            if (day > 25 || thisDay.Month != 12)
            {
                for (int i = 1; i <= 25; i++)
                {
                    var puzzle = puzzles.FirstOrDefault(p => p.GetDay() == i);
                    if (puzzle != null)
                    {
                        puzzle.RunOnlyResult();
                    }
                }
            }
            else
            {
                // previous days, only print results
                for (int i = 0; i < day - 1; i++)
                {
                    var prePuzzle = puzzles.FirstOrDefault(p => p.GetDay() == i);
                    if (prePuzzle != null)
                    {
                        prePuzzle.RunOnlyResult();
                    }
                }

                var todayPuzzle = puzzles.FirstOrDefault(p => p.GetDay() == day);
                if (todayPuzzle != null)
                {
                    todayPuzzle.Run();
                }
            }
        }
    }
}
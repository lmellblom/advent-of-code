using System;

namespace adventOfCode2020
{
    public abstract class AdventOfCode
    {
        private int Day;

        public AdventOfCode(int day)
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

            string result1 = First();
            ConsoleHelper.WriteResultMessage("FIRST", result1);
            ConsoleHelper.WriteBreakLine();

            // SECOND
            ConsoleHelper.WriteTestResultMessage("Test2", testResult2);
            ConsoleHelper.WriteBreakLine();

            string result2 = Second();
            ConsoleHelper.WriteResultMessage("SECOND", result2);
        }

        public void RunOnlyResult()
        {
            string result1 = First();
            string result2 = Second();
            ConsoleHelper.WriteHelloMessageAndResult(Day, result1, result2);
        }

        public abstract bool Test();
        public abstract bool Test2();

        public abstract string First();
        public abstract string Second();

        public string GetTestFilename() 
        {
            return  $"{Day}dec/input_test.txt";;
        }

        public string GetTest2Filename() 
        {
            return  $"{Day}dec/input_test_2.txt";;
        }

        public string GetFilename() 
        {
            return  $"{Day}dec/input.txt";;
        }
    }
}
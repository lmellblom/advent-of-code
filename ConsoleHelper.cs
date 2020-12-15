using System;

namespace adventOfCode2020
{
    public static class ConsoleHelper
    {
        public static void WriteHelloMessage(int dayNr) {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(" ");
            Console.WriteLine("------------------- ");
            Console.WriteLine($"|   Hello dec {dayNr}!   |");
            Console.WriteLine(" ------------------- ");
            Console.WriteLine(" ");
            Console.ResetColor();
        }


        public static void WriteHelloMessageAndResult(int dayNr, string result1, string result2) {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(" ");
            Console.WriteLine("------------------- ");
            Console.Write($"|   Hello dec {dayNr}!   |");
            Console.ForegroundColor = ConsoleColor.Green;

            // to make everything line up nicely
            var firstResult = $"   1: {result1}";
            Console.Write(firstResult);
            var spacesToAdd = 25 - firstResult.Length;
            for (int i = 0; i < spacesToAdd; i++)
            {
                Console.Write(" ");
            }
            var secondResult = $"2: {result2}";
            Console.Write(secondResult);

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(" ------------------- ");
            Console.WriteLine(" ");
            Console.ResetColor();
        }

        public static void WriteBreakLine() {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("- - - - - - ");
            Console.ResetColor();
        }

        public static void WriteTestResultMessage(string message, bool succeded) {
            if (succeded) {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine($"{message} succeded!");
            } else {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"{message} failed... :(");
            }

            Console.ResetColor();
        }

        public static void WriteResultMessage(string test, string result) {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{test} answer: {result}");
            Console.ResetColor();
        }
    }
}
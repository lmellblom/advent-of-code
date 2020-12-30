using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace adventOfCode.Y2020.D02
{
    [CodeName("Password Philosophy")]
    public class AoC : IAdventOfCodeWithTest
    {
        public Result First(List<string> input)
        {
            int validPasswords = ProcessInput(input)
                .Select(p => p.IsValid())
                .Count(v => v == true);
            return new Result(validPasswords);
        }

        public Result Second(List<string> input)
        {
            int validPasswords = ProcessInput(input)
                .Select(p => p.IsValidOnPosition())
                .Count(v => v == true);
            return new Result(validPasswords);
        }

        public TestResult Test(List<string> input)
        {
            int validPasswords = ProcessInput(input)
                .Select(p => p.IsValid())
                .Count(v => v == true);
            var expected = 2;
            bool succeded = validPasswords == expected;
            return new TestResult(succeded, expected, validPasswords);
        }

        public TestResult Test2(List<string> input)
        {
            int validPasswords = ProcessInput(input)
                .Select(p => p.IsValidOnPosition())
                .Count(v => v == true);
            var expected = 1;
            bool succeded = validPasswords == expected;
            return new TestResult(succeded, expected, validPasswords);
        }

        protected List<PasswordValidator> ProcessInput(List<string> inputs)
        {
            return inputs.Select(input => ConvertLineToPassword(input)).ToList();
        }

        private PasswordValidator ConvertLineToPassword(string input)
        {
            // split the password
            string[] splitInput = input.Replace(":", "").Split(" ");
            List<int> bounds = splitInput[0].Split("-").Select(Int32.Parse).ToList();
            string letter = splitInput[1];
            string password = splitInput[2];
            return new PasswordValidator(bounds.First(), bounds.Last(), char.Parse(letter), password);
        }

        protected record PasswordValidator(int FirstRuleNr, int SecondRuleNr, char Letter, string Password)
        {
            public bool IsValid()
            {
                // find number of count of the letter in password and check if in range
                var foundMatches = Password.Count(c => c == Letter);
                return FirstRuleNr <= foundMatches && foundMatches <= SecondRuleNr;
            }

            public bool IsValidOnPosition()
            {
                // 1 = first letter. so -1 on everything
                var firstChar = Password.ElementAtOrDefault(FirstRuleNr - 1);
                var secondChar = Password.ElementAtOrDefault(SecondRuleNr - 1);

                // only one is accaptable
                return firstChar != secondChar && (firstChar == Letter || secondChar == Letter);
            }
        };

    }
}
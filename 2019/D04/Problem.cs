using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace adventOfCode.Y2019.D04
{
    [CodeName("Secure Container")]
    public class AoC : IAdventOfCode
    {

        public Result First(List<string> input)
        {
            var result = ValidPasswords(206938, 679128);
            return new Result(result);
        }

        public Result Second(List<string> input)
        {
            var result = ValidPasswords(206938, 679128, true);
            return new Result(result);
        }

        public TestResult Test(List<string> input)
        {
            var value = "-";
            var expected = "-";
            bool succeded = value == expected;
            return new TestResult(succeded, expected, value);
        }

        public TestResult Test2(List<string> input)
        {
            var value = "-";
            var expected = "-";
            bool succeded = value == expected;
            return new TestResult(succeded, expected, value);
        }

        private int ValidPasswords(int start, int end, bool partOfLargerGroup = false)
        {
            var validPasswords = Enumerable
                .Range(start, end - start + 1)
                .Select(p => Valid(p.ToString(), partOfLargerGroup))
                .Where(v => v == true)
                .Count();

            return validPasswords;
        }

        private bool Valid(string password, bool partOfLargerGroup)
        {
            // sort string and se if the same
            var sorted = String.Join("", password.OrderBy(p => p));
            if (sorted != password)
            {
                return false;
            }

            // check if any two adjacent digits are the same
            var matches = Regex.Matches(password, @"(.)\1+");
            if (!matches.Any())
            {   
                return false;
            }

            if (partOfLargerGroup)
            {
                // check if there are any matches == 2
                var hasSmallGroup = matches.Where(m => m.Length == 2).Count();
                if (hasSmallGroup < 1)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
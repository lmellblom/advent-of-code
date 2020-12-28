using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace adventOfCode.Y2020.D07
{
    [CodeName("Handy Haversacks")]
    public class AoC : IAdventOfCode
    {
        public Dictionary<string, List<(int number, string bagColor)>> BagTree { get; set; }
        public static string BAG_COLOR_TO_FIND = "shiny gold";

        public Result First(List<string> input)
        {
            BuildBagTree(input);
            int sum = HowManyBagsIncludesDesiredColor();
            return new Result(sum);
        }

        public Result Second(List<string> input)
        {
            BuildBagTree(input);
            int sum = CountBags(BagTree[BAG_COLOR_TO_FIND]);
            return new Result(sum);
        }

        public TestResult Test(List<string> input)
        {
            BuildBagTree(input);
            int sum = HowManyBagsIncludesDesiredColor();
            var expected = 4;
            bool succeded = sum == expected;
            return new TestResult(succeded, expected, sum);
        }

        public TestResult Test2(List<string> input)
        {
            BuildBagTree(input);
            int sum = CountBags(BagTree[BAG_COLOR_TO_FIND]);
            var expected = 32;
            bool succeded = sum == expected;
            return new TestResult(succeded, expected, sum);
        }

        public class Bag
        {
            public string Color { get; set; }
            public List<(int number, string bagColor)> IncludeBags { get; set; } // bags inside this bag
            public Bag(string input)
            {
                var strings = input.Split("contain").Select(value => value.Trim().Replace(".", "")).ToList();
                var bagColor = GetBagColor(strings[0]);

                Color = bagColor;
                IncludeBags = new List<(int number, string bagColor)>();

                if (strings[1] != "no other bags")
                {
                    var bags = strings[1].Split(',').Select(value => value.Trim()).ToList();
                    foreach (var bag in bags)
                    {
                        // get the number
                        string mynumber = Regex.Replace(bag, @"\D", "");
                        int number = Int32.Parse(mynumber);

                        // get the bagcolor
                        string color = GetBagColor(Regex.Replace(bag, @"\d", ""));
                        IncludeBags.Add((number, color));
                    }
                }
            }
            private string GetBagColor(string input)
            {
                return input.Replace("bags", "").Replace("bag", "").Trim(); // ta bort ending of bags
            }
        }

        private void BuildBagTree(List<string> input)
        {
            BagTree = new Dictionary<string, List<(int number, string bagColor)>>();
            foreach (var line in input)
            {
                Bag rule = new Bag(line);
                BagTree.Add(rule.Color, rule.IncludeBags);
            }
        }

        private int HowManyBagsIncludesDesiredColor()
        {
            // how many bags can cary shiny gold (BAG_COLOR_TO_FIND)
            int sum = 0;
            foreach (var tree in BagTree)
            {
                if (BagIncludesColor(BAG_COLOR_TO_FIND, tree.Value))
                {
                    sum += 1;
                }
            }
            return sum;
        }

        private Boolean BagIncludesColor(string colorToFind, List<(int number, string bagColor)> includeBags)
        {
            if (includeBags.Count == 0)
            {
                return false;
            }

            if (includeBags.Any(bag => bag.bagColor == colorToFind))
            {
                return true;
            }
            else
            {
                return includeBags.Any(item => BagIncludesColor(colorToFind, BagTree[item.bagColor]));
            }
        }

        private int CountBags(List<(int number, string bagColor)> includeBags)
        {
            if (includeBags.Count == 0)
            {
                return 0;
            }

            return includeBags.Sum(item => item.number + item.number * CountBags(BagTree[item.bagColor]));
        }
    }
}
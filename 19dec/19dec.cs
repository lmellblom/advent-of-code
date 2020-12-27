using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace adventOfCode2020
{
    public class December19 : AdventOfCode
    {
        public December19() : base(19)
        {
        }

        public class MonsterMessages
        {
            public List<string> Messages { get; set; }
            public Dictionary<int, Rule> AllRules { get; set; }
            public MonsterMessages(List<string> input)
            {
                Messages = new List<string>();
                AllRules = new Dictionary<int, Rule>();

                // process input
                bool readingRules = true;
                foreach (var line in input)
                {
                    if (String.IsNullOrWhiteSpace(line))
                    {
                        readingRules = false;
                        continue;
                    }

                    if (readingRules)
                    {
                        var splittedInput = line.Split(":");
                        var rulestring = splittedInput[1].Trim();
                        int number = Int32.Parse(Regex.Replace(splittedInput[0], @"\D", ""));

                        if (rulestring.StartsWith("\""))
                        {
                            var value = rulestring.Replace("\"", "");
                            AllRules.Add(number, new Rule { Value = value });
                        }
                        else
                        {
                            var rulesString = rulestring.Split("|");
                            var rules = rulesString
                                .Select(rule => rule.Trim().Split(" ").Select(Int32.Parse).ToList())
                                .ToList();
                            AllRules.Add(number, new Rule { SubRules = rules });
                        }
                    }
                    else
                    {
                        Messages.Add(line);
                    }
                }
            }
            public void ReplaceRulePartTwo()
            {
                // 8: 42 | 42 8
                AllRules[8] = new Rule
                {
                    SubRules = new()
                    {
                        new() { 42 },
                        new() { 42, 8 }
                    }
                };
                // 11: 42 31 | 42 11 31
                AllRules[11] = new Rule
                {
                    SubRules = new()
                    {
                        new() { 42, 31 },
                        new() { 42, 11, 31 }
                    }
                };
            }

            public int CountValidMessagesFromRule0()
            {
                int sum = Messages
                    .Select(m => GetPossibleMatches(0, AllRules, m).Contains(m))
                    .Count(value => value == true);
                return sum;
            }

            // https://www.kenneth-truyers.net/2016/05/12/yield-return-in-c/
            public IEnumerable<string> GetPossibleMatches(int ruleNr, IReadOnlyDictionary<int, Rule> rules, string message)
            {
                var currRule = rules[ruleNr];
                if (currRule.HasValue)
                {
                    yield return currRule.Value;
                }
                else
                {
                    foreach (var rule in currRule.SubRules)
                    {
                        var ruleLength = rule.Count();

                        var firstResult = GetPossibleMatches(rule[0], rules, message);
                        foreach (var first in firstResult)
                        {
                            if (ruleLength == 1)
                            {
                                yield return first;
                            }
                            else
                            {
                                if (!message.StartsWith(first))
                                {
                                    // EXIT if the message no longer contains the possible values
                                    continue;
                                }

                                // only match the substring of the message that is left
                                var secondResult = GetPossibleMatches(rule[1], rules, message.Substring(first.Length));
                                foreach (var second in secondResult)
                                {
                                    if (ruleLength == 2)
                                    {
                                        yield return first + second;
                                    }
                                    else
                                    {
                                        if (!message.StartsWith(first + second))
                                        {
                                            // EXIT if the message no longer contains the possible values
                                            continue;
                                        }

                                        // only match the substring of the message that is left
                                        var thirdResult = GetPossibleMatches(rule[2], rules, message.Substring(first.Length + second.Length));
                                        foreach (var third in thirdResult)
                                        {
                                            yield return first + second + third;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public override bool Test()
        {
            string filename = GetTest2Filename();
            List<string> input = System.IO.File.ReadAllLines(filename).ToList();
            var messages = new MonsterMessages(input);
            int sum = messages.CountValidMessagesFromRule0();
            bool testSucceeded = sum == 2;
            return testSucceeded;
        }

        public override string First()
        {
            string filename = GetFilename();
            List<string> input = System.IO.File.ReadAllLines(filename).ToList();
            var messages = new MonsterMessages(input);
            int sum = messages.CountValidMessagesFromRule0();
            return sum.ToString();
        }

        public override bool Test2()
        {
            string filename = GetTestFilename();
            List<string> input = System.IO.File.ReadAllLines(filename).ToList();
            var messages = new MonsterMessages(input);
            messages.ReplaceRulePartTwo();
            int sum = messages.CountValidMessagesFromRule0();
            bool testSucceeded = sum == 12;
            return testSucceeded;
        }

        public override string Second()
        {
            string filename = GetFilename();
            List<string> input = System.IO.File.ReadAllLines(filename).ToList();
            var messages = new MonsterMessages(input);
            messages.ReplaceRulePartTwo();
            int sum = messages.CountValidMessagesFromRule0();
            return sum.ToString();
        }

        public record Rule
        {
            public List<List<int>> SubRules { get; init; }

            public string Value { get; init; }

            public bool HasValue => !string.IsNullOrWhiteSpace(Value);
        }
    }
}

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
                        string mynumber = Regex.Replace(splittedInput[0], @"\D", "");
                        int number = Int32.Parse(mynumber);

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
                        new() {42},
                        new() {42, 8}
                    }
                };
                // 11: 42 31 | 42 11 31
                AllRules[11] = new Rule
                {
                    SubRules = new()
                    {
                        new() {42, 31},
                        new() {42, 11, 31}
                    }
                };
            }

            public int CountValidMessagesFromRule0()
            {
                int sum = 0;
                foreach (var message in Messages)
                {
                    var potentialMessages = TraverseRule(0, AllRules, message);
                    bool valid = potentialMessages.Contains(message);
                    if (valid)
                    {
                        sum++;
                    }
                }
                return sum;
            }

            // https://www.kenneth-truyers.net/2016/05/12/yield-return-in-c/
            public IEnumerable<string> TraverseRule(int ruleNr, IReadOnlyDictionary<int, Rule> rules, string message)
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
                        if (rule.Count() == 1)
                        {
                            var possibleStrings = TraverseRule(rule[0], rules, message);
                            foreach (var s in possibleStrings)
                            {
                                yield return s;
                            }
                        }
                        else if (rule.Count() == 2)
                        {
                            var leftResult = TraverseRule(rule[0], rules, message);
                            foreach (var left in leftResult)
                            {
                                // to end traversing if the rule does not contains in the message
                                // if (!message.StartsWith(left))
                                // {
                                //     continue;
                                // }

                                var rightResult = TraverseRule(rule[1], rules, message.Substring(0));
                                foreach (var right in rightResult)
                                {
                                    yield return left + right;
                                }
                            }
                        }
                        else if (rule.Count() == 3)
                        {
                            var leftResult = TraverseRule(rule[0], rules, message);
                            foreach (var left in leftResult)
                            {
                                // to end traversing if the rule does not contains in the message
                                // if (!message.StartsWith(left))
                                // {
                                //     continue;
                                // }

                                var rightResult = TraverseRule(rule[1], rules, message.Substring(0));
                                foreach (var right in rightResult)
                                {

                                    // if (!message.StartsWith(left + right))
                                    // {
                                    //     continue;
                                    // }

                                    var rightRightResult = TraverseRule(rule[2], rules, message.Substring(0));
                                    foreach (var rightRight in rightRightResult)
                                    {
                                        yield return left + right + rightRight;
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

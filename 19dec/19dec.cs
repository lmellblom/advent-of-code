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

        // Found this.. not smart enough to come up with this. MOVE to helper class
        public static List<string> GetAllPossibleCombos(List<List<string>> strings)
        {
            IEnumerable<string> combos = new[] { "" };
            foreach (var inner in strings)
            {
                combos = combos.SelectMany(r => inner.Select(x => r + x));
            }
            return combos.ToList();
        }

        public class Rule
        {
            public string Value;
            public int Index;
            public List<List<int>> RulesReferences = new List<List<int>>();
            public Rule(string input, int index)
            {
                Index = index;
                if (input.StartsWith("\""))
                {
                    Value = input.Replace("\"", "");
                }
                else
                {
                    var rules = input.Split("|");
                    foreach (var item in rules)
                    {
                        var intRules = item.Trim().Split(" ").Select(Int32.Parse).ToList();
                        RulesReferences.Add(intRules);
                    }
                }
            }
        }

        public class MessageRules
        {
            Dictionary<int, Rule> Rules = new Dictionary<int, Rule>();
            public MessageRules()
            {

            }
            public void AddRule(string input)
            {
                var rules = CreateRule(input);
                Rules.Add(rules.Index, rules);
            }

            public void ReplaceRule(string input)
            {
                var rules = CreateRule(input);
                Rules[rules.Index] = rules;
            }

            private Rule CreateRule(string input)
            {
                var splittedInput = input.Split(":");
                var rulestring = splittedInput[1].Trim();

                string mynumber = Regex.Replace(splittedInput[0], @"\D", "");
                int number = Int32.Parse(mynumber);

                // split the rulstring on rulestring
                var rules = new Rule(rulestring, number);
                return rules;
            }

            public Dictionary<string, List<string>> Memory = new Dictionary<string, List<string>>();

            public List<string> TraverseRules(Rule startRule)
            {
                int subRulesCount = startRule.RulesReferences.Count();
                if (subRulesCount == 0)
                {
                    var values = new List<string>() { startRule.Value };
                    string memoryIndex = startRule.Index.ToString();
                    Memory.Add(memoryIndex, values);
                    return values;
                }

                var output = new List<string>();
                foreach (var subRules in startRule.RulesReferences)
                {
                    var allResults = new List<List<string>>();
                    foreach (var ruleInt in subRules)
                    {
                        // check if exists in memory?
                        string memoryIndex = ruleInt.ToString();
                        List<string> result;
                        if (Memory.ContainsKey(memoryIndex))
                        {
                            result = Memory[memoryIndex];
                        }
                        else
                        {
                            // check infinite looop? 
                            if (ruleInt == startRule.Index)
                            {
                                Console.WriteLine("INFINITE LOOP! how to fix?");
                                continue; // WHAT TO ADD HERE???
                            }
                            else
                            {
                                var rule = Rules[ruleInt];
                                result = TraverseRules(rule);
                                if (!Memory.ContainsKey(memoryIndex))
                                {
                                    Memory.Add(memoryIndex, result);
                                }
                            }
                        }
                        allResults.Add(result);
                    }

                    var allCombos = GetAllPossibleCombos(allResults);
                    output = output.Concat(allCombos).ToList();
                }
                return output;
            }

            public List<string> ValidValuesFromIndex(int index)
            {
                var currentRule = Rules[index];
                var strings = TraverseRules(currentRule);
                return strings;
            }
        }

        public class MonsterMessages
        {
            public MessageRules Rules;
            public List<string> Messages = new List<string>();
            public MonsterMessages(List<string> input)
            {
                bool readingRules = true;
                Rules = new MessageRules();
                foreach (var line in input)
                {
                    if (String.IsNullOrWhiteSpace(line))
                    {
                        readingRules = false;
                        continue;
                    }

                    if (readingRules)
                    {
                        Rules.AddRule(line);
                    }
                    else
                    {
                        Messages.Add(line);
                    }
                }
            }

            public void ReplaceRule(string value)
            {
                Rules.ReplaceRule(value);
            }

            public int CountValidMessagesFromRule0()
            {
                Rules.Memory = new Dictionary<string, List<string>>();
                var rules = Rules.ValidValuesFromIndex(0);
                int sum = 0;
                foreach (var message in Messages)
                {
                    if (rules.Contains(message))
                    {
                        sum++;
                    }
                }
                return sum;
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
            int sum = messages.CountValidMessagesFromRule0();

            // replace rules
            //messages.ReplaceRule("8: 42 | 42 8");
            //messages.ReplaceRule("11: 42 31 | 42 11 31");

            int sum2 = messages.CountValidMessagesFromRule0();
            bool testSucceeded = sum2 == 12;
            return testSucceeded;
        }

        public override string Second()
        {
            string filename = GetFilename();
            List<string> input = System.IO.File.ReadAllLines(filename).ToList();
            return "not implemented";
        }
    }
}

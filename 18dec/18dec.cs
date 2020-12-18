using System;
using System.Collections.Generic;
using System.Linq;

namespace adventOfCode2020
{
    public class December18 : AdventOfCode
    {
        public December18() : base(18)
        {
        }

        public static class Math
        {
            // addition, multiplication, parentheses
            public static double EvaluateExpressions(List<string> input)
            {
                double res = 0;
                foreach (var expression in input)
                {
                    res += Evaluate(expression);
                }
                return res;
            }

            public static double Evaluate(string expression)
            {
                LinkedList<string> stack = new LinkedList<string>();

                string digitValue = "";
                for (int i = 0; i < expression.Count(); i++)
                {
                    char chr = expression[i];

                    // if no digit, add all values from before! (ex 10 is two digits but same number!!)
                    if (!char.IsDigit(chr))
                    {
                        if (!String.IsNullOrWhiteSpace(digitValue))
                        {
                            stack.AddLast(digitValue);
                            digitValue = "";
                        }
                    }

                    // go trough until find an )
                    if (chr == '(')
                    {
                        int bracketCount = 0;
                        string inner = "";
                        i++; // get next char

                        // until end of brackets..
                        for (; i < expression.Length; i++)
                        {
                            chr = expression[i];

                            // check if we found a new bracket
                            if (chr == '(')
                            {
                                bracketCount++;
                            }

                            // found end of bracket! is this the last one?
                            if (chr == ')')
                            {
                                if (bracketCount == 0)
                                {
                                    break;
                                }
                                else
                                {
                                    bracketCount--;
                                }
                            }
                            inner += chr;
                        }

                        // add the result from the inner bracket recursive
                        stack.AddLast(Evaluate(inner).ToString());
                    }
                    // go trough the operators
                    else if (chr == '+' || chr == '*')
                    {
                        stack.AddLast(chr.ToString());
                    }
                    else if (chr == ')' || String.IsNullOrEmpty(chr.ToString()))
                    {
                        // do nothing, alrady have done things above
                    }
                    else if (char.IsDigit(chr))
                    {
                        digitValue += chr;
                    }
                }

                // check if any digits left! add if so!
                if (!String.IsNullOrWhiteSpace(digitValue))
                {
                    stack.AddLast(digitValue);
                }

                while (stack.Count() >= 3)
                {

                    double left = Double.Parse(stack.First());
                    stack.RemoveFirst();
                    string op = stack.First();
                    stack.RemoveFirst();
                    double right = Double.Parse(stack.First());
                    stack.RemoveFirst();

                    double result = 0;
                    if (op == "+")
                    {
                        result = left + right;
                    }
                    else if (op == "*")
                    {
                        result = left * right;
                    }

                    // add the calculated value and the front of the list
                    stack.AddFirst(result.ToString());
                }

                return Double.Parse(stack.First());
            }
        }

        public override bool Test()
        {
            bool res1 = Math.Evaluate("1 + 2 * 3 + 4 * 5 + 6") == 71;
            bool res2 = Math.Evaluate("1 + (2 * 3) + (4 * (5 + 6))") == 51;
            bool res3 = Math.Evaluate("5 + (8 * 3 + 9 + 3 * 4 * 3)") == 437;
            bool res4 = Math.Evaluate("5 * 9 * (7 * 3 * 3 + 9 * 3 + (8 + 6 * 4))") == 12240;
            bool res5 = Math.Evaluate("((2 + 4 * 9) * (6 + 9 * 8 + 6) + 6) + 2 + 4 * 2") == 13632;
            bool testSucceeded = res1 && res2 && res3 && res4 && res5;
            return testSucceeded;
        }

        public override string First()
        {
            string filename = GetFilename();
            List<string> input = System.IO.File.ReadAllLines(filename).ToList();
            double sum = Math.EvaluateExpressions(input);
            return sum.ToString();
        }

        public override bool Test2()
        {
            string filename = GetTestFilename();
            List<string> input = System.IO.File.ReadAllLines(filename).ToList();
            bool testSucceeded = false;
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

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
            public static double EvaluateExpressions(List<string> input)
            {
                double res = 0;
                foreach (var expression in input)
                {
                    res += Evaluate(expression);
                }
                return res;
            }

            public static long Apply(char op, long x, long y)
            {
                switch (op)
                {
                    case '+':
                        return x + y;
                    case '*':
                        return x * y;
                }
                return 0;
            }

            public static long Evaluate(string expression)
            {
                // All the numbers to add togheter
                Stack<long> values = new Stack<long>();

                // All the operators 
                Stack<char> ops = new Stack<char>();

                string digitValue = ""; // to combine the value when a digit is more than 1 character
                for (int i = 0; i < expression.Count(); i++)
                {
                    char chr = expression[i];

                    if (chr == ' ') // empty string, continue
                    {
                        continue;
                    }

                    // if no digit, add all values from before! (ex 10 is two digits but same number!!)
                    if (!char.IsDigit(chr))
                    {
                        if (!String.IsNullOrWhiteSpace(digitValue))
                        {
                            values.Push(Int32.Parse(digitValue));
                            digitValue = "";
                        }
                    }

                    // go trough until find an )
                    if (chr == '(')
                    {
                        int bracketCount = 0;
                        string innerExpr = ""; // the new expression to evaluate
                        i++; // get next char

                        // until end of brackets..
                        for (; i < expression.Length; i++)
                        {
                            chr = expression[i];

                            // check if we found a new bracket before the end of the bracket
                            if (chr == '(') 
                            {
                                bracketCount++;
                            }

                            // found end of bracket! is this the last one?
                            if (chr == ')')
                            {
                                if (bracketCount == 0)
                                {
                                    break; // YEPP!
                                }
                                else
                                {
                                    bracketCount--;
                                }
                            }
                            innerExpr += chr;
                        }

                        // add the result from the inner bracket recursive
                        var innerDigitValue = Evaluate(innerExpr);
                        values.Push(innerDigitValue);
                    }
                    // go trough the operators, then add previous value togehter
                    else if (chr == '+' || chr == '*')
                    {
                        while (ops.Count > 0)
                        {
                            var res = Apply(ops.Pop(), values.Pop(), values.Pop());
                            values.Push(res);
                        }
                        ops.Push(chr);
                    }
                    else if (char.IsDigit(chr))
                    {
                        digitValue += chr;
                    }
                }

                // check if any digits left! add if so!
                if (!String.IsNullOrWhiteSpace(digitValue))
                {
                    int digit = Int32.Parse(digitValue);
                    values.Push(digit);
                }

                while (ops.Count > 0)
                {
                    var res = Apply(ops.Pop(), values.Pop(), values.Pop());
                    values.Push(res);
                }

                return values.Pop();
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
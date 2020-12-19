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
            public static Func<char, char, bool> CurrentPrecedenceRule;

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
            
            // here + is considered to apply before * !!
            public static bool AdditionOverMultiplication(char op1, char op2)
            {
                if ((op1 == '*') && (op2 == '+'))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }

            // does not matter! just take as you like
            public static bool LeftToRightRule(char op1, char op2)
            {
                return true;
            }

            public static long EvaluateExpressions(List<string> input)
            {
                long res = 0;
                foreach (string expression in input)
                {
                    res += Evaluate(expression);
                }
                return res;
            }

            public static long Evaluate(string expression)
            {
                Stack<long> values = new Stack<long>();
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
                        i++; // get next char and skip the (

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
                        long innerDigitValue = Evaluate(innerExpr);
                        values.Push(innerDigitValue);
                    }
                    // go trough the operators, then add previous value togehter
                    else if (chr == '+' || chr == '*')
                    {
                        // has the current operator at the stack higher order? if so, then add togheter values
                        while (ops.Count > 0 && CurrentPrecedenceRule(ops.Peek(), chr))
                        {
                            long res = Apply(ops.Pop(), values.Pop(), values.Pop());
                            values.Push(res);
                        }
                        ops.Push(chr);
                    }
                    else if (char.IsDigit(chr))
                    {
                        digitValue += chr;
                    }
                }

                // check if any digits left!
                if (!String.IsNullOrWhiteSpace(digitValue))
                {
                    int digit = Int32.Parse(digitValue);
                    values.Push(digit);
                }

                // end of input! just add togheter the rest
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
            Math.CurrentPrecedenceRule = Math.LeftToRightRule;
            bool res1 = Math.Evaluate("1 + 2 * 3 + 4 * 5 + 6") == 71;
            bool res2 = Math.Evaluate("1 + (2 * 3) + (4 * (5 + 6))") == 51;
            bool res3 = Math.Evaluate("5 + (8 * 3 + 9 + 3 * 4 * 3)") == 437;
            bool res4 = Math.Evaluate("5 * 9 * (7 * 3 * 3 + 9 * 3 + (8 + 6 * 4))") == 12240;
            bool res5 = Math.Evaluate("((2 + 4 * 9) * (6 + 9 * 8 + 6) + 6) + 2 + 4 * 2") == 13632;
            bool res6 = Math.Evaluate("2 * 3 + (4 * 5)") == 26;
            bool testSucceeded = res1 && res2 && res3 && res4 && res5 && res6;
            return testSucceeded;
        }

        public override string First()
        {
            string filename = GetFilename();
            List<string> input = System.IO.File.ReadAllLines(filename).ToList();
            Math.CurrentPrecedenceRule = Math.LeftToRightRule;
            double sum = Math.EvaluateExpressions(input);
            return sum.ToString();
        }

        public override bool Test2()
        {
            Math.CurrentPrecedenceRule = Math.AdditionOverMultiplication;
            bool res1 = Math.Evaluate("1 + 2 * 3 + 4 * 5 + 6") == 231;
            bool res2 = Math.Evaluate("1 + (2 * 3) + (4 * (5 + 6))") == 51;
            bool res3 = Math.Evaluate("5 + (8 * 3 + 9 + 3 * 4 * 3)") == 1445;
            bool res4 = Math.Evaluate("5 * 9 * (7 * 3 * 3 + 9 * 3 + (8 + 6 * 4))") == 669060;
            bool res5 = Math.Evaluate("((2 + 4 * 9) * (6 + 9 * 8 + 6) + 6) + 2 + 4 * 2") == 23340;
            bool res6 = Math.Evaluate("2 * 3 + (4 * 5)") == 46;
            bool testSucceeded = res1 && res2 && res3 && res4 && res5 && res6;
            return testSucceeded;
        }

        public override string Second()
        {
            string filename = GetFilename();
            List<string> input = System.IO.File.ReadAllLines(filename).ToList();
            Math.CurrentPrecedenceRule = Math.AdditionOverMultiplication;
            double sum = Math.EvaluateExpressions(input);
            return sum.ToString();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace adventOfCode2020
{
    public class December8 : AdventOfCode
    {
        public December8() : base(8)
        {
        }

        public enum OperationEnum
        {
            Accumulator,
            Jumps,
            NoOperation
        }

        public class Instruction
        {
            public OperationEnum Operation { get; set; }

            public int Argument { get; set; } // positive or negative number

            public bool Visited { get; set; }

            public Instruction(string input)
            {
                if (input.Contains("nop"))
                {
                    Operation = OperationEnum.NoOperation;
                    input = input.Replace("nop", "").Trim();
                }
                else if (input.Contains("jmp"))
                {
                    Operation = OperationEnum.Jumps;
                    input = input.Replace("jmp", "").Trim();
                }
                else if (input.Contains("acc"))
                {
                    Operation = OperationEnum.Accumulator;
                    input = input.Replace("acc", "").Trim();
                }

                Argument = Int32.Parse(Regex.Replace(input, @"\D", ""));

                // get the instruction negative/positive right
                if (input.StartsWith("-"))
                {
                    Argument *= -1;
                }

                Visited = false;
            }
        }

        public class Computer
        {
            public List<Instruction> Instructions { get; set; }
            public int Accumulator { get; set; }

            private int Index { get; set; }
            private List<string> RawInput { get; set; }

            public Computer(List<string> inputs)
            {
                Instructions = inputs.Select(line => new Instruction(line)).ToList();
                RawInput = inputs;

                Reset();
            }
            public void Reset()
            {
                Instructions = RawInput.Select(line => new Instruction(line)).ToList();
                Accumulator = 0;
                Index = 0;
            }

            public bool Run() // returns if we passed all the file or not
            {
                var currentInstrunction = Instructions[Index];
                while (!currentInstrunction.Visited)
                {
                    currentInstrunction.Visited = true;
                    Index += RunInstruction(currentInstrunction);

                    if (Index >= Instructions.Count() || Index < 0)
                    {
                        return true; // found!
                    }

                    currentInstrunction = Instructions[Index];
                }

                return false;
            }

            private int RunInstruction(Instruction currentInstrunction)
            {
                // Do different stuff depending on operation
                switch (currentInstrunction.Operation)
                {
                    case OperationEnum.Accumulator:
                        Accumulator += currentInstrunction.Argument;
                        return 1;
                    case OperationEnum.Jumps:
                        return currentInstrunction.Argument;
                    case OperationEnum.NoOperation:
                        return 1;
                }

                return 1; // should not happen
            }

            private bool ToggleInstruction(Instruction instructionToChange)
            {
                if (instructionToChange.Operation == OperationEnum.Jumps)
                {
                    instructionToChange.Operation = OperationEnum.NoOperation;
                    return true;
                }
                else if (instructionToChange.Operation == OperationEnum.NoOperation)
                {
                    instructionToChange.Operation = OperationEnum.Jumps;
                    return true;
                }

                return false;
            }

            public void RunWithChangedInstructions()
            {
                Reset();
                for (int i = 0; i < Instructions.Count(); i++)
                {
                    // toggle any instructions, if the instuction did not toggle, continue to the next
                    if (!ToggleInstruction(Instructions[i]))
                    {
                        continue;
                    }

                    // Run with the toggled instrunction, will return if it did go through the whole list or not
                    if (Run())
                    {
                        return; // found
                    }

                    Reset();
                }
            }
        }

        public override bool Test()
        {
            string filename = "8dec/input_test.txt";

            List<string> input = System.IO.File.ReadAllLines(filename).ToList();
            var program = new Computer(input);
            program.Run();

            bool testSucceeded = program.Accumulator == 5;
            return testSucceeded;
        }

        public override string First()
        {
            string filename = "8dec/input.txt";

            List<string> input = System.IO.File.ReadAllLines(filename).ToList();
            var program = new Computer(input);
            program.Run();

            return program.Accumulator.ToString();
        }

        public override bool Test2()
        {
            string filename = "8dec/input_test.txt";
            List<string> input = System.IO.File.ReadAllLines(filename).ToList();

            var program = new Computer(input);
            program.RunWithChangedInstructions();

            bool testSucceeded = program.Accumulator == 8;
            return testSucceeded;
        }

        public override string Second()
        {
            string filename = "8dec/input.txt";

            List<string> input = System.IO.File.ReadAllLines(filename).ToList();
            var program = new Computer(input);
            program.RunWithChangedInstructions();

            return program.Accumulator.ToString();
        }
    }
}

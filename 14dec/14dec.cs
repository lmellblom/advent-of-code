using System;
using System.Collections.Generic;
using System.Linq;

namespace adventOfCode2020
{
    public class December14 : AdventOfCode
    {
        public December14() : base(14)
        {
        }

        public class InstructionValue
        {
            public string MemoryIndex { get; set; }
            public ulong Value { get; set; }
            public InstructionValue(string index, ulong value)
            {
                MemoryIndex = index;
                Value = value;
            }
        }

        public class Instruction
        {
            public string Mask { get; set; }

            public List<InstructionValue> Instructions { get; set; }

            public Instruction(string mask)
            {
                Mask = mask;
                Instructions = new List<InstructionValue>();
            }

            public void AddInstruction(string index, ulong value)
            {
                Instructions.Add(new InstructionValue(index, value));
            }

        }

        public class Computer
        {
            public List<Instruction> Instructions { get; set; }
            public Dictionary<string, ulong> Memory { get; set; }

            public Computer(List<string> input)
            {
                Memory = new Dictionary<string, ulong>();
                Instructions = new List<Instruction>();

                foreach (var row in input)
                {
                    if (row.StartsWith("mask"))
                    {
                        string mask = row.Replace("mask = ", "");
                        Instructions.Add(new Instruction(mask));
                    }
                    else
                    {
                        // add all fields to the latest known instruction
                        var values = row.Split("=");
                        var index = values[0].Trim().Replace("mem[", "").Replace("]", "");
                        var value = ulong.Parse(values[1].Trim());

                        Instructions.Last().AddInstruction(index, value);
                    }
                }
            }

            // to modify a bit at position p in n to b. 
            public static ulong modifyBit(ulong n, int p, ulong b)
            {
                ulong mask = 1UL << p;
                return (n & ~mask) | ((b << p) & mask);
            }

            public void Run()
            {
                foreach (var instruction in Instructions)
                {
                    foreach (var item in instruction.Instructions)
                    {
                        var value = item.Value;
                        int index = 0;
                        foreach (var maskValue in instruction.Mask.Reverse())
                        {
                            if (maskValue != 'X')
                            {
                                var bitValue = ulong.Parse(maskValue.ToString());
                                value = modifyBit(value, index, bitValue);
                            }
                            index++;
                        }

                        Memory[item.MemoryIndex] = value;
                    }
                }
            }

            public ulong Sum => GetSum();

            private ulong GetSum() 
            {
                ulong sum = 0;
                foreach (var item in Memory)
                {
                    sum += item.Value;
                }
                return sum;
            }
        }

        public override bool Test()
        {
            string filename = GetTestFilename();
            List<string> input = System.IO.File.ReadAllLines(filename).ToList();

            var computer = new Computer(input);
            computer.Run();

            bool testSucceeded = computer.Sum == 165;
            return testSucceeded;
        }

        public override string First()
        {
            string filename = GetFilename();
            List<string> input = System.IO.File.ReadAllLines(filename).ToList();
            var computer = new Computer(input);
            computer.Run();
            return computer.Sum.ToString();
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

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
            public ulong MemoryIndex { get; set; }
            public ulong Value { get; set; }
            public InstructionValue(ulong index, ulong value)
            {
                MemoryIndex = index;
                Value = value;
            }
        }

        public class Instruction
        {
            public string Mask { get; set; }
            public List<(int index, ulong bitValue)> MaskModified { get; set; }

            public List<char[]> AllMasks { get; set; }

            public List<InstructionValue> Instructions { get; set; }

            public Instruction(string mask)
            {
                Mask = mask;
                Instructions = new List<InstructionValue>();
                ConstructMask();
            }

            public void AddInstruction(ulong index, ulong value)
            {
                Instructions.Add(new InstructionValue(index, value));
            }

            private void ConstructMask()
            {
                int index = 0;
                MaskModified = new List<(int, ulong)>();
                foreach (var maskValue in Mask.Reverse())
                {
                    if (maskValue != 'X')
                    {
                        var bitValue = ulong.Parse(maskValue.ToString());
                        MaskModified.Add((index, bitValue));
                    }
                    index++;
                }
            }
        }

        public class Computer
        {
            public List<Instruction> Instructions { get; set; }
            public Dictionary<ulong, ulong> Memory { get; set; }

            public Computer(List<string> input)
            {
                Memory = new Dictionary<ulong, ulong>();
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
                        var index = ulong.Parse(values[0].Trim().Replace("mem[", "").Replace("]", ""));
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
                        foreach (var mask in instruction.MaskModified)
                        {
                            value = modifyBit(value, mask.index, mask.bitValue);
                        }
                        Memory[item.MemoryIndex] = value;
                    }
                }
            }

            public void RunVersionTwo()
            {
                foreach (var instruction in Instructions)
                {
                    foreach (var item in instruction.Instructions)
                    {
                        var memoryValues = new List<ulong>();
                        memoryValues.Add(item.MemoryIndex);

                        int index = 0;
                        foreach (var maskValue in instruction.Mask.Reverse())
                        {
                            if (maskValue == 'X')
                            {
                                int count = memoryValues.Count();

                                // vill be two different values, flip the one in values and add a new one
                                for (int i = 0; i < count; i++)
                                {
                                    var memValue = memoryValues[i];
                                    memoryValues.Add(modifyBit(memValue, index, 1));    // add
                                    memoryValues[i] = modifyBit(memValue, index, 0);    // modify
                                }
                            }
                            else if (maskValue == '0')
                            {
                                // unchanged
                            }
                            else if (maskValue == '1')
                            {
                                var bitValue = ulong.Parse(maskValue.ToString());

                                // values to flip
                                for (int i = 0; i < memoryValues.Count(); i++)
                                {
                                    memoryValues[i] = modifyBit(memoryValues[i], index, bitValue);
                                }
                            }

                            index++;
                        }

                        foreach (var memoryIndex in memoryValues)
                        {
                            Memory[memoryIndex] = item.Value;
                        }

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
            string filename = GetTest2Filename();
            List<string> input = System.IO.File.ReadAllLines(filename).ToList();
            var computer = new Computer(input);
            computer.RunVersionTwo();

            bool testSucceeded = computer.Sum == 208;
            return testSucceeded;
        }

        public override string Second()
        {
            string filename = GetFilename();
            List<string> input = System.IO.File.ReadAllLines(filename).ToList();
            var computer = new Computer(input);
            computer.RunVersionTwo();
            return computer.Sum.ToString();
        }
    }
}

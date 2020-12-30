using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace adventOfCode.Y2019.D05
{
    [CodeName("Chance of Asteroids")]
    public class AoC : IAdventOfCode
    {

        public Result First(List<string> input)
        {
            var intInput = input.First().Split(',').Select(long.Parse).ToList();
            var dictionary = new Dictionary<long, long>();
            for (int i = 0; i < intInput.Count(); i++)
            {
                dictionary.Add(i, intInput[i]);
            }

            var computer = new IntCodeComputer(dictionary);
            computer.Run(1);

            return new Result("not implemented");
        }

        public Result Second(List<string> input)
        {
            return new Result("not implemented");
        }

        public class IntCodeComputer
        {
            enum Mode
            {
                Pos = 0,
                Immediate = 1
            }

            enum Opcode
            {
                Add = 1,
                Multi = 2,
                Input = 3,
                Output = 4,
                Halt = 99
            }

            public IntCodeComputer(Dictionary<long, long> mem)
            {
                Memory = mem;
            }

            public Dictionary<long, long> Memory { get; set; }

            #region Helpfunctions for get the mode or the opcode from an instruction
            private int[] modeMask = new int[] { 0, 100, 1000, 10000 };
            private Mode GetMode(int pos, int i) => (Mode)(Memory[pos] / modeMask[i] % 10);
            private Opcode GetOpcode(int pos) => (Opcode)(Memory[pos] % 100);
            #endregion

            #region Get the value from an adress depending on the mode
            private long GetAddress(int position, int i) => GetMode(position, i) switch
            {
                Mode.Pos => Memory[position + i],
                Mode.Immediate => position + i,
                _ => throw new ArgumentException()
            };
            private long GetValue(int position, int i) => Memory[GetAddress(position, i)];
            #endregion

            public List<long> Run(int inputVal)
            {
                var output = new List<long>();
                int i = 0;

                var until = Memory.Keys.Count();
                while (i < until)
                {
                    var opcode = GetOpcode(i);

                    switch (opcode)
                    {
                        case Opcode.Halt:
                            return output;
                        case Opcode.Add:
                            Memory[GetAddress(i, 3)] = GetValue(i, 1) + GetValue(i, 2);
                            i += 4;
                            break;
                        case Opcode.Multi:
                            Memory[GetAddress(i, 3)] = GetValue(i, 1) * GetValue(i, 2);
                            i += 4;
                            break;
                        case Opcode.Input:
                            Memory[GetAddress(i, 1)] = inputVal;
                            i += 2;
                            break;
                        case Opcode.Output:
                            var outValue = GetValue(i, 1);
                            output.Add(outValue);
                            Console.WriteLine($"Output: {outValue}, index = {i}");
                            i += 2;
                            break;
                        default:
                            throw new ArgumentException();
                    }
                }

                return output;
            }
        }

    }
}
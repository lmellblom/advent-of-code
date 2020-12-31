using System;
using System.Collections.Generic;
using System.Linq;

namespace adventOfCode.Y2019
{
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
            JmpTrue = 5,
            JmpFalse = 6,
            LessThan = 7,
            Equals = 8,
            Halt = 99
        }

        public IntCodeComputer(List<int> input)
        {
            Memory = new Dictionary<long, long>(
                input.Select((int v, int index) => new KeyValuePair<long, long>(index, v)));

            _inital = input;
        }

        public void Reset()
        {
            Memory.Clear();
            Memory = new Dictionary<long, long>(
                _inital.Select((int v, int index) => new KeyValuePair<long, long>(index, v)));
        }

        public Dictionary<long, long> Memory { get; set; }
        private List<int> _inital { get; set; }

        #region Helpfunctions for get the mode or the opcode from an instruction
        private int[] modeMask = new int[] { 0, 100, 1000, 10000 };
        private Mode GetMode(long pos, int i) => (Mode)(Memory[pos] / modeMask[i] % 10);
        private Opcode GetOpcode(long pos) => (Opcode)(Memory[pos] % 100); // the last two chars
        #endregion

        #region Get the value from an adress depending on the mode
        private long GetAddress(long position, int i) => GetMode(position, i) switch
        {
            Mode.Pos => Memory[position + i],
            Mode.Immediate => position + i,
            _ => throw new ArgumentException()
        };
        private long GetParam(long position, int i) => Memory[GetAddress(position, i)];
        #endregion

        public List<long> Run(int inputVal = 0)
        {
            var output = new List<long>();
            long i = 0;

            var until = Memory.Keys.Count();
            while (i < until)
            {
                var opcode = GetOpcode(i);
                switch (opcode)
                {
                    case Opcode.Halt:
                        return output;
                    case Opcode.Add:
                        Memory[GetAddress(i, 3)] = GetParam(i, 1) + GetParam(i, 2);
                        i += 4;
                        break;
                    case Opcode.Multi:
                        Memory[GetAddress(i, 3)] = GetParam(i, 1) * GetParam(i, 2);
                        i += 4;
                        break;
                    case Opcode.JmpTrue:
                        i = GetParam(i, 1) != 0 ? GetParam(i, 2) : i + 3;
                        break;
                    case Opcode.JmpFalse:
                        i = GetParam(i, 1) == 0 ? GetParam(i, 2) : i + 3;
                        break;
                    case Opcode.LessThan:
                        Memory[GetAddress(i, 3)] = GetParam(i, 1) < GetParam(i, 2) ? 1 : 0;
                        i += 4;
                        break;
                    case Opcode.Equals:
                        Memory[GetAddress(i, 3)] = GetParam(i, 1) == GetParam(i, 2) ? 1 : 0;
                        i += 4;
                        break;
                    case Opcode.Input:
                        Memory[GetAddress(i, 1)] = inputVal;
                        i += 2;
                        break;
                    case Opcode.Output:
                        var outValue = GetParam(i, 1);
                        output.Add(outValue);
                        i += 2;
                        break;
                    default:
                        throw new ArgumentException($"wrong opcode: {opcode}");
                }
            }

            return output;
        }
    }
}
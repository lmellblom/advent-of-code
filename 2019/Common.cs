using System;
using System.Collections.Generic;
using System.Linq;

namespace adventOfCode.Y2019
{
    public class Memory
    {
        public Dictionary<long, long> Values { get; set; }
        private List<int> _inital { get; set; }

        public long this[long position]
        {
            get
            {
                // when trying to get a item that does not exists, return 0
                return Values.ContainsKey(position) ? Values[position] : 0;
            }
            set
            {
                Values[position] = value;
            }
        }

        public Memory(List<int> instructions)
        {
            Values = new Dictionary<long, long>(
                instructions.Select((int v, int index) => new KeyValuePair<long, long>(index, v)));

            _inital = instructions;
        }

        public void Reset()
        {
            Values = new Dictionary<long, long>(
                _inital.Select((int v, int index) => new KeyValuePair<long, long>(index, v)));
        }
    }

    public class IntCodeComputer
    {
        enum Mode
        {
            Pos = 0,
            Immediate = 1,
            Relative = 2
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
            AdjustRelative = 9,
            Halt = 99
        }

        public IntCodeComputer(List<int> instructions)
        {
            Memory = new Memory(instructions);
            Input = new Queue<long>();

            _currentPointer = 0;
            _relativePointer = 0;
        }

        public void Reset()
        {
            Memory.Reset();
            Input.Clear();
            _currentPointer = 0;
            _relativePointer = 0;
        }

        public bool HasHalted() => GetOpcode() == Opcode.Halt;

        public Memory Memory { get; set; }
        public Queue<long> Input { get; set; }
        private long _currentPointer { get; set; }
        private long _relativePointer { get; set; }

        #region Helpfunctions for get the mode or the opcode from an instruction
        private int[] _modeMask = new int[] { 0, 100, 1000, 10000 };
        private Mode GetMode(int i) => (Mode)(Memory[_currentPointer] / _modeMask[i] % 10);
        private Opcode GetOpcode() => (Opcode)(Memory[_currentPointer] % 100); // the last two chars
        #endregion

        #region Get the value from an adress depending on the mode
        private long GetAddress(int i) => GetMode(i) switch
        {
            Mode.Pos => Memory[_currentPointer + i],
            Mode.Immediate => _currentPointer + i,
            Mode.Relative => _relativePointer + Memory[_currentPointer + i],
            _ => throw new ArgumentException()
        };
        private long GetParam(int i) => Memory[GetAddress(i)];
        #endregion

        public List<long> Run()
        {
            return Run(0);
        }

        public List<long> Run(params long[] inputs)
        {
            // add the input values
            foreach (var inputVal in inputs)
            {
                Input.Enqueue(inputVal);
            }

            var output = new List<long>();
            while (true)
            {
                var opcode = GetOpcode();
                switch (opcode)
                {
                    case Opcode.Halt:
                        return output;
                    case Opcode.Add:
                        Memory[GetAddress(3)] = GetParam(1) + GetParam(2);
                        _currentPointer += 4;
                        break;
                    case Opcode.Multi:
                        Memory[GetAddress(3)] = GetParam(1) * GetParam(2);
                        _currentPointer += 4;
                        break;
                    case Opcode.JmpTrue:
                        _currentPointer = GetParam(1) != 0 ? GetParam(2) : _currentPointer + 3;
                        break;
                    case Opcode.JmpFalse:
                        _currentPointer = GetParam(1) == 0 ? GetParam(2) : _currentPointer + 3;
                        break;
                    case Opcode.LessThan:
                        Memory[GetAddress(3)] = GetParam(1) < GetParam(2) ? 1 : 0;
                        _currentPointer += 4;
                        break;
                    case Opcode.Equals:
                        Memory[GetAddress(3)] = GetParam(1) == GetParam(2) ? 1 : 0;
                        _currentPointer += 4;
                        break;
                    case Opcode.AdjustRelative:
                        _relativePointer += GetParam(1);
                        _currentPointer += 2;
                        break;
                    case Opcode.Input:
                        if (Input.Count != 0)
                        {
                            Memory[GetAddress(1)] = Input.Dequeue();
                            _currentPointer += 2;
                        }
                        else
                        {
                            // if encouter no inputvalue left, then break the program and
                            // leave the state as it is
                            return output;
                        }
                        break;
                    case Opcode.Output:
                        var outValue = GetParam(1);
                        output.Add(outValue);
                        _currentPointer += 2;
                        break;
                    default:
                        throw new ArgumentException($"wrong opcode: {opcode}");
                }
            }
        }
    }
}
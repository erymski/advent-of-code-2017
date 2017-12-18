using System;
using System.Collections.Generic;

namespace day18
{
    internal class Engine
    {
        class Instruction
        {
            public string debug;
            public string register;
            public Action action = () => { }; // no-op by default

            // how much to shift the pointer
            public Func<int> getOffset = () => 1; // just shift to the next instruction
        }

        private readonly Dictionary<string, long> _registers = new Dictionary<string, long>();
        private readonly List<Instruction> _instructions = new List<Instruction>();
        private int _pos = 0; // current position

        /// <summary>
        /// Last played sound.
        /// </summary>
        public long LastSound
        {
            get { return _lastSound; }
        }
        private long _lastSound = 0;

        /// <summary>
        /// Indexer to access registers by name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private long this[string name]
        {
            get
            {
                long value;
                return _registers.TryGetValue(name, out value) ? value : 0 /* each register should start with a value of 0 */;
            }
            set
            {
                _registers[name] = value;
            }
        }

        public void AddInstruction(string line)
        {
            _instructions.Add(ToInstruction(line));
        }

        private Instruction ToInstruction(string line)
        {
            // split instruction line to pieces
            // first is type, second is register, third is optional and number
            // Example: "set a 1"
            string[] parts = line.Split(" ".ToCharArray());

            var instruction = new Instruction
            {
                debug = line,
                register = parts[1]
            };

            switch (parts[0])
            {
                case "snd": // plays a sound with a frequency equal to the value of X
                    instruction.action = () => _lastSound = this[instruction.register];
                    break;

                case "set": // sets register X to the value of Y
                    instruction.action = () => this[instruction.register] = SafeInteger(parts[2]);
                    break;

                case "add": // increases register X by the value of Y
                    instruction.action = () => this[instruction.register] += SafeInteger(parts[2]);
                    break;

                case "mul": // sets register X to the result of multiplying the value contained in register X by the value of Y
                    instruction.action = () => this[instruction.register] = this[instruction.register] * SafeInteger(parts[2]);
                    break;

                case "mod": // sets X to the result of X modulo Y
                    instruction.action = () => this[instruction.register] %= SafeInteger(parts[2]);
                    break;

                case "rcv": // recovers the frequency of the last sound played, but only when the value of X is not zero. (If it is zero, the command does nothing.)
                    instruction.action = () =>
                    {
                        var currValue = this[instruction.register];
                        if (currValue != 0)
                        {
                            this[instruction.register] = _lastSound;

                            // hack: just create giant offset to stop the loop
                            instruction.getOffset = () => int.MaxValue;
                        }
                    };
                    break;

                case "jgz": // jumps with an offset of the value of Y, but only if the value of X is greater than zero.
                    instruction.getOffset = () => (int)(this[instruction.register] > 0 ? SafeInteger(parts[2]) : 1); // can't be more than int... just cast
                    break;

            }
            return instruction;
        }

        /// <summary>
        /// Get integer from the input, which can be either integer or name of register.
        /// </summary>
        private long SafeInteger(string numOrRegister)
        {
            long result;
            return long.TryParse(numOrRegister, out result) ? result : this[numOrRegister];
        }

        public void Execute()
        {
            _pos = 0;
            while ((_pos >= 0) && (_pos < _instructions.Count))
            {
                var instruction = _instructions[_pos];

                instruction.action();
                _pos += instruction.getOffset();
            };
        }
    }
}
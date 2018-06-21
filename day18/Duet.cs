using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace day18
{
    internal class Duet
    {
        [DebuggerDisplay("Program #{" + nameof(_initValue) + "}")]
        internal class HalfEngine
        {
            private readonly int _initValue;

            [DebuggerDisplay("{" + nameof(debug) + "}")]
            class Instruction
            {
                public string debug;
                public string register;
                public Action action = () => { }; // no-op by default

                public int offset = 1; // just shift to the next instruction
            }

            private readonly Dictionary<string, int> _registers = new Dictionary<string, int>();
            private readonly Queue<int> _queue = new Queue<int>();
            private readonly List<Instruction> _instructions = new List<Instruction>();
            private int _pos = 0; // current position
            private Action<int> _sendCallback;

            /// <summary>
            /// Indexer to access registers by name.
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            private int this[string name]
            {
                get
                {
                    int value;
                    return _registers.TryGetValue(name, out value) ? value : 0 /* each register should start with a value of 0 */;
                }
                set
                {
                    _registers[name] = value;
                }
            }

            /// <summary>
            /// Constructor.
            /// </summary>
            /// <param name="initValue">Initial value for p register.</param>
            public HalfEngine(int initValue)
            {
                _initValue = initValue;
                this["p"] = initValue;
            }

            public void Enqueue(int value)
            {
                _queue.Enqueue(value);
            }

            public void Init(string[] lines, Action<int> sendCallback)
            {
                _sendCallback = sendCallback;
                foreach (var line in lines)
                {
                    _instructions.Add(ToInstruction(line));
                }
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
                    case "snd": // sends the value of X to the other program.
                        instruction.action = () =>
                        {
                            _sendCallback(SafeInteger(instruction.register));
                            SendCount++;
                        };
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

                    case "rcv": // receives the next value and stores it in register X
                        instruction.action = () =>
                        {
                            if (_queue.Count > 0)
                            {
                                if (this[instruction.register] > 0)
                                    this[instruction.register] = _queue.Dequeue();
                                instruction.offset = 1;
                            }
                            else
                            {
                                // wait for data
                                instruction.offset = 0;
                            }
                        };
                        break;

                    case "jgz": // jumps with an offset of the value of Y, but only if the value of X is greater than zero.
                        instruction.action = () =>
                        {
                            instruction.offset = (int)(this[instruction.register] > 0 ? SafeInteger(parts[2]) : 1); // can't be more than int... just cast
                        };
                        break;

                }
                return instruction;
            }

            /// <summary>
            /// Get integer from the input, which can be either integer or name of register.
            /// </summary>
            private int SafeInteger(string numOrRegister)
            {
                int result;
                return int.TryParse(numOrRegister, out result) ? result : this[numOrRegister];
            }

            /// <summary>
            /// Execute current command.
            /// </summary>
            /// <returns>Offset after command execution.</returns>
            public int ExecuteStep()
            {
                var instruction = _instructions[_pos];
                //Console.WriteLine(instruction.debug);

                instruction.action();
                //Console.WriteLine($"{_initValue}\t{_pos}->{_pos+instruction.offset}\t{instruction.debug}: {Regs}");
                _pos += instruction.offset;
                //if (_initValue == 1)
                {
                    //Console.WriteLine($"{_initValue}\t{_pos}: {instruction.debug}: {Regs}");
                }
                return instruction.offset;
            }

            public bool IsRunning => (_pos >= 0) && (_pos < _instructions.Count);
            public int SendCount { get; private set; }
            public bool IsEmpty => _queue.Count == 0;

            public string Regs
            {
                get
                {
                    StringBuilder builder = new StringBuilder();
                    foreach (var pair in _registers)
                    {
                        builder.Append($"{pair.Key}={pair.Value}  ");
                    }
                    builder.Length -= 2;
                    return builder.ToString();
                }
            }
        }

        public static int Play(string[] lines)
        {
            var engine0 = new HalfEngine(0);
            var engine1 = new HalfEngine(1);

            engine0.Init(lines, data => engine1.Enqueue(data));
            engine1.Init(lines, data => engine0.Enqueue(data));

            int count = 0;
            while (true)
            {
                if (!engine0.IsRunning) break;
                if (!engine1.IsRunning) break;

                //Console.WriteLine("-- " + count++);
                int offset0 = engine0.ExecuteStep();
                int offset1 = engine1.ExecuteStep();

                // check for completion or deadlock
                if (offset0 == 0 && offset1 == 0 && engine0.IsEmpty && engine1.IsEmpty) break;
                count++;
            }

            return engine1.SendCount;
        }
    }
}
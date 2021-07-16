using System;
using System.Collections.Generic;

namespace day25
{
    enum Name
    {
        A = 0,
        B,
        C,
        D,
        E,
        F,
        All
    }

    class Action
    {
        public Action(int assign, int move, int nextState)
        {
            Assign = assign;
            Move = move;
            NextState = nextState;
        }

        public int Assign { get; }
        public int Move { get; }

        /// Index of the next state.
        public int NextState { get; set; }
    }

    class State
    {
        public State(Action onZero, Action onOne)
        {
            Actions = new[] { onZero, onOne };
        }

        public Action[] Actions { get; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            const int right = 1, left = -1;
            const int a = 0, b = 1, c = 2, d = 3, e = 4, f = 5;

            // init
            var states = new State[6]
            {
                new State(new Action(1, right, b), new Action(0, left, d)),
                new State(new Action(1, right, c), new Action(0, right, f)), 
                new State(new Action(1, left, c), new Action(1, left, a)),
                new State(new Action(0, left, e), new Action(1, right, a)),
                new State(new Action(1, left, a), new Action(0, right, b)),
                new State(new Action(0, right, c), new Action(0, right, e))
            };

            int currentState = a;
            int cursor = 0;
            var tape = new HashSet<int>(); // position of ones.  if zero - no present in the hash set

            const int steps = 12302209;
            for (int i = 0; i < steps; i++)
            {
                var state = states[currentState];
                var action = tape.Contains(cursor) ? state.Actions[1] : state.Actions[0];

                if (action.Assign == 0)
                {
                    tape.Remove(cursor);
                }
                else
                {
                    tape.Add(cursor);
                }

                cursor += action.Move;
                currentState = action.NextState;
            }

            Console.WriteLine(tape.Count);
            Console.ReadKey();
        }
    }
}

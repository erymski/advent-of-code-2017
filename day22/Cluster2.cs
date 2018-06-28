using System;

namespace day22
{
    class Cluster2 : ClusterBase
    {
        public Cluster2(string[] lines) : base(lines)
        {
        }

        public override bool Burst()
        {
            var packedPos = Pack(_x, _y);
            if (!_infected.TryGetValue(packedPos, out var state))
            {
                state = State.Clean;
            }

            switch (state)
            {
                case State.Clean:
                    Left();
                    break;
                case State.Weakened:
                    // don't change direction
                    break;
                case State.Infected:
                    Right();
                    break;
                case State.Flagged:
                    Reverse();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            State newState = (State) ((int)(state + 1) % 4);
            _infected[packedPos] = newState;

            // move
            Move();

            return newState == State.Infected;
        }
    }
}
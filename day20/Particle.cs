using System;

namespace day20
{
    class Particle
    {
        /// <summary>
        /// Particle index.
        /// </summary>
        public int Index { get; }

        /// <summary>
        /// If particle is alive
        /// </summary>
        public bool Alive = true;

        /// <summary>
        /// Particle data.
        /// 
        /// Nine numbers in order: x,y,z,vx,vy,vz,ax,ay,az
        /// </summary>
        private readonly long[] _data;

        /// <summary>
        /// Distance from origin.
        /// </summary>
        public long Distance => Math.Abs(_data[0]) + Math.Abs(_data[1]) + Math.Abs(_data[2]);

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="data">Array with patricle data.</param>
        /// <param name="index">Particle index.</param>
        public Particle(long[] data, int index)
        {
            _data = data;
            Index = index;
        }

        /// <summary>
        /// Move particle.
        /// </summary>
        public void Step()
        {
            SubStep(0); // x
            SubStep(1); // y
            SubStep(2); // z
        }

        private void SubStep(int posIndex)
        {
            // velocity offsets position by 3 (index + 3)
            // acceleration offsets position by 6 (index + 6)

            // adjust velocity
            _data[posIndex + 3] += _data[posIndex + 6];

            // adjust position
            _data[posIndex] += _data[posIndex + 3];
        }

        public long Hash()
        {
            // only x,y,z matter

            return _data[0] * 10001 + _data[1] * 101 + _data[2];

            // previous, slower version
            //long hash = 17;
            //for (var i = 0; i < 3; i++)
            //{
            //    hash = (hash * 31) ^ _data[i];
            //}
            //return hash.GetHashCode();
        }
    }
}
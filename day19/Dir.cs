using System.Collections.Generic;

namespace day19
{
    /// <summary>
    /// Represents both position and direction.
    /// Direction is unit vector.
    /// </summary>
    struct Dir
    {
        public int x;
        public int y;

        /// <summary>
        /// Add vectors.
        /// </summary>
        public Dir Add(Dir dir)
        {
            return new Dir { x = x + dir.x, y = y + dir.y };
        }

        /// <summary>
        /// Inverse direction.
        /// </summary>
        private Dir Negate()
        {
            return new Dir { x = x * -1, y = y * -1 };
        }

        /// <summary>
        /// Rotate 90 degrees.
        /// </summary>
        private Dir Rotate()
        {
            var newX = y; // dx * 0 + dy * 1;
            var newY = x; // dx * 1 + dy * 0;
            return new Dir { x = newX, y = newY };
        }

        /// <summary>
        /// Get cells on left and right sides.
        /// </summary>
        public IEnumerable<Dir> Sides()
        {
            // take current direction and rotate 90 degrees
            Dir rotated = Rotate();
            yield return rotated;

            // and then rotate result 180 degrees (270 degrees of original direction)
            yield return rotated.Negate();
        }
    }
}
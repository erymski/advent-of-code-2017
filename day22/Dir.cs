namespace day22
{
    struct Dir
    {
        /// <summary>
        /// CCW directions: up, left, down, right.
        /// </summary>
        private static readonly Dir[] _dirs =
        {
            new Dir { dx = 0, dy = 1 },
            new Dir { dx = 1, dy = 0 },
            new Dir { dx = 0, dy = -1 },
            new Dir { dx = -1, dy = 0 },
        };

        public static Dir For(int index)
        {
            return _dirs[index];
        }

        public static readonly int Count = _dirs.Length;


        public int dx;
        public int dy;

    }
}
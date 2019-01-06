namespace Snake
{
    /// <summary>
    /// Data structure for the mazes. Each of the eight mazes is created from a template to fit the playing field size.
    /// </summary>
    class Mazes
    {
        private bool[][,] mazes;
        /// <summary>
        /// Maze data structure. True: obstacle, False: free space.
        /// </summary>
        /// <param name="i">The maze to be returned.</param>
        /// <returns>The i-th maze.</returns>
        public bool[,] this[int i]
        {
            get { return this.mazes[i]; }
        }

        /// <summary>
        /// The snake's head initial position for each maze.
        /// </summary>
        public System.Drawing.Point[] Head;

        /// <summary>
        /// Constructor. Creates the scaled mazes and the snake's head initial position.
        /// </summary>
        /// <param name="w">Width of the playing field.</param>
        /// <param name="h">Height of the playing field.</param>
        public Mazes(int w, int h)
        {
            this.mazes = new bool[8][,];
            this.Head = new System.Drawing.Point[8];

            this.Head[0].Y = this.Head[1].Y = 7 * h / 13;
            this.Head[2].Y = this.Head[4].Y = this.Head[5].Y = 6 * h / 13;
            this.Head[3].Y = this.Head[6].Y = 3 * h / 13;
            this.Head[7].Y = 5 * h / 13;
            this.Head[0].X = this.Head[1].X = 10 * w / 23;
            this.Head[2].X = 13 * w / 23;
            this.Head[3].X = 15 * w / 23;
            this.Head[4].X = 11 * w / 23;
            this.Head[5].X = 10 * w / 23;
            this.Head[6].X = 18 * w / 23;
            this.Head[7].X = 17 * w / 23;


            for (int i = 0; i < 8; i++)
                this.mazes[i] = new bool[w, h];

            // Maze 1:
            for (int i = 0; i < w; i++)
                this.mazes[1][i, 0] = this.mazes[1][i, h-1] = true;
            for (int i = 0; i < h; i++)
                this.mazes[1][0, i] = this.mazes[1][w - 1, i] = true;

            // Maze 2:
            for (int i = 0; i < w * 2 / 23; i++)
                this.mazes[2][i, 0] = this.mazes[2][i, h - 1] = this.mazes[2][w - i - 1, 0] = this.mazes[2][w - i - 1, h - 1] = true;
            for (int i = 0; i < h * 2 / 13; i++)
                this.mazes[2][0, i] = this.mazes[2][w - 1, i] = this.mazes[2][0, h - i - 1] = this.mazes[2][w - 1, h - i - 1] = true;
            for (int i = 0; i < w * 9 / 23; i++)
                this.mazes[2][7 * w / 23 + i, 4 * h / 13] = this.mazes[2][7 * w / 23 + i, 8 * h / 13] = true;

            // Maze 3:
            for (int i = 0; i < w * 8 / 23; i++)
                this.mazes[3][i, h * 10 / 13] = true;
            for (int i = w * 14 / 23; i < w ; i++)
                this.mazes[3][i, h * 2 / 13] = true;
            for (int i = 0; i < h * 6 / 13; i++)
                this.mazes[3][w * 8 / 23, i] = true;
            for (int i = h * 6 / 13; i < h; i++)
                this.mazes[3][w * 14 / 23, i] = true;

            // Maze 4:
            for (int i = 0; i < w; i++)
                this.mazes[4][i, 0] = this.mazes[4][i, h - 1] = true;
            for (int i = 0; i <= 4 * h / 13; i++)
                this.mazes[4][0, i]= this.mazes[4][w - 1, i] = true;
            for (int i = 0; i < 5 * h / 13; i++)
                this.mazes[4][0, h - i - 1] = this.mazes[4][w - 1, h - i - 1] = true;
            for (int i = h * 2 / 13; i <= h * 4 / 13; i++)
                this.mazes[4][6 * w / 23, i] = this.mazes[4][16 * w / 23, i] = true;
            for (int i = h * 8 / 13; i <= h * 10 / 13; i++)
                this.mazes[4][6 * w / 23, i] = this.mazes[4][16 * w / 23, i] = true;
            for (int i = w * 8 / 23; i <= w * 14 / 23; i++)
                this.mazes[4][i, 3 * h / 13] = this.mazes[4][i, 9 * h / 13] = true;

            // Maze 5:
            for (int i = 0; i < w; i++)
                this.mazes[5][i, 7 * h / 13] = true;
            for (int i = 7 * h / 13; i < h; i++)
                this.mazes[5][12 * w / 23, i] = true;
            for (int i = 0; i < h * 4 / 13; i++)
                this.mazes[5][10 * w / 23, i] = true;
            for (int i = 0; i < h / 13; i++)
                this.mazes[5][0, i] = true;
            for (int i = 0; i < w * 2 / 23; i++)
                this.mazes[5][i, 0] = true;
            for (int i = w * 5 / 23; i < w * 19 / 23; i++)
                this.mazes[5][i, 0] = true;
            for (int i = w * 13 / 23; i < w; i++)
                this.mazes[5][i, 4 * h / 13] = true;
            for (int i = 0; i <= w * 10 / 23; i++)
                this.mazes[5][i, 4 * h / 13] = true;

            // Maze 6:
            for (int i = 0; i < w; i++)
                this.mazes[6][i, h / 2] = true;
            for (int i = 0; i < h; i++)
                this.mazes[6][w / 2, i] = true;
            for (int i = w * 10 / 23; i < w * 13 / 23; i++)
                this.mazes[6][i, h * 2 / 13] = true;
            for (int i = w * 10 / 23; i < w * 13 / 23; i++)
                this.mazes[6][i, h * 10 / 13] = true;
            for (int i = h * 5 / 13; i < h * 8 / 13; i++)
                this.mazes[6][w * 7 / 23, i] = true;
            for (int i = h * 5 / 13; i < h * 8 / 13; i++)
                this.mazes[6][w * 16 / 23, i] = true;

            // Maze 7:
            for (int i = w * 14 / 23; i <= w * 16 / 23; i++)
                this.mazes[7][i, h / 13] = true;
            for (int i = 0; i <= w * 11 / 23; i++)
                this.mazes[7][i, h * 4 / 13] = true;
            for (int i = w * 14 / 23; i < w; i++)
                this.mazes[7][i, h * 4 / 13] = true;
            for (int i = w * 2 / 23; i <= w * 8 / 23; i++)
                this.mazes[7][i, h * 8 / 13] = true;
            for (int i = w * 11 / 23; i < w; i++)
                this.mazes[7][i, h * 8 / 13] = true;
            for (int i = w * 6 / 23; i <= w * 8 / 23; i++)
                this.mazes[7][i, h * 11 / 13] = true;
            for (int i = h * 3 / 13; i <= h * 4 / 13; i++)
                this.mazes[7][4 * w / 23, i] = true;
            for (int i = h * 2 / 13; i <= h * 4 / 13; i++)
                this.mazes[7][8 * w / 23, i] = true;
            for (int i = h * 6 / 13; i < h; i++)
                this.mazes[7][8 * w / 23, i] = true;
            for (int i = 0; i <= h * 5 / 13; i++)
                this.mazes[7][14 * w / 23, i] = true;
            for (int i = h * 8 / 13; i <= h * 10 / 13; i++)
                this.mazes[7][14 * w / 23, i] = true;
            for (int i = h * 8 / 13; i <= h * 9 / 13; i++)
                this.mazes[7][18 * w / 23, i] = true;
        }
    }
}

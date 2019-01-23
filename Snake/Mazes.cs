using System.Collections.Generic;
using System.Drawing;

namespace Snake
{
    /// <summary>
    /// Data structure for the mazes. Each of the eight mazes is created from a template to fit the playing field size.
    /// </summary>
    class Mazes
    {
        private List<Point>[] mazes;

        /// <summary>
        /// Maze data structure. Each element is the coordinate of a single obstacle.
        /// </summary>
        /// <param name="i">The maze to be returned.</param>
        /// <returns>The i-th maze.</returns>
        public List<Point> this[int i]
        {
            get { return this.mazes[i]; }
        }

        /// <summary>
        /// The snake's head initial position for each maze.
        /// </summary>
        public Point[] Head;

        /// <summary>
        /// Constructor. Creates the scaled mazes and the snake's head initial position.
        /// </summary>
        /// <param name="w">Width of the playing field.</param>
        /// <param name="h">Height of the playing field.</param>
        public Mazes(int w, int h)
        {
            this.mazes = new List<Point>[8];
            this.Head = new Point[8];

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
                this.mazes[i] = new List<Point>();

            // Maze 1:
            for (int i = 0; i < w; i++)
            {
                this.mazes[1].Add(new Point(i, 0));
                this.mazes[1].Add(new Point(i, h - 1));
            }
            for (int i = 0; i < h; i++)
            {
                this.mazes[1].Add(new Point(0, i));
                this.mazes[1].Add(new Point(w - 1, i));
            }

            // Maze 2:
            for (int i = 0; i < w * 2 / 23; i++)
            {
                this.mazes[2].Add(new Point(i, 0));
                this.mazes[2].Add(new Point(i, h - 1));
                this.mazes[2].Add(new Point(w - i - 1, 0));
                this.mazes[2].Add(new Point(w - i - 1, h - 1));
            }
            for (int i = 0; i < h * 2 / 13; i++)
            {
                this.mazes[2].Add(new Point(0, i));
                this.mazes[2].Add(new Point(w - 1, i));
                this.mazes[2].Add(new Point(0, h - i - 1));
                this.mazes[2].Add(new Point(w - 1, h - i - 1));
            }
            for (int i = 0; i < w * 9 / 23; i++)
            {
                this.mazes[2].Add(new Point(7 * w / 23 + i, 4 * h / 13));
                this.mazes[2].Add(new Point(7 * w / 23 + i, 8 * h / 13));
            }

            // Maze 3:
            for (int i = 0; i < w * 8 / 23; i++)
                this.mazes[3].Add(new Point(i, h * 10 / 13));
            for (int i = w * 14 / 23; i < w; i++)
                this.mazes[3].Add(new Point(i, h * 2 / 13));
            for (int i = 0; i < h * 6 / 13; i++)
                this.mazes[3].Add(new Point(w * 8 / 23, i));
            for (int i = h * 6 / 13; i < h; i++)
                this.mazes[3].Add(new Point(w * 14 / 23, i));

            // Maze 4:
            for (int i = 0; i < w; i++)
            {
                this.mazes[4].Add(new Point(i, 0));
                this.mazes[4].Add(new Point(i, h - 1));
            }
            for (int i = 0; i <= 4 * h / 13; i++)
            {
                this.mazes[4].Add(new Point(0, i));
                this.mazes[4].Add(new Point(w - 1, i));
            }
            for (int i = 0; i < 5 * h / 13; i++)
            {
                this.mazes[4].Add(new Point(0, h - i - 1));
                this.mazes[4].Add(new Point(w - 1, h - i - 1));
            }
            for (int i = h * 2 / 13; i <= h * 4 / 13; i++)
            {
                this.mazes[4].Add(new Point(6 * w / 23, i));
                this.mazes[4].Add(new Point(16 * w / 23, i));
            }
            for (int i = h * 8 / 13; i <= h * 10 / 13; i++)
            {
                this.mazes[4].Add(new Point(6 * w / 23, i));
                this.mazes[4].Add(new Point(16 * w / 23, i));
            }
            for (int i = w * 8 / 23; i <= w * 14 / 23; i++)
            {
                this.mazes[4].Add(new Point(i, 3 * h / 13));
                this.mazes[4].Add(new Point(i, 9 * h / 13));
            }

            // Maze 5:
            for (int i = 0; i < w; i++)
                this.mazes[5].Add(new Point(i, 7 * h / 13));
            for (int i = 7 * h / 13; i < h; i++)
                this.mazes[5].Add(new Point(12 * w / 23, i));
            for (int i = 0; i < h * 4 / 13; i++)
                this.mazes[5].Add(new Point(10 * w / 23, i));
            for (int i = 0; i < h / 13; i++)
                this.mazes[5].Add(new Point(0, i));
            for (int i = 0; i < w * 2 / 23; i++)
                this.mazes[5].Add(new Point(i, 0));
            for (int i = w * 5 / 23; i < w * 19 / 23; i++)
                this.mazes[5].Add(new Point(i, 0));
            for (int i = w * 13 / 23; i < w; i++)
                this.mazes[5].Add(new Point(i, 4 * h / 13));
            for (int i = 0; i <= w * 10 / 23; i++)
                this.mazes[5].Add(new Point(i, 4 * h / 13));

            // Maze 6:
            for (int i = 0; i < w; i++)
                this.mazes[6].Add(new Point(i, h / 2));
            for (int i = 0; i < h; i++)
                this.mazes[6].Add(new Point(w / 2, i));
            for (int i = w * 10 / 23; i < w * 13 / 23; i++)
                this.mazes[6].Add(new Point(i, h * 2 / 13));
            for (int i = w * 10 / 23; i < w * 13 / 23; i++)
                this.mazes[6].Add(new Point(i, h * 10 / 13));
            for (int i = h * 5 / 13; i < h * 8 / 13; i++)
                this.mazes[6].Add(new Point(w * 7 / 23, i));
            for (int i = h * 5 / 13; i < h * 8 / 13; i++)
                this.mazes[6].Add(new Point(w * 16 / 23, i));

            // Maze 7:
            for (int i = w * 14 / 23; i <= w * 16 / 23; i++)
                this.mazes[7].Add(new Point(i, h / 13));
            for (int i = 0; i <= w * 11 / 23; i++)
                this.mazes[7].Add(new Point(i, h * 4 / 13));
            for (int i = w * 14 / 23; i < w; i++)
                this.mazes[7].Add(new Point(i, h * 4 / 13));
            for (int i = w * 2 / 23; i <= w * 8 / 23; i++)
                this.mazes[7].Add(new Point(i, h * 8 / 13));
            for (int i = w * 11 / 23; i < w; i++)
                this.mazes[7].Add(new Point(i, h * 8 / 13));
            for (int i = w * 6 / 23; i <= w * 8 / 23; i++)
                this.mazes[7].Add(new Point(i, h * 11 / 13));
            for (int i = h * 3 / 13; i <= h * 4 / 13; i++)
                this.mazes[7].Add(new Point(4 * w / 23, i));
            for (int i = h * 2 / 13; i <= h * 4 / 13; i++)
                this.mazes[7].Add(new Point(8 * w / 23, i));
            for (int i = h * 6 / 13; i < h; i++)
                this.mazes[7].Add(new Point(8 * w / 23, i));
            for (int i = 0; i <= h * 5 / 13; i++)
                this.mazes[7].Add(new Point(14 * w / 23, i));
            for (int i = h * 8 / 13; i <= h * 10 / 13; i++)
                this.mazes[7].Add(new Point(14 * w / 23, i));
            for (int i = h * 8 / 13; i <= h * 9 / 13; i++)
                this.mazes[7].Add(new Point(18 * w / 23, i));
        }
    }
}

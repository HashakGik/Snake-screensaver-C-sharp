using System;

namespace Snake
{
    class Snake
    {
        private int[,] field;
        private int score;
        private System.Drawing.Point head;
        private System.Drawing.Point bonus;
        private int length;
        private Random r;
        private enum direction_t { D_NORTH, D_WEST, D_SOUTH, D_EAST};
        private direction_t direction;
        public delegate void GameOver();

        /// <summary>
        /// Enumeration for drawing each element differently.
        /// </summary>
        public enum fieldObjects_t { F_EMPTY, F_HEAD_V, F_HEAD_H, F_HEAD_OPEN_V, F_HEAD_OPEN_H, F_TAIL_V, F_TAIL_H, F_BODY_V, F_BODY_H, F_BODY_KNOT, F_FOOD, F_OBSTACLE, F_BONUS_A, F_BONUS_B, F_BONUS_C};
        /// <summary>
        /// Countdown for the bonus animal. Once it reaches zero, the animal disappears.
        /// </summary>
        private int remainingBonusTicks;

        /// <summary>
        /// Game over delegate. It will be invoked when the snake's head hits an obstacle or its body.
        /// </summary>
        public GameOver OnGameOver { get; set; }

        /// <summary>
        /// Playing field. Possible values are:
        /// >0: Snake
        /// 0: Empty
        /// -1: Food
        /// -2: Bonus animal
        /// -3: Fixed obstacle.
        /// </summary>
        public int[,] Field // REFACTOR: should become of type fieldObjects_t[,], in order to simplify drawing. The snake body will require a second data structure for the handling of the movement.
        {
            get { return this.field; }
        }

        public int Score { get
            { return this.score; }
        }
        /// <summary>
        /// Constructor without a maze. The snake is placed in the middle of the screen.
        /// </summary>
        /// <param name="w">Width of the playing field.</param>
        /// <param name="h">Height of the playing field.</param>
        /// <param name="initialLength">Initial length of the snake.</param>
        public Snake(int w, int h, int initialLength)
        {
            this.r = new Random();
            this.length = initialLength;
            this.field = new int[w, h];
            this.direction = direction_t.D_EAST;
            this.score = 0;

            // Place the snake in the middle of the playing field.
            for (int i = 0; i < this.length; i++)
                this.field[w / 2 + i, h / 2] = i + 1;
            this.head.X = w / 2;
            this.head.Y = h / 2;

            // Generate food.
            int x, y;
            do
            {
                x = this.r.Next(this.field.GetLength(0));
                y = this.r.Next(this.field.GetLength(1));
            } while (this.field[x, y] != 0);
            this.field[x, y] = -1;
        }
        /// <summary>
        /// Constructor with a maze and initial snake's position. Width and height are derived from the maze.
        /// </summary>
        /// <param name="maze">Maze to be used.</param>
        /// <param name="head">Initial snake's head position.</param>
        /// <param name="initialLength">Initial length of the snake.</param>
        public Snake(bool[,] maze, System.Drawing.Point head, int initialLength)
        {
            int w = maze.GetLength(0);
            int h = maze.GetLength(1);

            this.r = new Random();
            this.length = initialLength;
            this.score = 0;
            this.field = new int[w, h];
            this.direction = direction_t.D_EAST;

            // Generate the maze.
            for (int i = 0; i < maze.GetLength(0); i++)
                for (int j = 0; j < maze.GetLength(1); j++)
                    if (maze[i, j])
                        this.field[i, j] = -3;

            // Place the snake in the playing field at the specified position.
            this.head = head;
            for (int i = 0; i <= this.length; i++)
                this.field[this.head.X - i, this.head.Y] = this.length - i;

            // Generate food.
            int x, y;
            do
            {
                x = this.r.Next(this.field.GetLength(0));
                y = this.r.Next(this.field.GetLength(1));
            } while (this.field[x, y] != 0); // Retry until the food's position is free.
            this.field[x, y] = -1;
        }

        /// <summary>
        /// Updates the game. The snake's head moves forward and collisions are checked.
        /// </summary>
        public void Update()
        {
            // Move the snake's head forward.
            switch (this.direction)
            {
                case direction_t.D_NORTH:
                    this.head.Y = (this.head.Y + this.field.GetLength(1) - 1) % this.field.GetLength(1);
                    break;
                case direction_t.D_WEST:
                    this.head.X = (this.head.X + this.field.GetLength(0) - 1) % this.field.GetLength(0);
                    break;
                case direction_t.D_SOUTH:
                    this.head.Y = (this.head.Y + 1) % this.field.GetLength(1);
                    break;
                case direction_t.D_EAST:
                    this.head.X = (this.head.X + 1) % this.field.GetLength(0);
                    break;
            }

            if (this.field[this.head.X, this.head.Y] == 0) // The head's new position is free.
            {
                // Each body segment steps forward.
                for (int i = 0; i < this.field.GetLength(0); i++)
                    for (int j = 0; j < this.field.GetLength(1); j++)
                        if (this.field[i, j] > 0)
                            this.field[i, j]--;

                this.field[this.head.X, this.head.Y] = this.length;
            }
            else if (this.field[this.head.X, this.head.Y] == -1) // There is food in the head's new position.
            {
                this.length++;
                this.score++;
                this.field[this.head.X, this.head.Y] = this.length;

                // Generate new food.
                int x, y;
                do
                {
                    x = this.r.Next(this.field.GetLength(0));
                    y = this.r.Next(this.field.GetLength(1));
                } while (this.field[x, y] != 0); // Retry until the food's position is free.
                this.field[x, y] = -1;

                // TO DO: Randomly choose to generate a bonus animal.
            }
            else if (this.field[this.head.X, this.head.Y] == -2) // There is a bonus animal in the head's new position.
            {
                // TO DO: give bonus points.
            }
            else if (this.field[this.head.X, this.head.Y] == -3) // There is an obstacle in the head's new position.
            {
                if (this.OnGameOver != null)
                    this.OnGameOver();
            }
            else if (this.field[this.head.X, this.head.Y] < this.length) // There is the snake's body in the head's new position.
            {
                if (this.OnGameOver != null)
                    this.OnGameOver();
            }
        }

        /// <summary>
        /// Sets the head's direction for the next Update() as north.
        /// </summary>
        public void FaceNorth()
        {
            if (this.direction != direction_t.D_SOUTH)
                this.direction = direction_t.D_NORTH;
        }

        /// <summary>
        /// Sets the head's direction for the next Update() as west.
        /// </summary>
        public void FaceWest()
        {
            if (this.direction != direction_t.D_EAST)
                this.direction = direction_t.D_WEST;
        }

        /// <summary>
        /// Sets the head's direction for the next Update() as south.
        /// </summary>
        public void FaceSouth()
        {
            if (this.direction != direction_t.D_NORTH)
                this.direction = direction_t.D_SOUTH;
        }

        /// <summary>
        /// Sets the head's direction for the next Update() as east.
        /// </summary>
        public void FaceEast()
        {
            if (this.direction != direction_t.D_WEST)
                this.direction = direction_t.D_EAST;
        }
    }
}

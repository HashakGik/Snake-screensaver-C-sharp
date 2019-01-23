using System;
using System.Drawing;
using System.Collections.Generic;

namespace Snake
{
    /// <summary>
    /// Controller class. Handles the playing field elements (the snake, food, bonus animals and obstacles).
    /// </summary>
    class SnakeController
    {
        private int w;
        private int h;
        private Snake snake;
        private int score;
        private Point bonus;
        private Point food;
        private int length;
        private List<Point> maze;
        private Random r;
        public delegate void GameOver();

        /// <summary>
        /// Enumeration for drawing each element differently.
        /// </summary>
        public enum fieldObjects_t { F_EMPTY, F_HEAD_V, F_HEAD_H, F_HEAD_OPEN_V, F_HEAD_OPEN_H, F_TAIL_V, F_TAIL_H, F_BODY_V, F_BODY_H, F_BODY_KNOT, F_FOOD, F_OBSTACLE, F_BONUS_A, F_BONUS_B, F_BONUS_C };
        /// <summary>
        /// Countdown for the bonus animal. Once it reaches zero, the animal disappears.
        /// </summary>
        public int RemainingBonusTicks;

        /// <summary>
        /// Game over delegate. It will be invoked when the snake's head hits an obstacle or its body.
        /// </summary>
        public GameOver OnGameOver { get; set; }

        /// <summary>
        /// Playing field represented as a list of occupied blocks.
        /// </summary>
        public List<Point> Field // REFACTOR: should become of type List<KeyValuePair<Point, fieldObjects_t>>.
        {
            get {
                List<Point> ret = new List<Point>();

                foreach (Point p in this.snake.Body)
                    ret.Add(p);

                foreach (Point p in this.maze)
                    ret.Add(p);

                ret.Add(this.food);

                if (this.RemainingBonusTicks > 0)
                    ret.Add(this.bonus);

                return ret;
            }
        }

        public int Score
        {
            get { return this.score; }
        }

        /// <summary>
        /// Constructor without a maze. The snake is placed in the middle of the screen.
        /// </summary>
        /// <param name="w">Width of the playing field.</param>
        /// <param name="h">Height of the playing field.</param>
        /// <param name="initialLength">Initial length of the snake.</param>
        public SnakeController(int w, int h, int initialLength)
        {
            this.snake = new Snake(initialLength, Snake.Direction_t.EAST, new Point(w / 2, h / 2), w, h);
            this.maze = new List<Point>(); // Empty maze

            this.r = new Random();
            this.length = initialLength;
            this.w = w;
            this.h = h;
            this.score = 0;

            
            // Generate food.
            bool ok = true;
            do
            {
                this.food = new Point(this.r.Next(w), this.r.Next(h));

                for (int i = 0; ok && i < this.snake.Body.Count; i++)
                    ok &= this.food.X != this.snake.Body[i].X || this.food.Y != this.snake.Body[i].Y;
                for (int i = 0; ok && i < this.maze.Count; i++)
                    ok &= this.food.X != this.maze[i].X || this.food.Y != this.maze[i].Y;
            } while (!ok);

            // Generate bonus animal.
            if (this.r.Next(3) == 1)
            {
                this.RemainingBonusTicks = 100;
                ok = true;
                do
                {
                    this.bonus = new Point(this.r.Next(w), this.r.Next(h));

                    for (int i = 0; ok && i < this.snake.Body.Count; i++)
                        ok &= this.bonus.X != this.snake.Body[i].X || this.bonus.Y != this.snake.Body[i].Y;
                    for (int i = 0; ok && i < this.maze.Count; i++)
                        ok &= this.bonus.X != this.maze[i].X || this.bonus.Y != this.maze[i].Y;

                    ok &= this.bonus.X != this.food.X || this.bonus.Y != this.food.Y;
                } while (!ok);
            }
            else
                this.bonus = new Point();

        }
        /// <summary>
        /// Constructor with a maze and initial snake's position.
        /// </summary>
        /// <param name="maze">Maze to be used.</param>
        /// <param name="head">Initial snake's head position.</param>
        /// <param name="initialLength">Initial length of the snake.</param>
        public SnakeController(int w, int h, int initialLength, List<Point> maze, Point head)
        {
            this.snake = new Snake(initialLength, Snake.Direction_t.EAST, head, w, h);

            this.w = w;
            this.h = h;

            this.r = new Random();
            this.length = initialLength;
            this.score = 0;

            this.maze = maze;


            bool ok = true;
            do
            {
                this.food = new Point(this.r.Next(w), this.r.Next(h));

                for (int i = 0; ok && i < this.snake.Body.Count; i++)
                    ok &= this.food.X != this.snake.Body[i].X || this.food.Y != this.snake.Body[i].Y;
                for (int i = 0; ok && i < this.maze.Count; i++)
                    ok &= this.food.X != this.maze[i].X || this.food.Y != this.maze[i].Y;
            } while (!ok);

            if (this.r.Next(3) == 1)
            {
                this.RemainingBonusTicks = 100;
                ok = true;
                do
                {
                    this.bonus = new Point(this.r.Next(w), this.r.Next(h));

                    for (int i = 0; ok && i < this.snake.Body.Count; i++)
                        ok &= this.bonus.X != this.snake.Body[i].X || this.bonus.Y != this.snake.Body[i].Y;
                    for (int i = 0; ok && i < this.maze.Count; i++)
                        ok &= this.bonus.X != this.maze[i].X || this.bonus.Y != this.maze[i].Y;

                    ok &= this.bonus.X != this.food.X || this.bonus.Y != this.food.Y;
                } while (!ok);
            }
            else
                this.bonus = new Point();
        }

        /// <summary>
        /// Updates the game. The snake's head moves forward and collisions are checked.
        /// </summary>
        public void Update()
        {
            if (this.RemainingBonusTicks > 0)
                this.RemainingBonusTicks--;

            if (this.snake.CheckSelfCollision() || this.snake.CheckCollision(this.maze))
            {
                if (this.OnGameOver != null)
                    this.OnGameOver();
            }

            else if (this.snake.Body[0].X == this.food.X && this.snake.Body[0].Y == this.food.Y) // There is food in the head's new position.
            {
                this.snake.Eat();
                this.score++;

                // Generate new food.
                bool ok = true;
                do
                {
                    this.food = new Point(this.r.Next(this.w), this.r.Next(this.h));

                    for (int i = 0; ok && i < this.snake.Body.Count; i++)
                        ok &= this.food.X != this.snake.Body[i].X || this.food.Y != this.snake.Body[i].Y;
                    for (int i = 0; ok && i < this.maze.Count; i++)
                        ok &= this.food.X != this.maze[i].X || this.food.Y != this.maze[i].Y;
                    if (this.RemainingBonusTicks > 0)
                        ok &= this.bonus.X != this.food.X || this.bonus.Y != this.food.Y;
                } while (!ok);

                // Generate new bonus animal
                if (this.r.Next(3) == 1 && this.RemainingBonusTicks == 0)
                {
                    this.RemainingBonusTicks = 100;
                    ok = true;
                    do
                    {
                        this.bonus = new Point(this.r.Next(w), this.r.Next(h));

                        for (int i = 0; ok && i < this.snake.Body.Count; i++)
                            ok &= this.bonus.X != this.snake.Body[i].X || this.bonus.Y != this.snake.Body[i].Y;
                        for (int i = 0; ok && i < this.maze.Count; i++)
                            ok &= this.bonus.X != this.maze[i].X || this.bonus.Y != this.maze[i].Y;

                        ok &= this.bonus.X != this.food.X || this.bonus.Y != this.food.Y;
                    } while (!ok);
                }
                else
                    this.bonus = new Point();
            }
            else if (this.snake.Body[0].X == this.bonus.X && this.snake.Body[0].Y == this.bonus.Y && this.RemainingBonusTicks > 0)
            {
                this.RemainingBonusTicks = 0;
                this.score += 10;
                this.snake.Eat();
            }

            this.snake.Move();
        }

        /// <summary>
        /// Steers the snake to its left.
        /// </summary>
        public void SteerLeft()
        {
            this.snake.SteerLeft();
        }

        /// <summary>
        /// Steers the snake to its right.
        /// </summary>
        public void SteerRight()
        {
            this.snake.SteerRight();
        }

        /// <summary>
        /// Makes the snake's head point north.
        /// </summary>
        public void FaceNorth()
        {
            this.snake.FaceNorth();
        }

        /// <summary>
        /// Makes the snake's head point east.
        /// </summary>
        public void FaceEast()
        {
            this.snake.FaceEast();
        }

        /// <summary>
        /// Makes the snake's head point south.
        /// </summary>
        public void FaceSouth()
        {
            this.snake.FaceSouth();
        }

        /// <summary>
        /// Makes the snake's head point west.
        /// </summary>
        public void FaceWest()
        {
            this.snake.FaceWest();
        }
    }
}

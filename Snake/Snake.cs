using System.Collections.Generic;
using System.Drawing;

namespace Snake
{
    /// <summary>
    /// Snake class. Keeps track of each body segment.
    /// </summary>
    class Snake
    {
        private int w;
        private int h;
        /// <summary>
        /// List of body segments.
        /// </summary>
        public List<Point> Body
        {
            get; set;
        }
        /// <summary>
        /// Direction enumeration.
        /// </summary>
        public enum Direction_t { NORTH = 0, EAST = 1, SOUTH = 2, WEST = 3};

        /// <summary>
        /// Current direction of the snake's head.
        /// </summary>
        public Direction_t Direction
        {
            get;
            private set;
        }
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="length">Initial snake's length.</param>
        /// <param name="direction">Initial snake's direction.</param>
        /// <param name="position">Initial head position.</param>
        /// <param name="w">Width of the playing field (required for wrapping the snake around the boundaries).</param>
        /// <param name="h">Height of the playing field (required for wrapping the snake around the boundaries).</param>
        public Snake(int length, Direction_t direction, Point position, int w, int h)
        {
            Point tmp = position;
            this.Body = new List<Point>();
            this.w = w;
            this.h = h;

            for (int i = 0; i < length; i++)
            {
                switch (direction)
                {
                    case Direction_t.NORTH:
                        tmp.Y = position.Y + i;
                        break;
                    case Direction_t.EAST:
                        tmp.X = position.X - i;
                        break;
                    case Direction_t.SOUTH:
                        tmp.Y = position.Y - i;
                        break;
                    case Direction_t.WEST:
                        tmp.X = position.X + i;
                        break;
                }
                this.Body.Add(tmp);
            }

            this.Direction = direction;
        }

        /// <summary>
        /// Moves the snake forward.
        /// </summary>
        public void Move()
        {
            Point tmp;
            for (int i = this.Body.Count - 1; i > 0; i--)
            {
                this.Body[i] = this.Body[i - 1];
            }
            tmp = this.Body[0];
            switch (this.Direction)
            {
                case Direction_t.NORTH:
                    tmp.Y = (tmp.Y + h - 1) % this.h;
                    break;
                case Direction_t.EAST:
                    tmp.X = (tmp.X + 1) % this.w;
                    break;
                case Direction_t.SOUTH:
                    tmp.Y = (tmp.Y + 1) % this.h;
                    break;
                case Direction_t.WEST:
                    tmp.X = (tmp.X + w - 1) % this.w;
                    break;
            }
            this.Body[0] = tmp;
        }

        /// <summary>
        /// Elongates the snake by one segment.
        /// </summary>
        public void Eat()
        {
            Point tmp = this.Body[0];

            switch (this.Direction)
            {
                case Direction_t.NORTH:
                    tmp.Y += 1;
                    break;
                case Direction_t.EAST:
                    tmp.X -= 1;
                    break;
                case Direction_t.SOUTH:
                    tmp.Y -= 1;
                    break;
                case Direction_t.WEST:
                    tmp.X += 1;
                    break;
            }

            this.Body.Insert(0, tmp);
        }

        /// <summary>
        /// Checks if the snake's head collides with its body.
        /// </summary>
        /// <returns>True if there is a collision.</returns>
        public bool CheckSelfCollision()
        {
            bool ret = false;
            for (int i = 1; !ret && i < this.Body.Count; i++)
            {
                ret |= (this.Body[0].X == this.Body[i].X && this.Body[0].Y == this.Body[i].Y);
            }

            return ret;
        }
        /// <summary>
        /// Checks if the snake's head collides with an obstacle.
        /// </summary>
        /// <param name="obstacles">List of obstacles.</param>
        /// <returns>True if there is a collision.</returns>
        public bool CheckCollision(List<Point> obstacles)
        {
            bool ret = false;

            for (int i = 0; !ret && i < obstacles.Count; i++)
                ret |= this.Body[0].X == obstacles[i].X && this.Body[0].Y == obstacles[i].Y;

            return ret;
        }
        /// <summary>
        /// Steers the snake to its left.
        /// </summary>
        public void SteerLeft()
        {
            this.Direction = (Direction_t)(((int)this.Direction + 3) % 4);
        }
        /// <summary>
        /// Steers the snake to its right.
        /// </summary>
        public void SteerRight()
        {
            this.Direction = (Direction_t)(((int)this.Direction + 1) % 4);
        }
        /// <summary>
        /// Makes the snake's head point north.
        /// </summary>
        public void FaceNorth()
        {
            if (this.Direction != Direction_t.SOUTH)
                this.Direction = Direction_t.NORTH;
        }
        /// <summary>
        /// Makes the snake's head point east.
        /// </summary>
        public void FaceEast()
        {
            if (this.Direction != Direction_t.WEST)
                this.Direction = Direction_t.EAST;
        }
        /// <summary>
        /// Makes the snake's head point south.
        /// </summary>
        public void FaceSouth()
        {
            if (this.Direction != Direction_t.NORTH)
                this.Direction = Direction_t.SOUTH;
        }
        /// <summary>
        /// Makes the snake's head point west.
        /// </summary>
        public void FaceWest()
        {
            if (this.Direction != Direction_t.EAST)
                this.Direction = Direction_t.WEST;
        }
    }
}

using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Snake
{
    /// <summary>
    /// Game form. It will be shown in screensaver and preview modes.
    /// </summary>
    public partial class Form1 : Form
    {
        [DllImport("user32.dll")]
        static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);
        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);
        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        static extern bool GetClientRect(IntPtr hWnd, out Rectangle lpRect);

        private Snake s;
        private bool gameover;
        private int i;
        private bool preview;
        private int x = -1;
        private int y = -1;
        private int speed;
        private int fieldWidth;
        private int fieldHeight;

        /// <summary>
        /// Initializes the game. It loads the current settings, creates the Snake object and attaches a delegate to the game over.
        /// </summary>
        private void Init()
        {
            Properties.Settings.Default.Reload();
            this.fieldWidth = Properties.Settings.Default.Width;
            this.fieldHeight = Properties.Settings.Default.Height;
            this.speed = Properties.Settings.Default.Speed;
            this.timer1.Interval = 20 * (10 - this.speed);

            Mazes m = new Mazes(this.fieldWidth, this.fieldHeight);

            this.i = 0;
            this.gameover = false;

            this.s = new Snake(m[this.i], m.Head[this.i], 7);

            // Creates the game over delegate: it changes the timer's interval to 5 seconds and notifies the Paint event of the game over, finally it creates a new Snake instance with the next maze and attaches itself as the game over delegate for the new instance.
            Snake.GameOver D = () => {
                this.i = (this.i + 1) % 8;
                this.gameover = true;
                timer1.Interval = 5000;
                this.s = new Snake(m[this.i], m.Head[this.i], 7);
                timer1.Enabled = true;
            };
            D += delegate { this.s.OnGameOver += D; };

            this.s.OnGameOver += D;
        }

        /// <summary>
        /// Constructor invoked when in screensaver mode.
        /// </summary>
        public Form1()
        {
            InitializeComponent();

            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            Cursor.Hide();
            
            this.Init();

            this.preview = false;
        }

        /// <summary>
        /// Constructor invoked when in preview mode.
        /// </summary>
        /// <param name="handle">Handle to the parent window.</param>
        public Form1(IntPtr handle)
        {
            InitializeComponent();
            SetParent(this.Handle, handle);
            SetWindowLong(this.Handle, -16, new IntPtr(GetWindowLong(this.Handle, -16) | 0x40000000));
            Rectangle parent;
            GetClientRect(handle, out parent);
            parent.Height += 50;
            this.MaximumSize = parent.Size;
            this.Location = new Point(0, -50);
            this.FormBorderStyle = FormBorderStyle.None;

            this.Init();

            this.preview = true;
        }

        /// <summary>
        /// Paint event handler. During a game over it displays "Game over" at the center of the form, otherwise  it draws playing field, current maze number, current score and current speed.
        /// The playing field is scaled so that fills the entire form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            int blockW = this.Width / this.fieldWidth;
            int blockH = (this.Height - 50) / this.fieldHeight;

            int xShift = (this.Width - this.fieldWidth * blockW) / 2;
            int yShift = (this.Height - this.fieldHeight * blockH) / 2;

            StringFormat f = new StringFormat();
            f.LineAlignment = StringAlignment.Center;
            f.Alignment = StringAlignment.Center;

            if (!this.gameover)
            {
                e.Graphics.DrawString("Maze: " + this.i, DefaultFont, Brushes.Black, new Rectangle(0, 0, this.Width / 3, 50), f);
                e.Graphics.DrawString("Score: " + this.s.Score, DefaultFont, Brushes.Black, new Rectangle(this.Width / 3, 0, this.Width / 3, 50), f);
                e.Graphics.DrawString("Speed: " + this.speed, DefaultFont, Brushes.Black, new Rectangle(2 * this.Width / 3, 0, this.Width / 3, 50), f);
                e.Graphics.DrawRectangle(Pens.Black, xShift, yShift, this.fieldWidth * blockW, this.fieldHeight * blockH);

                for (int i = 0; i < this.fieldWidth; i++)
                    for (int j = 0; j < this.fieldHeight; j++)
                    {
                        if (this.s.Field[i, j] != 0)
                            e.Graphics.FillRectangle(Brushes.Black, xShift + i * blockW, yShift + j * blockH, blockW, blockH);
                    }
            }
            else
                e.Graphics.DrawString("Game over", DefaultFont, Brushes.Black, new Rectangle(0, 0, this.Width, this.Height), f);
        }

        /// <summary>
        /// Timer's tick event handler. If it ticks during a game over, it restores the original interval and restores the playing mode, otherwise it updates the Snake instance.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!this.gameover)
                this.s.Update();
            else
            {
                this.gameover = false;
                this.timer1.Interval = 20 * (10 - this.speed);
            }

            this.Invalidate();
        }

        /// <summary>
        /// Keyboard event handler. If in screensaver mode, any key other than the arrows closes the application.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    this.s.FaceNorth();
                    break;
                case Keys.Down:
                    this.s.FaceSouth();
                    break;
                case Keys.Left:
                    this.s.FaceWest();
                    break;
                case Keys.Right:
                    this.s.FaceEast();
                    break;
                case Keys.Escape:
                    Application.Exit();
                    break;
                default:
                    if (!this.preview)
                        Application.Exit();
                    break;
            }
        }

        /// <summary>
        /// Mouse event handler. If the mouse moves in screensaver mode, the application is closed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (x == -1 && y == -1)
            {
                x = e.X;
                y = e.Y;
            }

            if (Math.Abs(e.X - x) > 0 || Math.Abs(e.Y - y) > 0)
                if (!this.preview)
                    Application.Exit();
        }
    }
}

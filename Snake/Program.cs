using System;
using System.Windows.Forms;

namespace Snake
{
    /// <summary>
    /// This program is a draft for an interactive screensaver which implements the Snake game (Nokia 3310 version).
    /// For more informations about screensavers, see the Screensaver solution.
    /// In order to work correctly, it must be compiled in the same architecture of the OS (untick "Prefer 32 bit" in the build options), renamed with a .scr extension and saved in C:/Windows/System32
    /// </summary>
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// <param name="args">Flags: /p preview mode, /s screensaver mode, /c configuration mode</param>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Form f;
            if (args.Length > 0)
            {
                switch (args[0])
                {
                    case "/p":
                        if (args.Length > 1)
                        {
                            f = new Form1(new IntPtr(long.Parse(args[1])));
                            f.Show();
                            Application.Run(f);
                        }
                        break;
                    case "/s":
                        f = new Form1();
                        f.Show();
                        Application.Run(f);
                        break;
                    case "/c":
                    default:
                        f = new Settings();
                        f.Show();
                        Application.Run(f);
                        break;
                }
            }
            else
            {
                f = new Form1();
                f.Show();
                Application.Run(f);
            }
        }
    }
}

using System;
using System.Windows.Forms;

namespace Snake
{
    /// <summary>
    /// Configuration form. It allows to configure the playing field size and the game speed.
    /// </summary>
    public partial class Settings : Form
    {
        public Settings()
        {
            InitializeComponent();
        }
        /// <summary>
        /// About button event handler. Runs the About form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            new AboutBox1().ShowDialog();
        }
        /// <summary>
        /// Cancel button event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Application.Exit();
        }
        /// <summary>
        /// OK button event handler. It stores the values in the application settings.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Width = (int) this.numericUpDown1.Value;
            Properties.Settings.Default.Height = (int) this.numericUpDown2.Value;
            Properties.Settings.Default.Speed = (int)this.numericUpDown3.Value;

            Properties.Settings.Default.Save();

            this.DialogResult = DialogResult.OK;
            Application.Exit();
        }
        /// <summary>
        /// Form loading event handler. It retrieves the values from the application settings.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Settings_Load(object sender, EventArgs e)
        {
            Properties.Settings.Default.Reload();
            this.numericUpDown1.Value = Properties.Settings.Default.Width;
            this.numericUpDown2.Value = Properties.Settings.Default.Height;
            this.numericUpDown3.Value = Properties.Settings.Default.Speed;
        }
    }
}

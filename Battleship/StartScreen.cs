using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Battleship
{
    public partial class StartScreen : Form
    {
        public StartScreen()
        {
            InitializeComponent();
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void AIButton_Click(object sender, EventArgs e)
        {
            Hide();
            Form Game = new MainScreen(true);
            Game.ShowDialog();
            Dispose();
        }

        private void MPButton_Click(object sender, EventArgs e)
        {
            Hide();
            Form Game = new MainScreen(false);
            Game.ShowDialog();
            Dispose();
        }
    }
}

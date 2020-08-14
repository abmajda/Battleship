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
    public partial class MainScreen : Form
    {
        bool AIGame;

        public MainScreen(bool AI)
        {
            InitializeComponent();

            if (AI)
                AIGame = true;
            else
                AIGame = false;
        }
    }
}

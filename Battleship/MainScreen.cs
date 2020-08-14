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
        public MainScreen(bool AISelection)
        {
            InitializeComponent();

            // set the flag for if this is an AI game or not
            if (AISelection)
            {
                PlayAIGame();
            }
            else
            {
                // implement MP game here
            }
        }

        // start a game against the AI
        public void PlayAIGame()
        {

        }
    }
}

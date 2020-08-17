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
    // a struct to keep track of ship information for placement purposes
    public struct ShipInfo
    {
        public string symbol { get; }
        public int size { get; }

        public ShipInfo(string Symbol, int Size)
        {
            symbol = Symbol;
            size = Size;
        }
    }

    public partial class MainScreen : Form
    {
        // a queue of ships that have not been placed yet
        Queue<ShipInfo> shipsToPlace;
        // a list of coordinates to keep track of where to display the ship outline
        List<Coords> placementOutline;
        // a boolean to check if the position for placement is valid
        bool validPlacement = false;
        // a placeholder for an AI to be initialized if play against the aI is selected
        AIOpponent AI;


        public MainScreen(bool AISelection)
        {
            InitializeComponent();

            placementOutline = new List<Coords>();

            // set up the queue of ships to place
            shipsToPlace = new Queue<ShipInfo>();
            shipsToPlace.Enqueue(new ShipInfo("C", 5));
            shipsToPlace.Enqueue(new ShipInfo("B", 4));
            shipsToPlace.Enqueue(new ShipInfo("D", 3));
            shipsToPlace.Enqueue(new ShipInfo("S", 3));
            shipsToPlace.Enqueue(new ShipInfo("P", 2));

            // set the flag for if this is an AI game or not
            if (AISelection)
            {
                AI = new AIOpponent();
            }
            else
            {
                // implement MP game here
            }
        }

        private void PositionClick(object sender, EventArgs e)
        {
            // consider adding a check for type in the future to made it more modular
            Label selected = (Label)sender;
            ShipInfo ship = shipsToPlace.Peek();
            int size = ship.size;
            int startRow = BoardTable.GetRow(selected);
            int startColumn = BoardTable.GetColumn(selected);

            validPlacement = true;

            // display horizontal if this is the first click or if the label is the same one clicked 
            if (placementOutline.Count == 0 || ((placementOutline[0].x != BoardTable.GetRow(selected)) || (placementOutline[0].y != BoardTable.GetColumn(selected))))
            {
                // get rid of the old outline
                resetOutline();

                if (size > (10 - startColumn))
                {
                    size = (10 - startColumn);
                    validPlacement = false;
                }

                // make a new outline, checking if it's valid
                for (int i = 0; i < size; i++)
                {
                    placementOutline.Add(new Coords(startRow, (startColumn + i)));

                    // if the space has a ship already in it mark as invalid
                    if (BoardTable.GetControlFromPosition((startColumn + i), startRow).Text != "")
                        validPlacement = false;
                }
            }
            // otherwise flip it to vertical
            else
            {
                // get rid of the old outline
                resetOutline();

                if (size > (10 - startRow))
                {
                    size = (10 - startRow);
                    validPlacement = false;
                }

                // make a new outline, checking if it's valid
                for (int i = 0; i < size; i++)
                {
                    placementOutline.Add(new Coords((startRow + i), startColumn));

                    // if the space has a ship already in it mark as invalid
                    if (BoardTable.GetControlFromPosition(startColumn, (startRow + i)).Text != "")
                        validPlacement = false;
                }
            }

            // display the placeholder on the board
            foreach (Coords coords in placementOutline)
            {
                Control label = BoardTable.GetControlFromPosition(coords.y, coords.x);
                if (validPlacement)
                    label.BackColor = Color.Lime;
                else
                    label.BackColor = Color.Red;
            }

        }

        // reset every label in the current outline to the normal background color
        private void resetOutline()
        {
            foreach (Coords coords in placementOutline)
            {
                Control label = BoardTable.GetControlFromPosition(coords.y, coords.x);
                label.BackColor = Color.DodgerBlue;
            }

            placementOutline.Clear();
        }

        // exits the game
        private void CloseButton_Click(object sender, EventArgs e)
        {
            // ** send some forfeit if multiplayer ***
            Close();
        }

        // place the ship if the placement is valid
        private void PlaceButton_Click(object sender, EventArgs e)
        {
            if (!validPlacement)
            {
                MessageBox.Show("That is an invalid placement for that ship.", "Warning");
            }
            else
            {
                ShipInfo ship = shipsToPlace.Dequeue();
                string symbol = ship.symbol;

                // for every coordinate reset the background color and set the symbol
                foreach (Coords coords in placementOutline)
                {
                    Control label = BoardTable.GetControlFromPosition(coords.y, coords.x);
                    label.BackColor = Color.DodgerBlue;
                    label.Text = symbol;
                }
            }

            // if all ships are placed, start the game
            if (shipsToPlace.Count == 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        BoardTable.GetControlFromPosition(i, j).Enabled = false;
                        OpponentBoard.GetControlFromPosition(i, j).Enabled = true;
                    }
                }

                PlaceButton.Enabled = false;
            }
        }

        private void TakeShot(object sender, EventArgs e)
        {
            // consider adding a check for type in the future to made it more modular
            Label selected = (Label)sender;

            // create a Coords for the player shot and send it to the opponent
            if (selected.BackColor == Color.DodgerBlue)
            {
                Coords playerShot = new Coords(OpponentBoard.GetRow(selected), OpponentBoard.GetColumn(selected));
                int result = AI.ResolveShot(playerShot);
                if (result == 0)
                {
                    selected.BackColor = Color.AliceBlue;
                }
                else
                {
                    selected.BackColor = Color.Red;
                    // temporary code
                    if (result > 1)
                    {
                        MessageBox.Show("You sunk a ship", "woohoo");
                    }
                }
            }
            // if the have already selected that space it will not be the normal color and we can discout their click
            else
                return;
        }
    }
}

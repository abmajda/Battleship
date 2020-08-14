using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    // holds an x and y coordinate, used to de spaghettify the code a bit
    public struct Coords
    {
        public int x { get; }
        public int y { get; }

        public Coords(int X, int Y)
        {
            x = X;
            y = Y;
        }
    }

    abstract class Ship
    {
        protected int health;
        protected bool horizontal;
        List<Coords> location;

        // places the ship
        public Ship(Coords start, bool horizontal)
        {
            // constructor
        }

        // check if the coords provided is in the ships location
        public bool checkHit(Coords shot)
        {
            // check hit function
            return false;
        }

        // check if the ship is sunk
        public bool sunk()
        {
            if (health == 0)
                return true;
            else
                return false;
        }
    }
}

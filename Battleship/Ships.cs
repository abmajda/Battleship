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

    // the base class for all ships. Abstract class all ships will derive from it
    public abstract class Ship
    {
        protected int health;
        protected int size;
        protected List<Coords> location;

        // places the ship and initialize it's health
        public Ship(Coords start, bool horizontal)
        {
            health = size;
            location = new List<Coords>();

            if (horizontal)
            {
                for (int i = 0; i < size; i++)
                {
                    location.Add(new Coords((start.x + i), start.y));
                }
            }
            
            else
            {
                for (int i = 0; i < size; i++)
                {
                    location.Add(new Coords(start.x , (start.y + i)));
                }
            }
        }

        // check if the shot hits and subtract health if it did
        public bool CheckHit(Coords shot)
        {
            foreach (Coords space in location)
            {
                if (shot.Equals(space))
                {
                    health--;
                    return true;
                }
            }

            // if no match is found return false
            return false;
        }

        // check if the coords provided is in the ships location, do not take away health, this is used for checking collisions when placing ships
        public bool CheckCollision(Coords placed)
        {
            foreach (Coords space in location)
            {
                if (placed.Equals(space))
                    return true;
            }

            // if no match is found return false
            return false;
        }

        // check if the ship is sunk
        public bool Sunk()
        {
            if (health == 0)
                return true;
            else
                return false;
        }
    }

    // derived class for the carrier
    public class Carrier : Ship
    {
        protected new int size = 5;
        // places the ship and initialize it's health
        public Carrier(Coords start, bool horizontal) : base(start, horizontal) { }
    }

    // derived class for the Battleship
    public class Battleship : Ship
    {
        protected new int size = 4;
        // places the ship and initialize it's health
        public Battleship(Coords start, bool horizontal) : base(start, horizontal) { }
    }

    // derived class for the Destroyer
    public class Destroyer : Ship
    {
        protected new int size = 3;
        // places the ship and initialize it's health
        public Destroyer(Coords start, bool horizontal) : base(start, horizontal) { }
    }

    // derived class for the Submarine
    public class Submarine : Ship
    {
        protected new int size = 3;
        // places the ship and initialize it's health
        public Submarine(Coords start, bool horizontal) : base(start, horizontal) { }
    }

    // derived class for the PTBoat
    public class PTBoat : Ship
    {
        protected new int size = 2;
        // places the ship and initialize it's health
        public PTBoat(Coords start, bool horizontal) : base(start, horizontal) { }
    }
}

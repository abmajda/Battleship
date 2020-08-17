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

    // used as a return when validating ship placements for the AI
    public struct ShipPlacement
    {
        public Coords startPosition { get; set; }
        public bool horizontal { get; set; }

        public ShipPlacement(Coords start, bool Horizontal)
        {
            startPosition = start;
            horizontal = Horizontal;
        }
    }

    // the base class for all ships. Abstract class all ships will derive from it
    public abstract class Ship
    {
        protected int health;
        abstract public int size { get; }
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

        // check if the coords provided is in the ships location, do not take away health, this is used for checking collisions when placing ships by the AI
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
        public override int size
        {
            get { return 5; }
        }
        // places the ship and initialize it's health
        public Carrier(Coords start, bool horizontal) : base(start, horizontal)
        {
        }
    }

    // derived class for the Battleship
    public class Battleship : Ship
    {
        public override int size
        {
            get { return 4; }
        }
        // places the ship and initialize it's health
        public Battleship(Coords start, bool horizontal) : base(start, horizontal) { }
    }

    // derived class for the Destroyer
    public class Destroyer : Ship
    {
        public override int size
        {
            get { return 3; }
        }
        // places the ship and initialize it's health
        public Destroyer(Coords start, bool horizontal) : base(start, horizontal) { }
    }

    // derived class for the Submarine
    public class Submarine : Ship
    {
        public override int size
        {
            get { return 3; }
        }
        // places the ship and initialize it's health
        public Submarine(Coords start, bool horizontal) : base(start, horizontal) { }
    }

    // derived class for the PTBoat
    public class PTBoat : Ship
    {
        public override int size
        {
            get { return 2; }
        }
        // places the ship and initialize it's health
        public PTBoat(Coords start, bool horizontal) : base(start, horizontal) { }
    }
}

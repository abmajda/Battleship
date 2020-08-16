using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    class AI
    {
        private List<Coords> pastGuesses = new List<Coords>();
        private List<Ship> ships = new List<Ship>();
        Random randomizer = new Random();

        public AI()
        {
            SetupShips();
        }

        // randomly generates initial ship positions for the AI
        private void SetupShips()
        {
            ShipPlacement randomPosition;
            
            // place the carrier
            randomPosition = PlaceShip(5);
            Carrier carrier = new Carrier(randomPosition.startPosition, randomPosition.horizontal);
            ships.Add(carrier);

            // place the battleship
            randomPosition = PlaceShip(4);
            Battleship battleship = new Battleship(randomPosition.startPosition, randomPosition.horizontal);
            ships.Add(battleship);

            // place the destroyer
            randomPosition = PlaceShip(3);
            Destroyer destroyer = new Destroyer(randomPosition.startPosition, randomPosition.horizontal);
            ships.Add(destroyer);

            // place the submarine
            randomPosition = PlaceShip(3);
            Submarine submarine = new Submarine(randomPosition.startPosition, randomPosition.horizontal);
            ships.Add(submarine);

            // place the PT boat
            randomPosition = PlaceShip(2);
            PTBoat ptboat = new PTBoat(randomPosition.startPosition, randomPosition.horizontal);
            ships.Add(ptboat);
        }
        
        // resolves the shot against the player
        public int ResolveShot(Coords playerShot)
        {
            foreach (Ship ship in ships)
            {
                if (ship.CheckHit(playerShot))
                {
                    // if we sunk a ship, report it's ship code to announce it is sunkr
                    if (ship.Sunk())
                    {
                        // Carrier ship code is 6
                        if (ship is Carrier)
                        {
                            return 6;
                        }
                        // Battleship ship code is 5
                        else if (ship is Battleship)
                        {
                            return 5;
                        }
                        // Destroyer ship code is 4
                        else if (ship is Destroyer)
                        {
                            return 4;
                        }
                        // Submarine ship code is 3
                        else if (ship is Submarine)
                        {
                            return 3;
                        }
                        // PTBoat ship code is 2
                        else if (ship is PTBoat)
                        {
                            return 2;
                        }
                    }

                    // return a 1 to indicate generic hit if no ships are sunk
                    return 1;
                }
            }

            // if nothing is hit return a 0 to indicate miss
            return 0;
        }

        // placeholder for AI turns until more is done
        public Coords TakeTurn()
        {
            Coords guess = new Coords(randomizer.Next(10), randomizer.Next(10));
            bool validGuess = false;

            while (!validGuess)
            {
                if (ValidateGuess(guess))
                    validGuess = true;
                else
                    guess = new Coords(randomizer.Next(10), randomizer.Next(10));
            }

            return guess;
        }

        // go through each of the past guesses to make sure we aren't guessing a previous guess
        private bool ValidateGuess(Coords guess)
        {
            foreach (Coords space in pastGuesses)
            {
                if (guess.Equals(space))
                    return false;
            }

            return true;
        }

        // go through each placed ship and ensure there are no collisions to worry about
        private bool ValidatePlaces(ShipPlacement trial, int size)
        {
            foreach (Ship ship in ships)
            {
                if (trial.horizontal)
                {
                    for (int i = 0; i < size; i++)
                    {
                        if (ship.CheckCollision(new Coords((trial.startPosition.x + i), trial.startPosition.y)))
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < size; i++)
                    {
                        if (ship.CheckCollision(new Coords(trial.startPosition.x, (trial.startPosition.y + i))))
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        // generates a random starting position for a ship with a given size
        private ShipPlacement PlaceShip(int size)
        {
            bool horizontal;
            bool validPlacement = false;
            ShipPlacement trial = new ShipPlacement();

            horizontal = (randomizer.Next(2) == 0);
            if (horizontal)
            {
                trial.startPosition = new Coords(randomizer.Next(10 - size), randomizer.Next(10));
                trial.horizontal = true;
            }
            else
            {
                trial.startPosition = new Coords(randomizer.Next(10), randomizer.Next(10 - size));
                trial.horizontal = false;
            }

            while (!validPlacement)
            {
                if (ValidatePlaces(trial, size))
                {
                    validPlacement = true;
                }
                else
                {
                    horizontal = (randomizer.Next(2) == 0);
                    if (horizontal)
                    {
                        trial.startPosition = new Coords(randomizer.Next(10 - size), randomizer.Next(10));
                        trial.horizontal = true;
                    }
                    else
                    {
                        trial.startPosition = new Coords(randomizer.Next(10), randomizer.Next(10 - size));
                        trial.horizontal = false;
                    }
                }
            }

            return trial;
        }
    }
}

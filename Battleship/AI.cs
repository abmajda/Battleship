using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    class AIOpponent
    {
        private List<Coords> pastGuesses = new List<Coords>();
        private List<Coords> pastHits = new List<Coords>();
        private List<Coords> hitArea = new List<Coords>();
        private Queue<Coords> multiHit = new Queue<Coords>();
        private Queue<Queue<Coords>> chainedMultiHits = new Queue<Queue<Coords>>();
        private Coords directionModifier;
        private Coords hitSpot;
        private List<Ship> ships = new List<Ship>();
        private int AIstate;
        Random randomizer = new Random();

        public AIOpponent()
        {
            SetupShips();
            AIstate = 1;
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
        
        // resolves the shot from the player
        public int ResolveShot(Coords playerShot)
        {
            foreach (Ship ship in ships)
            {
                if (ship.CheckHit(playerShot))
                {
                    // if we sunk a ship, report it's ship code to announce it is sunk
                    if (ship.Sunk())
                    {
                        // if all ships are sunk
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

        // takes the result of the guess and sets state accordingly
        public void Report(int status)
        {
            // this controls the logic in case of a miss
            if (status == 0)
            {
                // this gets the last guess so we can use as a reference
                Coords guess = pastGuesses[pastGuesses.Count - 1];

                switch (AIstate)
                {
                    case 1:
                        break;
                    case 2:
                        if (hitArea.Count == 0)
                            AIstate = 1;
                        break;
                    case 3:
                        reverseDirection();
                        AIstate = 4;
                        break;
                    case 4:
                        ConvertToMultiHit();
                        pastHits.Clear();
                        hitSpot = multiHit.Dequeue();
                        pastHits.Add(hitSpot);
                        findDirectionMulti(hitSpot);
                        AIstate = 6;
                        break;
                    case 5:
                        ConvertToMultiHit();
                        pastHits.Clear();
                        hitSpot = multiHit.Dequeue();
                        pastHits.Add(hitSpot);
                        findDirectionMulti(hitSpot);
                        AIstate = 6;
                        break;
                    case 6:
                        reverseDirection();
                        AIstate = 8;
                        break;
                    case 7:
                        reverseDirection();
                        AIstate = 8;
                        break;
                    case 8:
                        SaveMultiHit();
                        break;
                    case 9:
                        SaveMultiHit();
                        break;
                    default:
                        AIstate = 1;
                        break;
                }
            }

            // this controls the logic if a hit is scored
            else if (status == 1)
            {
                // this gets the last guess so we can use as a reference
                Coords guess = pastGuesses[pastGuesses.Count - 1];

                switch(AIstate)
                {
                    case 1:
                        hitSpot = guess;
                        createHitArea(guess);
                        pastHits.Add(guess);
                        AIstate = 2;
                        break;
                    case 2:
                        pastHits.Add(guess);
                        findDirection();
                        AIstate = 3;
                        break;
                    case 3:
                        pastHits.Add(guess);
                        break;
                    case 4:
                        pastHits.Add(guess);
                        AIstate = 5;
                        break;
                    case 5:
                        pastHits.Add(guess);
                        break;
                    case 6:
                        pastHits.Add(guess);
                        AIstate = 7;
                        break;
                    case 7:
                        pastHits.Add(guess);
                        break;
                    case 8:
                        pastHits.Add(guess);
                        AIstate = 9;
                        break;
                    case 9:
                        pastHits.Add(guess);
                        break;
                    default:
                        AIstate = 1;
                        break;
                }
            }

            // this controls the logic if a ship is sunk
            else if (status > 1)
            {
                int size;
                Coords guess = pastGuesses[pastGuesses.Count - 1];
                pastHits.Add(guess);

                switch (status)
                {
                    case 2:
                        size = 2;
                        break;
                    case 3:
                        size = 3;
                        break;
                    case 4:
                        size = 3;
                        break;
                    case 5:
                        size = 4;
                        break;
                    case 6:
                        size = 5;
                        break;
                    default:
                        //something has gone horribly wrong
                        size = 0;
                        break;
                }

                switch (AIstate)
                {
                    case 1:
                        resetState();
                        AIstate = 1;
                        break;
                    case 2:
                        resetState();
                        AIstate = 1;
                        break;
                    case 3:
                        if (pastHits.Count == size)
                        {
                            resetState();
                            AIstate = 1;
                        }
                        else
                        {
                            TrimHits(size);
                            reverseDirection();
                            AIstate = 4;
                        }
                        break;
                    case 4:
                        if (pastHits.Count == size)
                        {
                            resetState();
                            AIstate = 1;
                        }
                        else
                        {
                            TrimHits(size);
                            ConvertToMultiHit();
                            pastHits.Clear();
                            hitSpot = multiHit.Dequeue();
                            pastHits.Add(hitSpot);
                            findDirectionMulti(hitSpot);
                            AIstate = 6;
                            break;
                        }
                        break;
                    case 5:
                        if (pastHits.Count == size)
                        {
                            resetState();
                            AIstate = 1;
                        }
                        else
                        {
                            TrimHits(size);
                            ConvertToMultiHit();
                            pastHits.Clear();
                            hitSpot = multiHit.Dequeue();
                            pastHits.Add(hitSpot);
                            findDirectionMulti(hitSpot);
                            AIstate = 6;
                            break;
                        }
                        break;
                    case 6:
                        if (pastHits.Count == size)
                        {
                            pastHits.Clear();
                            if (multiHit.Count == 0)
                            {
                                resetState();
                                AIstate = 1;
                            }
                            else
                            {
                                hitSpot = multiHit.Dequeue();
                                pastHits.Add(hitSpot);
                            }
                        }
                        else
                        {
                            TrimHits(size);
                            reverseDirection();
                            AIstate = 6;
                        }
                        break;
                    case 7:
                        if (pastHits.Count == size)
                        {
                            pastHits.Clear();
                            if (multiHit.Count == 0)
                            {
                                resetState();
                                AIstate = 1;
                            }
                            else
                            {
                                hitSpot = multiHit.Dequeue();
                                pastHits.Add(hitSpot);
                                AIstate = 6;
                            }
                        }
                        else
                        {
                            TrimHits(size);
                            reverseDirection();
                            AIstate = 6;
                        }
                        break;
                    case 8:
                        if (pastHits.Count == size)
                        {
                            pastHits.Clear();
                            if (multiHit.Count == 0)
                            {
                                resetState();
                                AIstate = 1;
                            }
                            else
                            {
                                hitSpot = multiHit.Dequeue();
                                pastHits.Add(hitSpot);
                            }
                        }
                        else
                        {
                            TrimHits(size);
                            SaveMultiHit();

                            // get a new multihit going
                            if (multiHit.Count == 0)
                            {
                                multiHit = chainedMultiHits.Dequeue();
                                pastHits.Clear();
                                hitSpot = multiHit.Dequeue();
                                pastHits.Add(hitSpot);
                                findDirectionMulti(hitSpot);
                                AIstate = 6;
                            }
                            // save for later
                            else
                            {
                                pastHits.Clear();
                                hitSpot = multiHit.Dequeue();
                                pastHits.Add(hitSpot);
                                AIstate = 6;
                            }
                        }
                        break;
                    case 9:
                        if (pastHits.Count == size)
                        {
                            pastHits.Clear();
                            if (multiHit.Count == 0)
                            {
                                resetState();
                                AIstate = 1;
                            }
                            else
                            {
                                hitSpot = multiHit.Dequeue();
                                pastHits.Add(hitSpot);
                                AIstate = 6;
                            }
                        }
                        else
                        {
                            TrimHits(size);
                            SaveMultiHit();

                            // get a new multihit going
                            if (multiHit.Count == 0)
                            {
                                multiHit = chainedMultiHits.Dequeue();
                                pastHits.Clear();
                                hitSpot = multiHit.Dequeue();
                                pastHits.Add(hitSpot);
                                findDirectionMulti(hitSpot);
                                AIstate = 6;
                            }
                            // save for later
                            else
                            {
                                pastHits.Clear();
                                hitSpot = multiHit.Dequeue();
                                pastHits.Add(hitSpot);
                                AIstate = 6;
                            }
                        }
                        break;
                    default:
                        resetState();
                        AIstate = 1;
                        break;
                }
            }
        }

        public Coords TakeTurn()
        {
            Coords guess = new Coords(0, 0);
            Coords lastGuess;
            bool valid = false;

            /* Handle the AI behavior based on state. The states are as follows:
             * 1 - random guesses to find a hit.
             * 2 - got a hit but searching for a path to keep getting hits
             * 3 - following the path that is found until sinking a ship or getting a miss
             * 4 - path reached end, now going the other direction to score a sink
             * 5 - continuing the direction following the reversal, similar to state 3
             * 6 - go to a multihit location and start going, similar to state 4
             * 7 - continue the direction, similar to state 3 and 5
             * 8 - When we have already flipped during the multihit stage, look out for multihits within multihits
             * 9 - contrinuing along post reverse looking out for multihits within multihits
             * */

            while (!valid)
            {
                switch (AIstate)
                {
                    case 1:
                        guess = RandomGuess();
                        // validation built into the function
                        valid = true;
                        break;
                    case 2:
                        int trialHit = randomizer.Next(hitArea.Count);
                        guess = hitArea[trialHit];
                        // validation built into the function
                        valid = true;
                        pastGuesses.Add(guess);
                        hitArea.RemoveAt(trialHit);
                        break;
                    case 3:
                        lastGuess = pastGuesses[pastGuesses.Count - 1];
                        guess = new Coords((lastGuess.x + directionModifier.x), (lastGuess.y + directionModifier.y));
                        if (ValidateGuess(guess))
                        {
                            pastGuesses.Add(guess);
                            valid = true;
                        }
                        else
                            Report(0);
                        break;
                    case 4:
                        guess = new Coords((hitSpot.x + directionModifier.x), (hitSpot.y + directionModifier.y));
                        if (ValidateGuess(guess))
                        {
                            pastGuesses.Add(guess);
                            valid = true;
                        }
                        else
                            Report(0);
                        break;
                    case 5:
                        lastGuess = pastGuesses[pastGuesses.Count - 1];
                        guess = new Coords((lastGuess.x + directionModifier.x), (lastGuess.y + directionModifier.y));
                        if (ValidateGuess(guess))
                        {
                            pastGuesses.Add(guess);
                            valid = true;
                        }
                        else
                            Report(0);
                        break;
                    case 6:
                        guess = new Coords((hitSpot.x + directionModifier.x), (hitSpot.y + directionModifier.y));
                        if (ValidateGuess(guess))
                        {
                            pastGuesses.Add(guess);
                            valid = true;
                        }
                        else
                            Report(0);
                        break;
                    case 7:
                        lastGuess = pastGuesses[pastGuesses.Count - 1];
                        guess = new Coords((lastGuess.x + directionModifier.x), (lastGuess.y + directionModifier.y));
                        if (ValidateGuess(guess))
                        {
                            pastGuesses.Add(guess);
                            valid = true;
                        }
                        else
                            Report(0);
                        break;
                    case 8:
                        guess = new Coords((hitSpot.x + directionModifier.x), (hitSpot.y + directionModifier.y));
                        if (ValidateGuess(guess))
                        {
                            pastGuesses.Add(guess);
                            valid = true;
                        }
                        else
                            Report(0);
                        break;
                    case 9:
                        lastGuess = pastGuesses[pastGuesses.Count - 1];
                        guess = new Coords((lastGuess.x + directionModifier.x), (lastGuess.y + directionModifier.y));
                        if (ValidateGuess(guess))
                        {
                            pastGuesses.Add(guess);
                            valid = true;
                        }
                        else
                            Report(0);
                        break;
                    default:
                        valid = true;
                        guess = RandomGuess();
                        break;
                }
            }

            return guess;
        }

        // created a reserve multi hit
        private void SaveMultiHit()
        {
            Queue<Coords> newMultiHit = new Queue<Coords>();

            foreach (Coords coord in pastHits)
            {
                newMultiHit.Enqueue(coord);
            }

            chainedMultiHits.Enqueue(newMultiHit);
            pastHits.Clear();
        }

        // copies the past hits to a new multiHit list to contain a list of ships to go down
        private void ConvertToMultiHit()
        {
            foreach (Coords coord in pastHits)
            {
                multiHit.Enqueue(coord);
            }

            pastHits.Clear();
        }

        // used to subract a sunk ship amount of coordinates from past hits in the case that we tagged two ships
        private void TrimHits(int size)
        {
            //int count = pastHits.Count - 1;
            //int trimTo = count - size;
            //for (int i = count; i > trimTo; i--)
            //{
            //    pastHits.RemoveAt(i);
            //}

            // new code to replace old starts here
            Coords lastHit = pastHits[pastHits.Count - 1];

            // if we are horizontal
            if ((lastHit.y - pastHits[0].y) == 0)
            {
                if ((lastHit.x - pastHits[0].x) < 0)
                {
                    // O(n^m) which is worrisome, possible candidate for refactor, might not matter with the max of m being 5 though
                    for (int i = 0; i < size; i++)
                    {
                        foreach (Coords coord in pastHits)
                        {
                            if ((coord.x - i) == lastHit.x)
                            {
                                pastHits.Remove(coord);
                                break;
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < size; i++)
                    {
                        foreach (Coords coord in pastHits)
                        {
                            if ((coord.x + i) == lastHit.x)
                            {
                                pastHits.Remove(coord);
                                break;
                            }
                        }
                    }
                }
            }
            // otherwise we are vertical orientation
            else
            {
                if ((lastHit.y - pastHits[0].y) < 0)
                {
                    for (int i = 0; i < size; i++)
                    {
                        foreach (Coords coord in pastHits)
                        {
                            if ((coord.y - i) == lastHit.y)
                            {
                                pastHits.Remove(coord);
                                break;
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < size; i++)
                    {
                        foreach (Coords coord in pastHits)
                        {
                            if ((coord.y + i) == lastHit.y)
                            {
                                pastHits.Remove(coord);
                                break;
                            }
                        }
                    }
                }
            }
        }

        // resets the state so we start fresh from AI state 1
        private void resetState()
        {
            pastHits.Clear();
            hitArea.Clear();
            multiHit.Clear();
            chainedMultiHits.Clear();
        }

        // find whether we are going horizontal or vertical
        private void findDirectionMulti(Coords current)
        {
            // if there is nothing left with are dealing with an l shape, just reverse the directionality
            if (multiHit.Count == 0)
            {
                directionModifier = new Coords(directionModifier.y, directionModifier.x);
                if (!ValidateGuess(new Coords((hitSpot.x + directionModifier.x), (hitSpot.x + directionModifier.x))))
                {
                    directionModifier = new Coords((0 - directionModifier.x), (0 - directionModifier.y));
                }
            }
            else
            {
                // this is if horizontal
                if ((current.y - multiHit.Peek().y) == 0)
                {
                    if (randomizer.Next(2) == 0)
                        directionModifier = new Coords(0, 1);
                    else
                        directionModifier = new Coords(0, -1);
                }
                // otherwise it is vertical
                else
                {
                    if (randomizer.Next(2) == 0)
                        directionModifier = new Coords(1, 0);
                    else
                        directionModifier = new Coords(-1, 0);
                }
            }
        }

        //get a modifier that we can add to a coordinate to get a new coordinate in the correct direction
        private void findDirection()
        {
            directionModifier = new Coords((pastHits[pastHits.Count - 1].x - pastHits[pastHits.Count - 2].x), (pastHits[pastHits.Count - 1].y - pastHits[pastHits.Count - 2].y));
        }

        // reverses the direction modifier in case we need to swap
        private void reverseDirection()
        {
            directionModifier = new Coords(0 - directionModifier.x, 0 - directionModifier.y);
        }

        private void createHitArea(Coords guess)
        {
            Coords east = new Coords((guess.x + 1), guess.y);
            Coords west = new Coords((guess.x - 1), guess.y);
            Coords north = new Coords(guess.x, (guess.y + 1));
            Coords south = new Coords(guess.x, (guess.y - 1));

            if (ValidateGuess(east))
                hitArea.Add(east);
            if (ValidateGuess(west))
                hitArea.Add(west);
            if (ValidateGuess(north))
                hitArea.Add(north);
            if (ValidateGuess(south))
                hitArea.Add(south);
        }

        // placeholder for AI turns until more is done
        public Coords RandomGuess()
        {
            Coords guess = new Coords(randomizer.Next(10), randomizer.Next(10));
            bool validGuess = false;

            while (!validGuess)
            {
                if (ValidateGuess(guess))
                {
                    validGuess = true;
                    pastGuesses.Add(guess);
                }
                else
                    guess = new Coords(randomizer.Next(10), randomizer.Next(10));
            }

            return guess;
        }

        // go through each of the past guesses to make sure we aren't guessing a previous guess
        private bool ValidateGuess(Coords guess)
        {
            // if out of bounds it is false
            if (guess.x < 0 || guess.x > 9 || guess.y < 0 || guess.y > 9)
                return false;

            // if it is a past guess it is false
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

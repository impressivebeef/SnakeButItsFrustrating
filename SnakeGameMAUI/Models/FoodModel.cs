﻿using SnakeGameMAUI;
using SnakeGameMAUI.Models;
using static SnakeGameMAUI.Controllers.GameController;

namespace SnakeGameMAUI.Models
{
    public class FoodModel : Model
    {
        private Point _foodPosition;
 
        private Random _random;
        private Direction _oppositeDirection = Direction.Right;

        public FoodModel() 
        {
            this._random = new Random();   
        }
        public void PlaceFood(HashSet<Point> invalidPositions)
        {
            Point nextFoodPosition;
            do
            {
                int x = _random.Next(UIConstants.GridWidth) * UIConstants.CellSize;
                int y = _random.Next(UIConstants.GridHeight) * UIConstants.CellSize;
                System.Diagnostics.Debug.WriteLine($"Next food position: {x}, {y}");
                nextFoodPosition = new Point(x, y);
                
            } while (invalidPositions.Contains(nextFoodPosition));

            this._foodPosition = nextFoodPosition;

            this.OnModelChanged();
        }

        public void MoveFood(HashSet<Point> invalidPositions)
        {
           // Create legal moves list
            List<Direction> directions = [Direction.Right, Direction.Left, Direction.Up, Direction.Down];
            directions.Remove(_oppositeDirection);

            int attemptCount = 0;
            Point nextFoodPosition;
            do
            {
                nextFoodPosition = this._foodPosition;
                // Break out if more every direction has been explored and deemed illegal
                // otherwise perpetual loops may occur
                if (attemptCount == directions.Count) { break; }
                attemptCount++;


                // Get random direction
                int randomDirectionIndex = _random.Next(directions.Count);
                Direction randomDirection = directions[randomDirectionIndex];
                
                // Calculate new position
                switch (randomDirection)
                {
                    case Direction.Left:
                        nextFoodPosition.X -= UIConstants.CellSize;
                        this._oppositeDirection = Direction.Right;
                        break;
                    case Direction.Right:
                        nextFoodPosition.X += UIConstants.CellSize;
                        this._oppositeDirection = Direction.Left;
                        break;
                    case Direction.Down:
                        nextFoodPosition.Y -= UIConstants.CellSize;
                        this._oppositeDirection = Direction.Up;
                        break;
                    case Direction.Up:
                        nextFoodPosition.Y += UIConstants.CellSize;
                        this._oppositeDirection = Direction.Down;
                        break;
                }

                System.Diagnostics.Debug.WriteLine($"Next food position: {nextFoodPosition.X}, {nextFoodPosition.Y}");

            } while (invalidPositions.Contains(nextFoodPosition) || 
                                               nextFoodPosition.X < 0 ||
                                               nextFoodPosition.Y < 0 ||  
                                               nextFoodPosition.X >= UIConstants.CanvasWidth || 
                                               nextFoodPosition.Y >= UIConstants.CanvasHeight);

            this._foodPosition = nextFoodPosition;

            this.OnModelChanged();
        }
        public bool IsColliding(Point position)
        {
            return this._foodPosition.Equals(position);
        }

        public Point GetFoodPosition() { return this._foodPosition; }

    }
}

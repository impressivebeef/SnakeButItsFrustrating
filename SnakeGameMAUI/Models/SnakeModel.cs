using SnakeGameMAUI;
using SnakeGameMAUI.Models;


namespace SnakeGame.Models
{
    public class SnakeModel : Model
    {
        private Queue<Point> _snakeBody = new Queue<Point>();
        private HashSet<Point> _snakePoints = new HashSet<Point>();

        public event Action DiedFromHunger;
        private int _hunger;
        private int _maxHunger;


        public SnakeModel(Point startPosition)
        {
            this._snakeBody.Enqueue(startPosition);
            this._hunger = 100;
            this._maxHunger = 100;
        }

        public bool IsColliding(Point newPosition, HashSet<ArtilleryPosition> artilleryPositions) {

            bool isCollidingWithWall = newPosition.X < 0 ||
                                       newPosition.Y < 0 ||
                                       newPosition.X >= UIConstants.CanvasHeight || 
                                       newPosition.Y >= UIConstants.CanvasWidth;

            bool isCollidingWithBody = this._snakePoints.Contains(newPosition);

            // Check for ArtilleryPositions where Phase > 10, that is when ArtilleryPosition changes from indicator to explosion
            // Hence collision only needs to be checked for those instances
            // Moreover only check collision for Head position (insta death), explosion collision in body is handled in CalculateDamage()
            bool isCollidingWithArtillery = artilleryPositions
                                            .Where(position => position.Phase > 10)
                                            .Any(position => newPosition.Equals(new Point(position.X, position.Y)));


            return isCollidingWithWall || isCollidingWithBody || isCollidingWithArtillery;
        }

        public void CalculateDamage(HashSet<ArtilleryPosition> artilleryPositions)
        {
            // For each snake bodypart (not head) that is hit by an explosion, decrease hunger by appropiate amount;
            foreach (var explosion in artilleryPositions.Where(position => position.Phase > 10))
            {
                if (this._snakePoints.Contains(new Point(explosion.X, explosion.Y)))
                {
                    // Calculate damage modifier based on proximity to the head of the snake
                    // The closer to the head the more damage the snake takes
                    int indexOfHit = this._snakeBody.ToList().IndexOf(new Point(explosion.X, explosion.Y));
                    double damageModifier = (((double)indexOfHit+1)/(this._snakeBody.Count));

                    // Calculate full damage, explosion phase reduces damages in order to simulate a weakening explosion
                    // 26 base damage is completely arbitrary => to be changed for balancing purposes
                    int totalDamage = (int) Math.Round(damageModifier * (26 - explosion.Phase),0);

                    int damage = Math.Max(totalDamage,1);
   
                    System.Diagnostics.Debug.WriteLine($"Total damage: {damage}, Explosion phase damage reduction: {explosion.Phase}, Damage modifier: {damageModifier} ");

                    this._hunger -= damage;

                    this.CheckDeathFromHunger();
                }
            }
        }

        public HashSet<Point> GetPoints() 
        { 
            return new HashSet<Point>(this._snakePoints);
        }

        public Queue<Point> GetBody()
        { 
            return new Queue<Point>(this._snakeBody); 
        }

        public Point GetSnakeHead()
        { 
            return this._snakeBody.Last(); 
        }

        public void UpdateBody(Point newPosition,bool foodEaten)
        {
            this._snakeBody.Enqueue(newPosition);
            this._snakePoints.Add(newPosition);

            if (!foodEaten)
            {
                Point tail = this._snakeBody.Dequeue();
                this._snakePoints.Remove(tail);
                
                this._hunger--;
                this.CheckDeathFromHunger();
            }
            else 
            {
                this.SetHungerOnFoodEaten(); 
            }

            this.OnModelChanged();
        }

        private void CheckDeathFromHunger()
        {
            if (this._hunger <= 0)
            {
                this.DiedFromHunger?.Invoke();
            }
        }

        private void SetHungerOnFoodEaten()
        {
            this._maxHunger += 10;
            this._hunger = Math.Min(this._hunger + 100, this._maxHunger);
        }

        public int GetHunger() {  return this._hunger; }

        public int GetMaxHunger() { return this._maxHunger; }
    }
}

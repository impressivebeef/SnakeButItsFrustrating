using SnakeGameMAUI.Models;
using SnakeGameMAUI.Views;

namespace SnakeGameMAUI.Controllers
{
    public class SnakeController : Controller<SnakeModel,SnakeView>
    {
        public event Action DiedFromHunger; 

        public SnakeController(GraphicsView gameCanvas, Point startPosition)
        {
            this._model = new SnakeModel(startPosition);
            this._view = new SnakeView(gameCanvas, this._model);

            this._model.DiedFromHunger += () => { DiedFromHunger?.Invoke(); }; 
        }

        public bool Update(Point newPoint, bool foodEaten, HashSet<ArtilleryPosition> artilleryPositions)
        {
            if (this._model.IsColliding(newPoint, artilleryPositions)) { return false; }

            this._model.CalculateDamage(artilleryPositions);

            // Move Snake
            this._model.UpdateBody(newPoint, foodEaten);

            return true;
        }

        public HashSet<Point> GetBody() { return this._model.GetPoints(); }
        public Point GetSnakeHead() { return this._model.GetSnakeHead(); }
        public int GetHunger() { return this._model.GetHunger(); }
        public int GetMaxHunger() { return this._model.GetMaxHunger(); }
    }
}

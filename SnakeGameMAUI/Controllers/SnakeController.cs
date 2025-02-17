using Microsoft.Maui.Graphics;
using SnakeGame.Models;
using SnakeGame.Views;

namespace SnakeGameMAUI.Controllers
{
    public class SnakeController : Controller
    {
        public event Action DiedFromHunger; 

        public SnakeController(GraphicsView gameCanvas, Point startPosition)
        {
            this._model = new SnakeModel(startPosition);
            this._view = new SnakeView(gameCanvas, (SnakeModel)this._model);

            ((SnakeModel)this._model).DiedFromHunger += () => { DiedFromHunger?.Invoke(); }; 
        }

        public bool Update(Point newPoint, bool foodEaten, HashSet<ArtilleryPosition> artilleryPositions)
        {
            if (((SnakeModel)this._model).IsColliding(newPoint, artilleryPositions)) { return false; }

            ((SnakeModel)this._model).CalculateDamage(artilleryPositions);

            // Move Snake
            ((SnakeModel)this._model).UpdateBody(newPoint, foodEaten);

            // Rerender Snake
            this._view.Render();

            return true;
        }

        public HashSet<Point> GetBody() { return ((SnakeModel)this._model).GetPoints(); }
        public Point GetSnakeHead() { return ((SnakeModel)this._model).GetSnakeHead(); }
        public void ClearSnake() { ((SnakeView)this._view).ClearSnake(); }
        public int GetHunger() { return ((SnakeModel)this._model).GetHunger(); }
        public int GetMaxHunger() { return ((SnakeModel)this._model).GetMaxHunger(); }
    }
}

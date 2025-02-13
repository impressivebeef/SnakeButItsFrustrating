using Microsoft.Maui.Graphics;
using SnakeGame.Models;
using SnakeGame.Views;

namespace SnakeGameMAUI.Controllers
{
    public class FoodController : Controller
    {

        public FoodController( GraphicsView gameCanvas )
        { 
            this._model = new FoodModel();
            this._view = new FoodView(gameCanvas, (FoodModel)this._model);
        }

        public void GenerateNewFoodPosition(HashSet<Point> invalidPositions)
        {
            ((FoodModel)this._model).PlaceFood(invalidPositions);
            this._view.Render();
        }

        public void MoveFood(HashSet<Point> invalidPositions) 
        { 
            ((FoodModel)this._model).MoveFood(invalidPositions);
            this._view.Render();
        }

        public bool IsColliding(Point position) { return ((FoodModel)this._model).IsColliding(position); }

    }
}

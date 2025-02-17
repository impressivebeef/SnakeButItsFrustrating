using Microsoft.Maui.Graphics;
using SnakeGame.Models;
using SnakeGame.Views;

namespace SnakeGameMAUI.Controllers
{
    public class FoodController : Controller<FoodModel,FoodView>
    {

        public FoodController( GraphicsView gameCanvas )
        { 
            this._model = new FoodModel();
            this._view = new FoodView(gameCanvas, this._model);
        }

        public void GenerateNewFoodPosition(HashSet<Point> invalidPositions)
        {
            this._model.PlaceFood(invalidPositions);
        }

        public void MoveFood(HashSet<Point> invalidPositions) 
        { 
            this._model.MoveFood(invalidPositions);
        }

        public bool IsColliding(Point position) { return this._model.IsColliding(position); }

    }
}

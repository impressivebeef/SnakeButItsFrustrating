using SnakeGame.Models;
using SnakeGameMAUI;

namespace SnakeGame.Views
{
    public class FoodView : SnakeGameMAUI.Views.View<FoodModel>
    {
        public FoodView(GraphicsView gameCanvas, FoodModel foodModel) : base(gameCanvas, foodModel) { }

        public override void Draw(ICanvas canvas, RectF rect) {

            Point foodPosition = this._model.GetFoodPosition();

            canvas.FillColor = Colors.Red;
            canvas.FillRectangle((float) foodPosition.X, (float) foodPosition.Y, UIConstants.CellSize, UIConstants.CellSize);

        }
    }
}

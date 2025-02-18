using SnakeGameMAUI.Models;
using View = SnakeGameMAUI.Views.View<SnakeGameMAUI.Models.FoodModel>;

namespace SnakeGameMAUI.Views
{
    public class FoodView : View
    {
        public FoodView(GraphicsView gameCanvas, FoodModel foodModel) : base(gameCanvas, foodModel) { }

        public override void Draw(ICanvas canvas, RectF rect) {

            Point foodPosition = this._model.GetFoodPosition();

            canvas.FillColor = Colors.Red;
            canvas.FillRectangle((float) foodPosition.X, (float) foodPosition.Y, UIConstants.CellSize, UIConstants.CellSize);

        }
    }
}

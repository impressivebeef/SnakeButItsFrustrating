using SnakeGame.Models;
using SnakeGameMAUI;

namespace SnakeGame.Views
{
    public class FoodView : SnakeGameMAUI.Views.View
    {
        private Point _FoodPosition;

        public FoodView(GraphicsView gameCanvas, FoodModel foodModel) : base(gameCanvas, foodModel) { }
       
        public override void Render() 
        {
            this._FoodPosition = ((FoodModel)this._model).GetFoodPosition();
            this._gameCanvas.Invalidate();
        }

        public override void Draw(ICanvas canvas, RectF rect) { 
            
            canvas.FillColor = Colors.Red;
            canvas.FillRectangle((float) _FoodPosition.X, (float) _FoodPosition.Y, UIConstants.CellSize, UIConstants.CellSize);

        }
    }
}

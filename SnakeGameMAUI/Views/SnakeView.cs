using SnakeGame.Models;
using SnakeGameMAUI;
using SnakeGameMAUI.Views;

namespace SnakeGame.Views
{
    public class SnakeView : SnakeGameMAUI.Views.View<SnakeModel>
    {
        public SnakeView(GraphicsView gameCanvas, SnakeModel snakeModel) : base(gameCanvas, snakeModel) {  }

        public override void Draw(ICanvas canvas, RectF rect) {

            Queue<Point> snakeSegments = this._model.GetBody();

            // Draw snake
            canvas.FillColor = Colors.Green;
            foreach (Point segment in snakeSegments) 
            {
                canvas.FillRectangle((float) segment.X, (float) segment.Y, UIConstants.CellSize, UIConstants.CellSize);
            }
        }

    }
}

using SnakeGameMAUI.Models;
using View = SnakeGameMAUI.Views.View<SnakeGameMAUI.Models.SnakeModel>;

namespace SnakeGameMAUI.Views
{
    public class SnakeView : View
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

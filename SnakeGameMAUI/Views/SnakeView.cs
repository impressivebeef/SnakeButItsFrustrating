using SnakeGame.Models;
using SnakeGameMAUI;
using SnakeGameMAUI.Views;

namespace SnakeGame.Views
{
    public class SnakeView : SnakeGameMAUI.Views.View
    {
        private Queue<Point> _SnakeSegments;
        public SnakeView(GraphicsView gameCanvas, SnakeModel snakeModel) : base(gameCanvas, snakeModel) 
        {
            this._SnakeSegments = new Queue<Point>();
        }

        public override void Render() {
            this._SnakeSegments = ((SnakeModel)this._model).GetBody();
            this._gameCanvas.Invalidate();
        }

        public void ClearSnake() {
            this._SnakeSegments.Clear();
        }

        public override void Draw(ICanvas canvas, RectF rect) {

            // Draw snake
            canvas.FillColor = Colors.Green;
            foreach (Point segment in this._SnakeSegments) 
            {
                canvas.FillRectangle((float) segment.X, (float) segment.Y, UIConstants.CellSize, UIConstants.CellSize);
            }
        }

    }
}

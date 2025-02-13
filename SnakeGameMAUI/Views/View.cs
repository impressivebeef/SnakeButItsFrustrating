using SnakeGameMAUI.Models;

namespace SnakeGameMAUI.Views
{
    public abstract class View : IDrawable
    {

        protected GraphicsView _gameCanvas;
        protected Model _model;

        public View(GraphicsView gameCanvas, Model model)
        {
            this._gameCanvas = gameCanvas;
            this._model = model;
        }

        abstract public void Draw(ICanvas canvas, RectF rect);

        abstract public void Render();

    }
}

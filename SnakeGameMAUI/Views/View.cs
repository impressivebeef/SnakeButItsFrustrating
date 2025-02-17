using SnakeGameMAUI.Models;

namespace SnakeGameMAUI.Views
{
    public abstract class View<TModel> : IDrawable
    {

        protected GraphicsView _gameCanvas;
        protected TModel _model;

        public View(GraphicsView gameCanvas, TModel model)
        {
            this._gameCanvas = gameCanvas;
            this._model = model;
        }

        abstract public void Draw(ICanvas canvas, RectF rect);

        abstract public void Render();

    }
}

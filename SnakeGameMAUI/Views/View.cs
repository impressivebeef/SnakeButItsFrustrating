using SnakeGameMAUI.Models;

namespace SnakeGameMAUI.Views
{
    public abstract class View<TModel> : IDrawable where TModel : Model
    {

        protected GraphicsView _gameCanvas;
        protected TModel _model;

        public View(GraphicsView gameCanvas, TModel model)
        {
            this._gameCanvas = gameCanvas;
            this._model = model;
            
            // Subscribe to the model changed event in model, so that the view rerenders everytime the data in the model changes
            this._model.ModelChanged += () => { gameCanvas.Invalidate(); };
        }

        abstract public void Draw(ICanvas canvas, RectF rect);

    }
}

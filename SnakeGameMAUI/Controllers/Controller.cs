using SnakeGameMAUI.Models;

namespace SnakeGameMAUI.Controllers
{
    public abstract class Controller<TModel,TView> 
        where TModel : Model 
        where TView : SnakeGameMAUI.Views.View<TModel>
    {
        protected TModel _model;
        protected TView _view; 
        
        public TView GetView() { return this._view; }
    }
}

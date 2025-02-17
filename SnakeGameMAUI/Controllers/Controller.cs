using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SnakeGameMAUI.Models;

namespace SnakeGameMAUI.Controllers
{
    public abstract class Controller<TModel,TView>
    {
        protected TModel _model;
        protected TView _view; 
    
        public TView GetView() { return this._view; }
    }
}

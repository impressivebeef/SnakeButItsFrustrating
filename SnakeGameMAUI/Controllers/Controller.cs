using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SnakeGameMAUI.Models;

namespace SnakeGameMAUI.Controllers
{
    public abstract class Controller
    {
        protected Model _model;
        protected SnakeGameMAUI.Views.View _view; 
    
        public SnakeGameMAUI.Views.View GetView() { return this._view; }
    }
}

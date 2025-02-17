namespace SnakeGameMAUI.Models
{
    public abstract class Model
    {

        public event Action ModelChanged;

        protected void OnModelChanged() 
        {
            ModelChanged?.Invoke();
        }
    }
}

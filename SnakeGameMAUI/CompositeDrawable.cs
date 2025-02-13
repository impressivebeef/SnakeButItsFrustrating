
namespace SnakeGameMAUI
{
    public class CompositeDrawable : IDrawable
    {
        private readonly List<IDrawable> _drawables = new();

        public void AddDrawable(IDrawable drawable)
        {
            _drawables.Add(drawable);
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            foreach (var drawable in _drawables)
            {
                drawable.Draw(canvas, dirtyRect);
            }
        }
    }
}

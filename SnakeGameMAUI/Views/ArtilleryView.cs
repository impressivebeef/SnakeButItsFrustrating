using SnakeGameMAUI.Models;

namespace SnakeGameMAUI.Views
{
    public class ArtilleryView : View<ArtilleryModel>
    {
        public ArtilleryView(GraphicsView gameCanvas, ArtilleryModel artilleryModel) : base(gameCanvas, artilleryModel) { }

        public override void Draw(ICanvas canvas, RectF rect)
        {
            HashSet<ArtilleryPosition> positions = this._model.GetPositions();

            foreach (ArtilleryPosition position in positions)
            {
                if (position.Phase <= 10 && position.Phase % 2 == 0) { canvas.FillColor = Colors.Blue; }
                else if (position.Phase <= 10) { canvas.FillColor = Colors.DarkBlue; }
                else
                {
                    canvas.FillColor = position.Phase switch
                    {
                        11 => Colors.Red,
                        12 => Colors.Orange,
                        13 => Colors.Yellow,
                        14 => Colors.Brown,
                        15 => Colors.Black,
                        _  => Colors.Transparent
                    };
                }
                canvas.FillRectangle((float) position.X, (float) position.Y, UIConstants.CellSize, UIConstants.CellSize);
            }
           
        }
    }
}

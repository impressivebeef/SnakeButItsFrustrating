using SnakeGameMAUI.Models;
using Color = Microsoft.Maui.Graphics.Color;
using View = SnakeGameMAUI.Views.View<SnakeGameMAUI.Models.ArtilleryModel>;

namespace SnakeGameMAUI.Views
{
    public class ArtilleryView : View
    {
        public ArtilleryView(GraphicsView gameCanvas, ArtilleryModel artilleryModel) : base(gameCanvas, artilleryModel) { }

        public override void Draw(ICanvas canvas, RectF rect)
        {
            HashSet<ArtilleryPosition> positions = this._model.GetPositions();

            foreach (ArtilleryPosition position in positions)
            {
                canvas.FillColor = GetColor(position.Phase);
                
                canvas.FillRectangle((float) position.X, (float) position.Y, UIConstants.CellSize, UIConstants.CellSize);
            }
        }

        private Color GetColor(int phase)
        {

            Color color;

            if (phase <= 10 && phase % 2 == 0) { color = Colors.Blue; }
            else if (phase <= 10) { color = Colors.DarkBlue; }
            else
            {
                color = phase switch
                {
                    11 => Colors.Red,
                    12 => Colors.Orange,
                    13 => Colors.Yellow,
                    14 => Colors.Brown,
                    15 => Colors.Black,
                    _ => Colors.Transparent
                };
            }

            return color;
        }
    }
}

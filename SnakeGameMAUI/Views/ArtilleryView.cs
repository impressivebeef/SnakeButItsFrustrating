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
                if (position.Phase <= 10 && position.Phase % 2 == 0)
                {

                    canvas.FillColor = Colors.Blue;

                }
                else if (position.Phase <= 10)
                {
                    canvas.FillColor = Colors.DarkBlue;
                }
                else
                {
                    switch (position.Phase)
                    {
                        case 11:
                            canvas.FillColor = Colors.Red;
                            break;
                        case 12:
                            canvas.FillColor = Colors.Orange;
                            break;
                        case 13:
                            canvas.FillColor = Colors.Yellow;
                            break;
                        case 14:
                            canvas.FillColor = Colors.Brown;
                            break;
                        case 15:
                            canvas.FillColor = Colors.Black;
                            break;
                    }
                }
                canvas.FillRectangle((float) position.X, (float) position.Y, UIConstants.CellSize, UIConstants.CellSize);
            }
           
        }
    }
}

using SnakeGameMAUI.Models;
using SnakeGameMAUI.Views;

namespace SnakeGameMAUI.Controllers
{
    public class ArtilleryController : Controller
    {

        public ArtilleryController(GraphicsView gameCanvas)
        {
            this._model = new ArtilleryModel();
            this._view = new ArtilleryView(gameCanvas, (ArtilleryModel)this._model);
        }

        public void UpdateArtillery()
        {
            ((ArtilleryModel)this._model).UpdateArtillery();
            this._view.Render();
        }

        public void GenerateArtilleryPoint()
        {
            ((ArtilleryModel)this._model).PlaceArtilleryPoint();
            this._view.Render();
        }

        public HashSet<ArtilleryPosition> GetArtilleryPositions() { return ((ArtilleryModel)this._model).GetPositions(); }

    }
}

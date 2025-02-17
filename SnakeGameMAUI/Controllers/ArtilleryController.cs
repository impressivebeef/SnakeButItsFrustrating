using SnakeGameMAUI.Models;
using SnakeGameMAUI.Views;

namespace SnakeGameMAUI.Controllers
{
    public class ArtilleryController : Controller<ArtilleryModel,ArtilleryView>
    {

        public ArtilleryController(GraphicsView gameCanvas)
        {
            this._model = new ArtilleryModel();
            this._view = new ArtilleryView(gameCanvas, (ArtilleryModel)this._model);
        }

        public void UpdateArtillery()
        {
            this._model.UpdateArtillery();
            this._view.Render();
        }

        public void GenerateArtilleryPoint()
        {
            this._model.PlaceArtilleryPoint();
            this._view.Render();
        }

        public HashSet<ArtilleryPosition> GetArtilleryPositions() { return this._model.GetPositions(); }

    }
}

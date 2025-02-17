namespace SnakeGameMAUI.Models
{
    public class ArtilleryModel : Model
    {
        private Random _random;

        private HashSet<ArtilleryPosition> _positions;

        public ArtilleryModel() 
        {

            this._positions = new HashSet<ArtilleryPosition>();
            this._random = new Random();
        }

        public void PlaceArtilleryPoint()
        {

            int x = _random.Next(UIConstants.GridWidth) * UIConstants.CellSize;
            int y = _random.Next(UIConstants.GridHeight) * UIConstants.CellSize;
            System.Diagnostics.Debug.WriteLine($"Next Artillery position: {x}, {y}");

            ArtilleryPosition ArtilleryPosition = new ArtilleryPosition(x, y, 0, 0);

            this._positions.Add(ArtilleryPosition);

            this.OnModelChanged();
        }

        public void UpdateArtillery() { 
            // Create a list to track the positions to be added
            List<ArtilleryPosition> newPositions = new List<ArtilleryPosition>();

            // Create a Point list for all positions
            // This is needed, because while the position in ArtilleryPosition objects stay the same, both the Phase and Generation attributes do not
            // Hence when checking if position is taken its better to compare Point objects,
            // Otherwise its possible to end up with duplicate positions => Game gets laggy while rendering explosions
            HashSet<Point> artilleryPositions = this._positions.Select(position => new Point(position.X, position.Y)).ToHashSet();

            // Loop through each artillery position
            foreach (ArtilleryPosition artilleryPosition in this._positions)
            {
                artilleryPosition.IncrementPhase();

                // If ArtilleryPosition phase is > 10, then create an explosion by adding new positions adjacent to the selected point
                // Explosion grows exponentially but is cut off by the Generation attribute which limits the explosion size to 3
                if (artilleryPosition.Phase > 10)
                {
                    Point[] adjacentPositions = this.GetAdjacentPositions(artilleryPosition);

                    foreach(Point position in adjacentPositions)
                    {
                        // IMPORTANT: do not remove the Generation check => unending explosion otherwise
                        if (!artilleryPositions.Contains(position) && artilleryPosition.Generation < 3)
                        {
                            ArtilleryPosition newArtilleryPosition = new ArtilleryPosition((int)position.X, (int)position.Y, 11, artilleryPosition.Generation + 1);
                            System.Diagnostics.Debug.WriteLine($"Added artillery position {newArtilleryPosition.X},{newArtilleryPosition.Y}");
                            
                            newPositions.Add(newArtilleryPosition);
                            artilleryPositions.Add(position);
                        }
                    }
                }
            }

            // Add new positions to hashset
            foreach(var position in newPositions)
            {
                this._positions.Add(position);
            }

            // remove all postions that exceed Phase > 15
            this._positions.RemoveWhere(position => position.Phase > 15);

            this.OnModelChanged();
        }

        private Point[] GetAdjacentPositions(ArtilleryPosition artilleryPosition)
        {
            return [new Point(artilleryPosition.X + UIConstants.CellSize, artilleryPosition.Y),
                    new Point(artilleryPosition.X - UIConstants.CellSize, artilleryPosition.Y),
                    new Point(artilleryPosition.X, artilleryPosition.Y + UIConstants.CellSize),
                    new Point(artilleryPosition.X, artilleryPosition.Y - UIConstants.CellSize)];
        }

        public HashSet<ArtilleryPosition> GetPositions() {  return this._positions; }
    }
}

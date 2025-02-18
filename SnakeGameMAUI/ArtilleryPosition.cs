namespace SnakeGameMAUI
{
    public class ArtilleryPosition
    {

        public int X { get; set; } 
        public int Y { get; set; }
        public int Phase { get; set; }
        public int Generation { get; set; }
        public ArtilleryPosition(int X, int Y, int Phase, int Generation) 
        { 
            this.X = X;
            this.Y = Y;
            this.Phase = Phase;
            this.Generation = Generation;
        }

        public void IncrementPhase() {  Phase++; }
    }
}

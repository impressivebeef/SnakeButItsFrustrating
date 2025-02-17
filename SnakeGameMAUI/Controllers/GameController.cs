using System.Diagnostics;
using Microsoft.Maui.Graphics;
using SnakeGame.Views;
using SnakeGameMAUI.Views;

namespace SnakeGameMAUI.Controllers
{
    public class GameController
    {
        private const int _GameSpeed = 100;
        private bool _IsRunning;

        private CancellationTokenSource _CancellationTokenSource;
        private int _Score;
        private int _HighScore;
        private int _tickCounter;

        private FoodController _FoodController;
        private SnakeController _SnakeController;
        private ArtilleryController _ArtilleryController;

        public enum Direction { Left, Right, Up, Down };
        private Direction _CurrentDirection = Direction.Right;
        private Direction? _BufferedDirection = null;

        GraphicsView GameCanvas;
        Label ScoreLabel;
        Label HungerLabel;

        public GameController(GraphicsView gameCanvas, Label ScoreLabel, Label hungerLabel) 
        { 
            this.GameCanvas = gameCanvas;
            this.ScoreLabel = ScoreLabel;
            this.HungerLabel = hungerLabel;

            InitializeGame();
        }
        private void InitializeGame()
        {

            // Create new Cancellation token
            _CancellationTokenSource = new CancellationTokenSource();

            // Initiate Snake Controller
            Point startPosition = new Point(200, 200);
            this._SnakeController = new SnakeController(GameCanvas, startPosition);
            this._SnakeController.DiedFromHunger += CreateGameOverScreen;

            // Initiate Food Controller
            this._FoodController = new FoodController(GameCanvas);

            // Generate initial food position
            this._FoodController.GenerateNewFoodPosition(_SnakeController.GetBody());

            // Initiate Artillery Controller
            this._ArtilleryController = new ArtilleryController(GameCanvas);

            // Get highscore
            this._HighScore = GetHighScore();

            //Initiate hungerbar
            this.HungerLabel.Text = "Hunger: 100/100";

            // Set score to 0
            this._Score = 0;
            this.ScoreLabel.Text = $"Score: {this._Score.ToString()} Highscore: {this._HighScore.ToString()}";

            // Set tickcounter 0
            this._tickCounter = 0;

            // PROBLEM: Graphicsview doesnt support having multiple Drawable objects attached to GameCanvas 
            // SOLUTION => Combine all views in 1 composite object and add the resulting object to assign to GameCanvas
            CompositeDrawable compositeDrawable = new CompositeDrawable();
            compositeDrawable.AddDrawable(this._SnakeController.GetView());
            compositeDrawable.AddDrawable(this._FoodController.GetView());
            compositeDrawable.AddDrawable(this._ArtilleryController.GetView());
            this.GameCanvas.Drawable = compositeDrawable;

            // Run game loop
            _ = RunGameLoop(this._CancellationTokenSource.Token);
        }

        private void RestartGame()
        {

            // Cancel old gameloop
            this._CancellationTokenSource.Cancel();

            // Reinitialize Game
            InitializeGame();
        }

        private async Task RunGameLoop(CancellationToken cancellationToken)
        {
            Stopwatch timer = new Stopwatch();
            this._IsRunning = true;

            while (this._IsRunning && !cancellationToken.IsCancellationRequested)
            {
                timer.Restart();

                // Run the game
                ProcessGameTick();

                _tickCounter++;

                // Calculate the delay from running the game and wait the remaining time
                TimeSpan delay = TimeSpan.FromMilliseconds(_GameSpeed) - timer.Elapsed;

                System.Diagnostics.Debug.WriteLine($"Delay: {delay.ToString()}");

                // Await the delay time until next tick
                if (delay > TimeSpan.Zero) { await Task.Delay(delay); }

            }
        }

        private void ProcessGameTick()
        {
            // Check if direction changed
            if (this._BufferedDirection != null)
            {
                this._CurrentDirection = _BufferedDirection.Value;
                this._BufferedDirection = null;
            }

            // Calculate next position
            Point nextPosition = this.CalculateNextPosition(this._SnakeController.GetSnakeHead());

            // Check if snake head is colliding with food
            bool foodEaten = this._FoodController.IsColliding(nextPosition);

            // Update the snakebody and check if there are collisions
            // If colliding => generate game-over screen
            if (!this._SnakeController.Update(nextPosition, foodEaten, this._ArtilleryController.GetArtilleryPositions()))
            {
                this.CreateGameOverScreen();
            }
            // If true => extend SnakeBody and generate new food position
            if (foodEaten)
            {
                System.Diagnostics.Debug.WriteLine("Eaten food");
                this._FoodController.GenerateNewFoodPosition(this._SnakeController.GetBody());
                this._Score++;

                int highscore = this._Score < this._HighScore ? this._HighScore : this._Score;

                this.ScoreLabel.Text = $"Score: {this._Score} Highscore: {highscore}";
            }

            if(this._tickCounter % 2 == 0)
            {
                this._FoodController.MoveFood(this._SnakeController.GetBody());
            }
            else if(this._tickCounter >= 6)
            {
                this._ArtilleryController.GenerateArtilleryPoint();
                this._tickCounter = 0;
            }

            //Update hungerbar
            this.UpdateHungerLabel();

            this._ArtilleryController.UpdateArtillery();

        }

        private void CreateGameOverScreen()
        {
            this._IsRunning = false;

            MainThread.BeginInvokeOnMainThread(async () =>
            {
                if (_Score > _HighScore) { SaveHighScore(_Score); }

                bool restart = await App.Current.MainPage.DisplayAlert("Game Over!", "Restart?", "Yes", "No");
                if (restart) { RestartGame(); }
                else { Environment.Exit(1); }
            });
        }

        private Point CalculateNextPosition(Point currentPosition)
        {
            Point nextPosition = currentPosition;

            switch (this._CurrentDirection)
            {
                case Direction.Up:
                    nextPosition.Y -= UIConstants.CellSize;
                    break;

                case Direction.Down:
                    nextPosition.Y += UIConstants.CellSize;
                    break;

                case Direction.Left:
                    nextPosition.X -= UIConstants.CellSize;
                    break;

                case Direction.Right:
                    nextPosition.X += UIConstants.CellSize;
                    break;
            }

            return nextPosition;
        }
        
        private void UpdateHungerLabel()
        {
            int hunger = this._SnakeController.GetHunger();
            int maxHunger = this._SnakeController.GetMaxHunger();

            this.HungerLabel.Text = $"Hunger: {hunger}/{maxHunger}";
        }

        public void HandleInput(char input) {

            // Convert input char to lower
            input = Char.ToLower(input);

            // Use a buffered input
            // Otherwise illegal turns might be made if 2 inputs are send before the game tick is over
            switch (input)
            {

                case 'w':
                    if (this._CurrentDirection != Direction.Down) { this._BufferedDirection = Direction.Up; }
                    break;

                case 'a':
                    if (this._CurrentDirection != Direction.Right) { this._BufferedDirection = Direction.Left; }
                    break;

                case 's':
                    if (this._CurrentDirection != Direction.Up) { this._BufferedDirection = Direction.Down; }
                    break;

                case 'd':
                    if (this._CurrentDirection != Direction.Left) { this._BufferedDirection = Direction.Right; }
                    break;

                default:
                    throw new ArgumentException("Invalid key: " + input);
            }
        }

        private void SaveHighScore(int highScore) 
        { 
            string path = Environment.CurrentDirectory;

            using (StreamWriter sw = new StreamWriter(Path.Combine(path, "HighScore.txt")))
            {
                try
                {
                    sw.WriteLine(highScore);
                    System.Diagnostics.Debug.WriteLine("Saved new highscore: " + highScore.ToString());
                }
                catch(Exception e) { System.Diagnostics.Debug.WriteLine(e.Message); }
                
                
            }
        }

        private int GetHighScore() 
        { 
            int highScore;

            try 
            {
                string path = Environment.CurrentDirectory;

                using (StreamReader sr = new StreamReader(Path.Combine(path, "HighScore.txt"))) 
                { 
                    highScore = int.Parse(sr.ReadLine());
                }
            }
            
            catch (Exception e) { highScore = 0; };

        return highScore;
        }
    }
}

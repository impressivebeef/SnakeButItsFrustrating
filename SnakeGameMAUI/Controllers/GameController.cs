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
            HungerLabel.Text = "Hunger: 100/100";

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
            GameCanvas.Drawable = compositeDrawable;

            // Run game loop
            _ = RunGameLoop(_CancellationTokenSource.Token);
        }

        private void RestartGame()
        {

            // Cancel old gameloop
            _CancellationTokenSource.Cancel();

            // Reinitialize Game
            InitializeGame();
        }

        private async Task RunGameLoop(CancellationToken cancellationToken)
        {
            DateTime nextTick;
            _IsRunning = true;

            while (_IsRunning && !cancellationToken.IsCancellationRequested)
            {
                nextTick = DateTime.Now;
                nextTick = nextTick.AddMilliseconds(_GameSpeed);

                // Run the game
                ProcessGameTick();

                _tickCounter++;

                // Calculate the delay from running the game and wait the remaining time
                var delay = (nextTick - DateTime.Now);
                System.Diagnostics.Debug.WriteLine($"Delay: {delay.ToString()}");

                // Await the delay time until next tick
                if (delay > TimeSpan.Zero) { await Task.Delay(delay); }

            }
        }

        private void ProcessGameTick()
        {
            // Check if direction changed
            if (_BufferedDirection != null)
            {
                _CurrentDirection = _BufferedDirection.Value;
                _BufferedDirection = null;
            }

            // Calculate next position
            Point nextPosition = CalculateNextPosition(_SnakeController.GetSnakeHead());

            // Check if snake head is colliding with food
            bool foodEaten = _FoodController.IsColliding(nextPosition);

            // Update the snakebody and check if there are collisions
            // If colliding => generate game-over screen
            if (!_SnakeController.Update(nextPosition, foodEaten, _ArtilleryController.GetArtilleryPositions()))
            {
                this.CreateGameOverScreen();
            }
            // If true => extend SnakeBody and generate new food position
            if (foodEaten)
            {
                System.Diagnostics.Debug.WriteLine("Eaten food");
                _FoodController.GenerateNewFoodPosition(_SnakeController.GetBody());
                this._Score++;

                int highscore = this._Score < this._HighScore ? this._HighScore : this._Score;

                this.ScoreLabel.Text = $"Score: {this._Score} Highscore: {highscore}";
            }

            if(this._tickCounter % 2 == 0)
            {
                _FoodController.MoveFood(_SnakeController.GetBody());
            }
            else if(this._tickCounter >= 6)
            {
                _ArtilleryController.GenerateArtilleryPoint();
                this._tickCounter = 0;
            }

            //Update hungerbar
            this.UpdateHungerLabel();

            _ArtilleryController.UpdateArtillery();

        }

        private void CreateGameOverScreen()
        {
            _IsRunning = false;

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

            switch (_CurrentDirection)
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

            HungerLabel.Text = $"Hunger: {hunger}/{maxHunger}";
        }

        public void HandleInput(char input) {

            // Convert input char to lower
            input = Char.ToLower(input);

            // Use a buffered input
            // Otherwise illegal turns might be made if 2 inputs are send before the game tick is over
            switch (input)
            {

                case 'w':
                    if (_CurrentDirection != Direction.Down) { _BufferedDirection = Direction.Up; }
                    break;

                case 'a':
                    if (_CurrentDirection != Direction.Right) { _BufferedDirection = Direction.Left; }
                    break;

                case 's':
                    if (_CurrentDirection != Direction.Up) { _BufferedDirection = Direction.Down; }
                    break;

                case 'd':
                    if (_CurrentDirection != Direction.Left) { _BufferedDirection = Direction.Right; }
                    break;

                default:
                    throw new Exception("Invalid key: " + input);
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

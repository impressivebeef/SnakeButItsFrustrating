using Plugin.Maui.KeyListener;
using SharpHook.Native;
using SharpHook.Reactive;
using SharpHook;

namespace SnakeGameMAUI
{
    public partial class MainPage : ContentPage
    {

        private SnakeGameMAUI.Controllers.GameController controller;
        private SimpleReactiveGlobalHook KeyboardHook = new(GlobalHookType.Keyboard, runAsyncOnBackgroundThread: true);
       
        public MainPage()
        {
            InitializeComponent();

            // Initialize GameController
            // Also immediately starts Gameloop
            this.controller = new SnakeGameMAUI.Controllers.GameController(GameCanvas, ScoreLabel, HungerLabel);
            
            // Create a keyboard listener to process key inputs
            HandleKeyboardInput();
        }
        private void HandleKeyboardInput() 
        {
            KeyboardHook.KeyPressed.Subscribe(e =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    try
                    {
                        // Dirty fix to convert SharpHook KeyCode to WASD commands
                        this.controller.HandleInput(e.Data.KeyCode.ToString()[2]);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"{ex.Message}");
                    }
                });
            });

            KeyboardHook.RunAsync();
        }
    }

}

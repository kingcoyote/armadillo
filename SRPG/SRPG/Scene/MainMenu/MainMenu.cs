using Nuclex.UserInterface;
using Nuclex.UserInterface.Visuals.Flat;
using Torch;

namespace SRPG.Scene.MainMenu
{
    class MainMenu : Torch.Scene
    {
        private readonly MenuOptionsDialog _menuOptionsDialog;
        private readonly OptionsControl _optionsControl;

        public MainMenu(Game game) : base(game)
        {
            Components.Add(new BackgroundLayer(this, null, "MainMenu/bg") { DrawOrder = -10000 });
            _menuOptionsDialog = new MenuOptionsDialog();
            _menuOptionsDialog.Bounds = new UniRectangle(10, 10, 180, 275);

            _optionsControl = new OptionsControl();
            _optionsControl.Bounds = new UniRectangle(
                new UniScalar(0, 200), new UniScalar(10),
                new UniScalar(310), new UniScalar(500) 
            );
            _optionsControl.OptionsCanceled += () => HideGui(_optionsControl);
            _optionsControl.OptionsSaved += (fullscreen, width, height) =>
                {
                    Game.ChangeResolution(fullscreen, width, height);
                    HideGui(_optionsControl);
                };
            HideGui(_optionsControl);

            _menuOptionsDialog.OnExitPressed += () => Game.Exit();
            _menuOptionsDialog.OnNewGamePressed += () => ((SRPGGame)Game).StartGame();

            _menuOptionsDialog.OnOptionsPressed += () =>
                {
                    ShowGui(_optionsControl);
                    _optionsControl.UpdateSettings(game);
                };

            _menuOptionsDialog.OnLoadGamePressed += () => ((SRPGGame)Game).ShowLoadScreen();
            _menuOptionsDialog.OnContinuePressed += () => ((SRPGGame)Game).ContinueLastGame();

            Gui.Screen.Desktop.Children.Add(_menuOptionsDialog);
            Gui.Visualizer = FlatGuiVisualizer.FromFile(Game.Services, "Content/Gui/main_gui.xml");
        }

        protected override void OnEntered()
        {
            base.OnEntered();
            Game.IsMouseVisible = true;
        }

        protected override void OnResume()
        {
            base.OnResume();
            Game.IsMouseVisible = true;
        }

        protected override void OnLeaving()
        {
            base.OnLeaving();
            Game.IsMouseVisible = false;
        }


    }
}

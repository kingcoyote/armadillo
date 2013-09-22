using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nuclex.UserInterface.Visuals.Flat;
using Torch;

namespace SRPG.Scene.MainMenu
{
    class MainMenu : Torch.Scene
    {
        private MenuOptionsDialog _menuOptionsDialog;
        public MainMenu(Game game) : base(game) { }
    
        public override void Initialize()
        {
            base.Initialize();

            Components.Add(new BackgroundLayer(this, null, "MainMenu/bg") { DrawOrder = -10000 });
            _menuOptionsDialog = new MenuOptionsDialog();

            _menuOptionsDialog.OnExitPressed += (s, a) => Game.Exit();
            _menuOptionsDialog.OnNewGamePressed += (s, a) => ((SRPGGame) Game).StartGame();

            Gui.Screen.Desktop.Children.Add(_menuOptionsDialog);
            Gui.Visualizer = FlatGuiVisualizer.FromFile(Game.Services, "Content/Gui/main_menu.xml");
        }

        public override void Start()
        {
            Game.IsMouseVisible = true;
        }

        public override void Stop()
        {
            Game.IsMouseVisible = false;
        }
    }
}

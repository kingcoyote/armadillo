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
        public MainMenu(Game game) : base(game)
        {
            Components.Add(new BackgroundLayer(this, null, "MainMenu/bg") { DrawOrder = -10000 });
            _menuOptionsDialog = new MenuOptionsDialog();

            _menuOptionsDialog.OnExitPressed += () => Game.Exit();
            _menuOptionsDialog.OnNewGamePressed += () => ((SRPGGame)Game).StartGame();

            Gui.Screen.Desktop.Children.Add(_menuOptionsDialog);
            Gui.Visualizer = FlatGuiVisualizer.FromFile(Game.Services, "Content/Gui/main_menu.xml");
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

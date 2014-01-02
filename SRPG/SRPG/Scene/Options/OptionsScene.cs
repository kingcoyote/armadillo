using Microsoft.Xna.Framework.Input;
using Nuclex.UserInterface.Visuals.Flat;
using Torch;

namespace SRPG.Scene.Options
{
    class OptionsScene : Torch.Scene
    {
        public OptionsScene(Game game) : base(game)
        {
            var keyboard = new KeyboardInputLayer(this, null);
            keyboard.AddKeyDownBinding(Keys.Escape, () => Game.PopScene());

            Components.Add(keyboard);

            var menuDialog = new MenuDialog();
            menuDialog.OnReturnPressed += () => Game.PopScene();
            menuDialog.OnExitPressed += () => Game.Exit();
            menuDialog.OnMainMenuPressed += () => ((SRPGGame)Game).ReturnToMainMenu();

            Gui.Screen.Desktop.Children.Add(menuDialog);
            Gui.Visualizer = FlatGuiVisualizer.FromFile(Game.Services, "Content/Gui/main_gui.xml");
        }

        protected override void OnEntered()
        {
            Game.IsMouseVisible = true;

            base.OnEntered();
        }

        protected override void OnResume()
        {
            Game.IsMouseVisible = true;

            base.OnResume();
        }
    }
}

using Nuclex.UserInterface;
using Nuclex.UserInterface.Controls.Desktop;
using Nuclex.UserInterface.Visuals.Flat;
using Torch;

namespace SRPG.Scene.SaveGame
{
    partial class SaveGameScene : Torch.Scene
    {
        public SaveGameScene(Game game, string zone, string door) : base(game)
        {
            // GUI elements:
            // cancel button
            var cancelButton = new ButtonControl();
            cancelButton.Text = "Cancel";
            cancelButton.Pressed += (s, a) => Game.PopScene();
            cancelButton.Bounds = new UniRectangle(
                new UniScalar(1.0f, -150), new UniScalar(0),
                new UniScalar(150), new UniScalar(45) 
            );
            Gui.Screen.Desktop.Children.Add(cancelButton);
            
            // new save
            var newSaveButton = new ButtonControl();
            newSaveButton.Text = "New Save";
            newSaveButton.Pressed += (s, a) =>
                {
                    ((SRPGGame) Game).NewSaveGame(zone, door);
                    Game.PopScene();
                };
            newSaveButton.Bounds = new UniRectangle(
                new UniScalar(1.0f, -150), new UniScalar(55),
                new UniScalar(150), new UniScalar(45) 
            );
            Gui.Screen.Desktop.Children.Add(newSaveButton);

            // list of saves
            // list of games
            var saveGameList = Data.SaveGame.FetchAll((SRPGGame)game);

            for (var i = 0; i < saveGameList.Count; i++)
            {
                var saveGame = saveGameList[i];
                var dlg = new SavedGameDialog(saveGame);
                dlg.Bounds = new UniRectangle(
                    new UniScalar(0), new UniScalar(110 * i),
                    new UniScalar(1.0f, -160), new UniScalar(100)
                );
                var fileNumber = i;
                dlg.OnSelect += () =>
                    {
                        ((SRPGGame) Game).SaveGame(fileNumber, zone, door);
                        Game.PopScene();
                    };
                Gui.Screen.Desktop.Children.Add(dlg);
            }

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

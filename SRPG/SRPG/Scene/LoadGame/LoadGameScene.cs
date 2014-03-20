using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Controls.Desktop;
using Nuclex.UserInterface.Visuals.Flat;
using Torch;

namespace SRPG.Scene.LoadGame
{
    class LoadGameScene : Torch.Scene
    {
        public LoadGameScene(Game game) : base(game)
        {
            // list of games
            var saveGameList = Data.SaveGame.FetchAll((SRPGGame)game);

            for(var i = 0; i < saveGameList.Count; i++)
            {
                var saveGame = saveGameList[i];
                var dlg = new SavedGameDialog(saveGame);
                dlg.Bounds = new UniRectangle(
                    new UniScalar(0), new UniScalar(100 * i),
                    new UniScalar(1.0f, -160), new UniScalar(100) 
                );
                var fileNumber = i;
                dlg.OnSelect += () => ((SRPGGame) Game).LoadGame(fileNumber);
                Gui.Screen.Desktop.Children.Add(dlg);
            }

            // cancel button
            var button = new ButtonControl();
            button.Bounds = new UniRectangle(
                new UniScalar(1.0f, -150), new UniScalar(0),
                new UniScalar(150), new UniScalar(45)
            );
            button.Text = "Cancel";
            button.Pressed += (s, a) => Game.PopScene();
            Gui.Screen.Desktop.Children.Add(button);

            Gui.Visualizer = FlatGuiVisualizer.FromFile(Game.Services, "Content/Gui/main_gui.xml");
        }
    }
}

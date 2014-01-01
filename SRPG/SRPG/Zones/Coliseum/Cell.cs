using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SRPG.Data;
using SRPG.Scene.Overworld;
using Torch;
using Game = Microsoft.Xna.Framework.Game;

namespace SRPG.Zones.Coliseum
{
    class Cell : Zone
    {
        private bool _guardMoved;

        public Cell(Game game, Torch.Object parent) : base(game)
        {
            Name = "Coliseum Slave Cells";
            Sandbag = Grid.FromBitmap(Game.Services, "Zones/Coliseum/Cell/sandbag");
            ImageLayers.Add(new ImageObject(Game, parent, "Zones/Coliseum/Cell/cell"));
            ImageLayers.Add(new ImageObject(Game, parent, "Zones/Coliseum/Cell/bars") { X = 50, Y = 300, DrawOrder = 350 });
            ImageLayers.Add(new ImageObject(Game, parent, "Zones/Coliseum/Cell/bars") { X = 50, Y = 500, DrawOrder = 550 });
            Doors.Add(new Door { Location = new Rectangle(100, 650, 7*6, 8*6), Name = "bed", Orientation = Direction.Right });
            Doors.Add(new Door { Location = new Rectangle(905, 200, 45, 25), Name = "halls", Orientation = Direction.Down });

            Objects.Add(new InteractiveObject { Interact = SimpleDoor("coliseum/halls-south", "cell"), Location = new Rectangle(905, 190, 45, 15) });
#if DEBUG
            Objects.Add(new InteractiveObject
                {Interact = TestBattle("coliseum/halls"), Location = new Rectangle(230, 700, 10, 50)});
#endif

            Characters.Add("guard", Avatar.GenerateAvatar(game, null, "enemy"));
            Characters["guard"].Location.X = 150;
            Characters["guard"].Location.Y = 440;

            Characters["guard"].Interact = TalkToGuard;
        }

        public void TalkToGuard(object sender, EventArgs args)
        {
            if (_guardMoved)
            {
                var dialog = Dialog.Fetch("coliseum/cell", "guardMoved");
                ((OverworldScene)sender).StartDialog(dialog);
            }
            else
            {
                var dialog = Dialog.Fetch("coliseum/cell", "guard");
                dialog.OnExit = (o, eventArgs) => ((OverworldScene) sender).MoveCharacter("guard", new[] {new Vector2(-55, 0), new Vector2(2, 0)});
                ((OverworldScene) sender).StartDialog(dialog);
                _guardMoved = true;
            }
        }
    }
}

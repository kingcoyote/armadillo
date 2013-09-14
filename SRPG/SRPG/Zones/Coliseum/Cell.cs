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

        public Cell(Game game) : base(game)
        {
            Name = "Coliseum Slave Cells";
            Sandbag = Grid.FromBitmap(Game.Services, "Zones/Coliseum/Cell/sandbag");
            ImageLayers.Add(new ImageObject(Game, "Zones/Coliseum/Cell/cell"));
            Doors.Add(new Door { Location = new Rectangle(36*6, 126*6, 7*6, 8*6), Name = "bed", Orientation = Direction.Right });
            Doors.Add(new Door { Location = new Rectangle(1851, 162, 48, 9), Name = "halls", Orientation = Direction.Down });

            Objects.Add(new InteractiveObject { Interact = SimpleDoor("coliseum/halls", "cell"), Location = new Rectangle(1850, 143, 49, 12) });
            Objects.Add(new InteractiveObject
                {Interact = TestBattle("coliseum/halls"), Location = new Rectangle(818, 159, 32, 32)});

            Characters.Add("guard", Avatar.GenerateAvatar(game, "enemy"));
            Characters["guard"].Location.X = 315;
            Characters["guard"].Location.Y = 515;

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
                dialog.OnExit = (o, eventArgs) => ((OverworldScene) sender).MoveCharacter("guard", new[] {new Vector2(100, 0), new Vector2(-2, 0)});
                ((OverworldScene) sender).StartDialog(dialog);
                _guardMoved = true;
            }
        }
    }
}

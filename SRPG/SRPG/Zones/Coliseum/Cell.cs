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
            var propsSprite = new SpriteObject(Game, parent, "Zones/Coliseum/Cell/props");
            propsSprite.AddAnimation("barrel1", new SpriteAnimation { FrameRate = 1, FrameCount = 1, Size = new Rectangle(0, 0, 41, 51), StartRow = 63, StartCol = 99 });
            propsSprite.AddAnimation("barrel2", new SpriteAnimation { FrameRate = 1, FrameCount = 1, Size = new Rectangle(0, 0, 41, 50), StartRow = 63, StartCol = 140 });
            propsSprite.AddAnimation("bed1", new SpriteAnimation { FrameRate = 1, FrameCount = 1, Size = new Rectangle(0, 0, 94, 50), StartRow = 0, StartCol = 161 });
            propsSprite.AddAnimation("bed2", new SpriteAnimation { FrameRate = 1, FrameCount = 1, Size = new Rectangle(0, 0, 47, 87), StartRow = 0, StartCol = 52 });
            propsSprite.AddAnimation("bone", new SpriteAnimation { FrameRate = 1, FrameCount = 1, Size = new Rectangle(0, 0, 29, 16), StartRow = 95, StartCol = 0 });
            propsSprite.AddAnimation("boots", new SpriteAnimation { FrameRate = 1, FrameCount = 1, Size = new Rectangle(0, 0, 33, 33), StartRow = 50, StartCol = 218 });
            propsSprite.AddAnimation("bucket", new SpriteAnimation { FrameRate = 1, FrameCount = 1, Size = new Rectangle(0, 0, 30, 34), StartRow = 83, StartCol = 218 });
            propsSprite.AddAnimation("chest1", new SpriteAnimation { FrameRate = 1, FrameCount = 1, Size = new Rectangle(0, 0, 52, 95), StartRow = 0, StartCol = 0 });
            propsSprite.AddAnimation("pillar", new SpriteAnimation { FrameRate = 1, FrameCount = 1, Size = new Rectangle(0, 0, 62, 63), StartRow = 0, StartCol = 99 });
            propsSprite.AddAnimation("skull", new SpriteAnimation { FrameRate = 1, FrameCount = 1, Size = new Rectangle(0, 0, 30, 28), StartRow = 87, StartCol = 52 });
            propsSprite.AddAnimation("stool", new SpriteAnimation { FrameRate = 1, FrameCount = 1, Size = new Rectangle(0, 0, 37, 36), StartRow = 50, StartCol = 181 });

            propsSprite.SetAnimation("barrel1");
            propsSprite.X = 650;
            propsSprite.Y = 100;
            propsSprite.DrawOrder = 100;

            ImageLayers.Add(propsSprite.Clone());

            propsSprite.SetAnimation("bed1");
            propsSprite.X = 515;
            propsSprite.Y = 100;
            propsSprite.DrawOrder = 100;

            ImageLayers.Add(propsSprite.Clone());

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

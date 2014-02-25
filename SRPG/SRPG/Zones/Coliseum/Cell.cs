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

        public Cell(Game game, Torch.Object parent, byte[] data) : base(game, data)
        {
            Name = "Coliseum Slave Cells";
            SandbagImage = "Zones/Coliseum/Cell/sandbag";
            Sandbag = Grid.FromBitmap(Game.Services, SandbagImage);
            ImageLayers.Add(new ImageObject(Game, parent, "Zones/Coliseum/Cell/cell"));
            ImageLayers.Add(new ImageObject(Game, parent, "Zones/Coliseum/Cell/bars") { X = 50, Y = 300, DrawOrder = 350 });
            ImageLayers.Add(new ImageObject(Game, parent, "Zones/Coliseum/Cell/bars") { X = 50, Y = 500, DrawOrder = 550 });
            Doors.Add(new Door { Location = new Rectangle(100, 650, 7*6, 8*6), Name = "bed", Orientation = Direction.Right });
            Doors.Add(new Door { Location = new Rectangle(905, 200, 45, 25), Name = "halls", Orientation = Direction.Down });

            Objects.Add(new InteractiveObject { Interact = SimpleDoor("coliseum/halls-south", "cell"), Location = new Rectangle(905, 190, 45, 15) });
#if DEBUG
            Objects.Add(new InteractiveObject
                {Interact = TestBattle("coliseum/halls"), Location = new Rectangle(230, 550, 10, 50)});
#endif
            var propsSprite = new SpriteObject(Game, parent, "Zones/Coliseum/Cell/props");
            propsSprite.AddAnimation("barrel1", new SpriteAnimation { FrameRate = 1, FrameCount = 1, Size = new Rectangle(0, 0, 41, 51), StartRow = 63, StartCol = 99 });
            propsSprite.AddAnimation("barrel2", new SpriteAnimation { FrameRate = 1, FrameCount = 1, Size = new Rectangle(0, 0, 41, 50), StartRow = 63, StartCol = 140 });
            propsSprite.AddAnimation("bed1", new SpriteAnimation { FrameRate = 1, FrameCount = 1, Size = new Rectangle(0, 0, 94, 50), StartRow = 0, StartCol = 161 });
            propsSprite.AddAnimation("bed1-r", new SpriteAnimation { FrameRate = 1, FrameCount = 1, Size = new Rectangle(0, 0, 94, 50), StartRow = 0, StartCol = 161 });
            propsSprite.AddAnimation("bed2", new SpriteAnimation { FrameRate = 1, FrameCount = 1, Size = new Rectangle(0, 0, 47, 87), StartRow = 0, StartCol = 52 });
            propsSprite.AddAnimation("bone", new SpriteAnimation { FrameRate = 1, FrameCount = 1, Size = new Rectangle(0, 0, 29, 16), StartRow = 95, StartCol = 0 });
            propsSprite.AddAnimation("boots", new SpriteAnimation { FrameRate = 1, FrameCount = 1, Size = new Rectangle(0, 0, 33, 33), StartRow = 50, StartCol = 218 });
            propsSprite.AddAnimation("bucket", new SpriteAnimation { FrameRate = 1, FrameCount = 1, Size = new Rectangle(0, 0, 30, 34), StartRow = 83, StartCol = 218 });
            propsSprite.AddAnimation("chest1", new SpriteAnimation { FrameRate = 1, FrameCount = 1, Size = new Rectangle(0, 0, 52, 95), StartRow = 0, StartCol = 0 });
            propsSprite.AddAnimation("pillar", new SpriteAnimation { FrameRate = 1, FrameCount = 1, Size = new Rectangle(0, 0, 62, 63), StartRow = 0, StartCol = 99 });
            propsSprite.AddAnimation("skull", new SpriteAnimation { FrameRate = 1, FrameCount = 1, Size = new Rectangle(0, 0, 30, 28), StartRow = 87, StartCol = 52 });
            propsSprite.AddAnimation("stool", new SpriteAnimation { FrameRate = 1, FrameCount = 1, Size = new Rectangle(0, 0, 37, 36), StartRow = 50, StartCol = 181 });

            // bottom left room props
            ImageLayers.Add(propsSprite.Clone("bed2", 50, 550, 550));
            ImageLayers.Add(propsSprite.Clone("bed1-r", 145, 700, 150));
            ImageLayers.Add(propsSprite.Clone("bucket", 60, 700, 60));
            ImageLayers.Add(propsSprite.Clone("stool", 175, 640, 60));

            // bottom middle room props
            ImageLayers.Add(propsSprite.Clone("bed2", 260, 550, 550));
            ImageLayers.Add(propsSprite.Clone("bed2", 440, 550, 550));
            ImageLayers.Add(propsSprite.Clone("bucket", 270, 700, 270));
            ImageLayers.Add(propsSprite.Clone("boots", 450, 640, 270));

            // bottom right room props
            ImageLayers.Add(propsSprite.Clone("bed2", 510, 550, 550));
            ImageLayers.Add(propsSprite.Clone("bed1-r", 515, 700, 150));
            ImageLayers.Add(propsSprite.Clone("skull", 665, 710, 60));
            ImageLayers.Add(propsSprite.Clone("bucket", 655, 660, 60));

            // top left room props
            ImageLayers.Add(propsSprite.Clone("bed2", 50, 100, 100));
            ImageLayers.Add(propsSprite.Clone("bed1-r", 50, 300, 150));
            ImageLayers.Add(propsSprite.Clone("bucket", 200, 100, 150));

            // top middle room props
            ImageLayers.Add(propsSprite.Clone("bed2", 260, 100, 100));
            ImageLayers.Add(propsSprite.Clone("bed2", 440, 100, 100));
            ImageLayers.Add(propsSprite.Clone("bucket", 270, 250, 270));
            ImageLayers.Add(propsSprite.Clone("boots", 450, 190, 190));

            // top right room props
            ImageLayers.Add(propsSprite.Clone("barrel1", 650, 100, 100));
            ImageLayers.Add(propsSprite.Clone("bed1", 515, 100, 100));

            // outside the cells
            ImageLayers.Add(propsSprite.Clone("chest1", 50, 380, 50));

            Characters.Add("guard", Avatar.GenerateAvatar(game, null, "enemy"));

            if (data.Length == 0) data = new byte[1];

            if (data[0] == 0x00)
            {
                Characters["guard"].Location.X = 150;
                Characters["guard"].Location.Y = 440;
            } else
            {
                Characters["guard"].Location.X = 97;
                Characters["guard"].Location.Y = 440;
                Characters["guard"].UpdateVelocity(1, 0);
                Characters["guard"].UpdateVelocity(0, 0);

                _guardMoved = true;
            }

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

        public override byte[] ReadData()
        {
            var data = new byte[1];
            data[0] = (byte)(_guardMoved ? 0x01 : 0x00);
            return data;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SRPG.Data;
using Torch;
using Game = Microsoft.Xna.Framework.Game;

namespace SRPG.Zones.Coliseum
{
    class HallsSouth : Zone
    {
        public HallsSouth(Game game, Torch.Object parent, byte[] data)
            : base(game, data)
        {
            Name = "Coliseum Halls South";
            SandbagImage = "Zones/Coliseum/Halls/south-sb";
            Sandbag = Grid.FromBitmap(Game.Services, SandbagImage);
            ImageLayers.Add(new ImageObject(Game, parent, "Zones/Coliseum/Halls/south"));

            Doors.Add(new Door {Location = new Rectangle(345, 550, 50, 35), Name = "cell", Orientation = Direction.Up });
            Doors.Add(new Door { Location = new Rectangle(100, 100, 50, 35), Name = "halls-north", Orientation = Direction.Down });
            Doors.Add(new Door { Location = new Rectangle(660, 300, 35, 50), Name = "halls-east", Orientation = Direction.Left });

            Objects.Add(new InteractiveObject { Interact = SimpleDoor("coliseum/cell", "halls"), Location = new Rectangle(345, 585, 50, 25) });
            Objects.Add(new InteractiveObject { Interact = SimpleDoor("coliseum/halls-north", "halls-south"), Location = new Rectangle(100, 90, 50, 15) });
            Objects.Add(new InteractiveObject { Interact = SimpleDoor("coliseum/halls-east", "halls-south"), Location = new Rectangle(700, 300, 15, 50) });
        }
    }
}

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
    class HallsEast : Zone
    {
        public HallsEast(Game game, Torch.Object parent) : base(game)
        {
            Name = "Coliseum Halls East";
            SandbagImage = "Zones/Coliseum/Halls/east-sb";
            Sandbag = Grid.FromBitmap(Game.Services, SandbagImage);
            ImageLayers.Add(new ImageObject(Game, parent, "Zones/Coliseum/Halls/east"));

            Doors.Add(new Door {Location = new Rectangle(60, 450, 35, 50), Name = "halls-south", Orientation = Direction.Right });

            Objects.Add(new InteractiveObject { Interact = SimpleDoor("coliseum/halls-south", "halls-east"), Location = new Rectangle(50, 450, 15, 50) });
        }
    }
}

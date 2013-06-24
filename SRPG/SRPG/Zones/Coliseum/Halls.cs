using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SRPG.Data;
using Torch;
using Game = Torch.Game;

namespace SRPG.Zones.Coliseum
{
    class Halls : Zone
    {
        public Halls()
        {
            Name = "Coliseum Halls";
            Sandbag = Grid.FromBitmap("Zones/Coliseum/Halls/sandbag");
            ImageLayers.Add(new ImageObject(Game.GetInstance().Content.Load<Texture2D>("Zones/Coliseum/Halls/halls")));

            Doors.Add(new Door {Location = new Rectangle(734, 1933, 55, 8), Name = "cell", Orientation = Direction.Up });
            Doors.Add(new Door { Location = new Rectangle(487, 160, 51, 12), Name = "village", Orientation = Direction.Down });

            Objects.Add(new InteractiveObject{ Interact = SimpleDoor("coliseum/cell", "halls"), Location = new Rectangle(733, 1945, 60, 11) });
            Objects.Add(new InteractiveObject { Interact = SimpleDoor("village/village", "coliseum"), Location = new Rectangle(487, 138, 51, 12) });
        }
    }
}

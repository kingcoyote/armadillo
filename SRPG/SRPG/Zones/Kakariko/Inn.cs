using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Torch;
using Game = Torch.Game;

namespace SRPG.Data.Zones.Kakariko
{
    class Inn : Zone
    {
        public Inn()
        {
            Name = "Kakariko Inn";
            Sandbag = Grid.FromBitmap("Zones/Kakariko/Inn/sandbag");
            ImageLayers = new List<ImageObject>
                {
                    new ImageObject(Game.GetInstance().Content.Load<Texture2D>("Zones/Kakariko/Inn/inn")) {Z = -1},
                };

            Objects = new List<InteractiveObject>();

            Doors = new List<Door>
                {
                    new Door { Location = new Rectangle(258, 456, 59, 23), Name = "entrance", Orientation = Direction.Up, Zone = "kakariko/village", ZoneDoor = "inn" }
                };
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SRPG.Data;
using Torch;
using Game = Torch.Game;

namespace SRPG.Zones.Village
{
    class Inn : Zone
    {
        public Inn()
        {
            Name = "Village Inn";
            Sandbag = Grid.FromBitmap("Zones/Village/Inn/sandbag");
            ImageLayers.Add(new ImageObject(Game.GetInstance().Content.Load<Texture2D>("Zones/Village/Inn/inn")));
            Doors.Add(new Door { Location = new Rectangle(53*6, 95*6, 13*6, 2), Zone = "village/village", ZoneDoor = "inn exit", Name = "entrance", Orientation = Direction.Up });
        }
    }
}

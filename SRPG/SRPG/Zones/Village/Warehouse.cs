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
    class Warehouse : Zone
    {
        public Warehouse()
        {
            Name = "Village Warehouse";
            Sandbag = Grid.FromBitmap("Zones/Village/Warehouse/sandbag");

            ImageLayers.Add(new ImageObject("Zones/Village/Warehouse/warehouse"));

            Doors.Add(new Door { Location = new Rectangle(56*6, 130*6, 10*6, 1), Name = "entrance", Orientation = Direction.Up, Zone = "village/village", ZoneDoor = "warehouse exit"});
        }
    }
}

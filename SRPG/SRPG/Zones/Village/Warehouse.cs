using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SRPG.Data;
using Torch;
using Game = Microsoft.Xna.Framework.Game;

namespace SRPG.Zones.Village
{
    class Warehouse : Zone
    {
        public Warehouse(Game game, Torch.Object parent, byte[] data)
            : base(game, data)
        {
            Name = "Village Warehouse";
            SandbagImage = "Zones/Village/Warehouse/sandbag";
            Sandbag = Grid.FromBitmap(Game.Services, SandbagImage);

            ImageLayers.Add(new ImageObject(Game, parent, "Zones/Village/Warehouse/warehouse"));

            Doors.Add(new Door { Location = new Rectangle(56*6, 130*6, 10*6, 1), Name = "entrance", Orientation = Direction.Up, Zone = "village/village", ZoneDoor = "warehouse exit"});
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SRPG.Data;
using Torch;
using Game = Microsoft.Xna.Framework.Game;

namespace SRPG.Zones.Village
{
    public class Shop : Zone
    {
        public Shop(Game game, Torch.Object parent, byte[] data)
            : base(game, data)
        {
            Name = "Village Shop";
            SandbagImage = "Zones/Village/Shop/sandbag";
            Sandbag = Grid.FromBitmap(Game.Services, SandbagImage);

            ImageLayers.Add(new ImageObject(Game, parent, "Zones/Village/Shop/shop"));

            Doors.Add(new Door { Location = new Rectangle(33*6, 67*6, 8*6, 6), Name = "village", Orientation = Direction.Up, Zone = "village/village", ZoneDoor = "shop" });
        }
    }
}

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

            Objects.Add(new InteractiveObject { Interact = SimpleDoor("village/village", "shop"), Location = new Rectangle(180, 390, 80, 40) });

            Doors.Add(new Door { Location = new Rectangle(190, 370, 80, 15), Name = "village", Orientation = Direction.Up });
        }
    }
}

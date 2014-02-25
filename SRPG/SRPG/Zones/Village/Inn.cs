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
    class Inn : Zone
    {
        public Inn(Game game, Torch.Object parent, byte[] data)
            : base(game, data)
        {
            Name = "Village Inn";
            SandbagImage = "Zones/Village/Inn/sandbag";
            Sandbag = Grid.FromBitmap(Game.Services, SandbagImage);
            ImageLayers.Add(new ImageObject(Game, parent, "Zones/Village/Inn/inn"));
            Doors.Add(new Door { Location = new Rectangle(63*6, 93*6, 13*6, 2), Zone = "village/village", ZoneDoor = "inn exit", Name = "entrance", Orientation = Direction.Up });
        }
    }
}

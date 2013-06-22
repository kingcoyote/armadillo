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
    public class Village : Zone
    {
        public Village()
        {
            Name = "Desert Village";
            Sandbag = Grid.FromBitmap("Zones/Village/Village/sandbag");
            
            ImageLayers.Add(new ImageObject(Game.GetInstance().Content.Load<Texture2D>("Zones/Village/Village/village")));

            Doors.Add(new Door { Location = new Rectangle(1442, 2057, 171, 37), Name = "coliseum", Orientation = Direction.Up, Zone = "coliseum/halls", ZoneDoor = "village" });
        }
    }
}

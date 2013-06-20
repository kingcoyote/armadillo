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
    public class Town : Zone
    {
        public Town()
        {
            Name = "Desert Village";
            Sandbag = Grid.FromBitmap("Zones/Village/Village/sandbag");
            ImageLayers = new List<ImageObject>()
                {
                    new ImageObject(Game.GetInstance().Content.Load<Texture2D>("Zones/Village/Village/town"))
                };

            Doors = new List<Door>()
                {
                    new Door { Location = new Rectangle(1442, 2057, 171, 37), Name = "coliseum", Orientation = Direction.Up },
                };
        }
    }
}

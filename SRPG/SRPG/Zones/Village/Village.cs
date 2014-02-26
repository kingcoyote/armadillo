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
    public class Village : Zone
    {
        public Village(Game game, Torch.Object parent, byte[] data)
            : base(game, data)
        {
            Name = "Desert Village";
            SandbagImage = "Zones/Village/Village/sandbag";
            Sandbag = Grid.FromBitmap(Game.Services, SandbagImage);
            
            ImageLayers.Add(new ImageObject(Game, parent, "Zones/Village/Village/village"));

            ImageLayers.Add(new ImageObject(Game, parent, "Zones/Village/Village/inn") { X = 840, Y = 1372, DrawOrder = 1752 });
            ImageLayers.Add(new ImageObject(Game, parent, "Zones/Village/Village/warehouse") { X = 155, Y = 1300, DrawOrder = 1918 });
            ImageLayers.Add(new ImageObject(Game, parent, "Zones/Village/Village/well") { X = 1400, Y = 863, DrawOrder = 977 });
            ImageLayers.Add(new ImageObject(Game, parent, "Zones/Village/Village/Posts1") { X = 70, Y = 1940, DrawOrder = 2100 });
            ImageLayers.Add(new ImageObject(Game, parent, "Zones/Village/Village/house") { X = 754, Y = 701, DrawOrder = 1094 });

            Objects.Add(new InteractiveObject { Interact = SimpleDoor("village/shop", "village"), Location = new Rectangle(164*6, 192*6, 80, 40) });

            Doors.Add(new Door { Location = new Rectangle(1442, 2057, 171, 37), Name = "coliseum", Orientation = Direction.Up, Zone = "coliseum/halls-north", ZoneDoor = "village" });
            
            Doors.Add(new Door { Location = new Rectangle(69*6, 315*6, 9*6, 2), Name = "warehouse", Orientation = Direction.Down, Zone = "village/warehouse", ZoneDoor = "entrance" });
            Doors.Add(new Door { Location = new Rectangle(69*6, 320*6, 9*6, 2), Name = "warehouse exit", Orientation = Direction.Down });

            Doors.Add(new Door { Location = new Rectangle(177 * 6, 288 * 6, 12 * 6, 2), Name = "inn", Orientation = Direction.Down, Zone = "village/inn", ZoneDoor = "entrance" });
            Doors.Add(new Door { Location = new Rectangle(177 * 6, 295 * 6, 12 * 6, 2), Name = "inn exit", Orientation = Direction.Down });
            Doors.Add(new Door { Location = new Rectangle(164 * 6, 200 * 6, 80, 40), Name = "shop", Orientation = Direction.Down });


        }
    }
}

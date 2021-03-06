using Microsoft.Xna.Framework;
using SRPG.Data;
using Torch;
using System.Collections.Generic;
using Game = Microsoft.Xna.Framework.Game;

namespace SRPG.Zones.Kakariko
{
    class Village : Zone
    {
        public Village(Game game, Torch.Object parent, byte[] data)
            : base(game, data)
        {

            Name = "Kakariko Village";
            SandbagImage = "Zones/Kakariko/village/sandbag";
            Sandbag = Grid.FromBitmap(Game.Services, SandbagImage);
            ImageLayers = new List<ImageObject>
                {
                    new ImageObject(Game, parent, "Zones/Kakariko/Village/village") {DrawOrder = -1}, 
                    new ImageObject(Game, parent, "Zones/Kakariko/Village/arch") {X = 2568, Y = 2784, DrawOrder = 2925}, 
                    new ImageObject(Game, parent, "Zones/Kakariko/Village/house_1") {X = 1728, Y = 336, DrawOrder = 587},
                    new ImageObject(Game, parent, "Zones/Kakariko/Village/mailbox") {X = 483, Y = 1152, DrawOrder = 1224},
                    new ImageObject(Game, parent, "Zones/Kakariko/Village/cave") {X = 534, Y = 565, DrawOrder = 660},
                    new ImageObject(Game, parent, "Zones/Kakariko/Village/bombshop") {X = 240, Y = 2544, DrawOrder = 2750},
                };

            Objects = new List<InteractiveObject>();

            // mailbox
            var obj = new InteractiveObject { Location = new Rectangle(486, 1200, 36, 24) };
            obj.Interact += SimpleDialog("kakariko/town", "mailbox");
            Objects.Add(obj);

            // statue
            obj = new InteractiveObject { Location = new Rectangle(1512, 1224, 96, 96) };
            obj.Interact += SimpleDialog("kakariko/town", "statue");
            Objects.Add(obj);

            // cliff
            obj = new InteractiveObject { Location = new Rectangle(268, 445, 312 - 268, 5) };
            obj.Interact += SimpleDialog("kakariko/town", "cliff");
            Objects.Add(obj);

            // inn
            obj = new InteractiveObject {Location = new Rectangle(1895, 2009, 48, 7)};
            obj.Interact += SimpleDoor("kakariko/inn", "entrance");
            Objects.Add(obj);

            // merchant
            obj = new InteractiveObject {Location = new Rectangle(1941, 2736, 54, 12)};
            obj.Interact += SimpleMerchant("kakariko/town", "merchant");
            Objects.Add(obj);

            Doors = new List<Door>
                {
                    new Door { Location = new Rectangle(312, 2725, 48, 8), Name = "bombshop", Orientation = Direction.Down, Zone = "kakariko/bombshop", ZoneDoor = "entrance" },
                    new Door { Location = new Rectangle(2664, 2997, 48, 32), Name = "arch", Orientation = Direction.Up },
                    new Door { Location = new Rectangle(1895, 2030, 48, 7), Name = "inn", Orientation = Direction.Down }
                };
        }
    }
}

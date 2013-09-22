using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SRPG.Data;
using Torch;
using Game = Torch.Game;

namespace SRPG.Zones.Kakariko
{
    class Bombshop : Zone
    {
        public Bombshop(Microsoft.Xna.Framework.Game game, Torch.Object parent) : base(game)
        {
            Name = "Kakariko Shop";
            Sandbag = Grid.FromBitmap(Game.Services, "Zones/Kakariko/Bombshop/sandbag");
            ImageLayers = new List<ImageObject>
                {
                    new ImageObject(Game, parent, "Zones/Kakariko/Bombshop/bombshop") {DrawOrder = -1},
                };

            Objects = new List<InteractiveObject>();

            Doors = new List<Door>
                {
                    new Door { Location = new Rectangle(264, 457, 48, 14), Name = "entrance", Orientation = Direction.Up, Zone = "kakariko/village", ZoneDoor = "bombshop" }
                };
        }
    }
}

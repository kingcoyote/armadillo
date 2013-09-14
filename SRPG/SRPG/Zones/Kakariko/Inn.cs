using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SRPG.Data;
using Torch;
using Game = Microsoft.Xna.Framework.Game;

namespace SRPG.Zones.Kakariko
{
    class Inn : Zone
    {
        public Inn(Game game) : base(game)
        {
            Name = "Kakariko Inn";
            Sandbag = Grid.FromBitmap(Game.Services, "Zones/Kakariko/Inn/sandbag");
            ImageLayers = new List<ImageObject>
                {
                    new ImageObject(Game, "Zones/Kakariko/Inn/inn") { DrawOrder = -1},
                };

            Objects = new List<InteractiveObject>();

            Doors = new List<Door>
                {
                    new Door { Location = new Rectangle(258, 456, 59, 23), Name = "entrance", Orientation = Direction.Up, Zone = "kakariko/village", ZoneDoor = "inn" }
                };
        }
    }
}

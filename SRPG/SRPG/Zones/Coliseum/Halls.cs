using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SRPG.Data;
using Torch;
using Game = Microsoft.Xna.Framework.Game;

namespace SRPG.Zones.Coliseum
{
    class Halls : Zone
    {
        public Halls(Game game, Torch.Object parent) : base(game)
        {
            Name = "Coliseum Halls";
            Sandbag = Grid.FromBitmap(Game.Services, "Zones/Coliseum/Halls/sandbag");
            ImageLayers.Add(new ImageObject(Game, parent, "Zones/Coliseum/Halls/halls"));

            Doors.Add(new Door {Location = new Rectangle(691, 1847, 67, 43), Name = "cell", Orientation = Direction.Up });
            Doors.Add(new Door { Location = new Rectangle(446, 108, 57, 31), Name = "village", Orientation = Direction.Down });

            Objects.Add(new InteractiveObject{ Interact = SimpleDoor("coliseum/cell", "halls"), Location = new Rectangle(691, 1895, 67, 12) });
            Objects.Add(new InteractiveObject { Interact = SimpleDoor("village/village", "coliseum"), Location = new Rectangle(446, 90, 57, 12) });

            Objects.Add(new InteractiveObject { Interact = TestBattle("coliseum/halls"), Location = new Rectangle(1839, 1070, 16, 65) });
        }
    }
}

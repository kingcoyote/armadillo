using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SRPG.Data;
using Torch;
using Game = Torch.Game;

namespace SRPG.Zones.Coliseum
{
    class Cell : Zone
    {
        public Cell()
        {
            Name = "Coliseum Slave Cells";
            Sandbag = Grid.FromBitmap("Zones/Coliseum/Cell/sandbag");
            ImageLayers.Add(new ImageObject(Game.GetInstance().Content.Load<Texture2D>("Zones/Coliseum/Cell/cell")));
            Doors.Add(new Door { Location = new Rectangle(254, 731, 70, 64), Name = "bed", Orientation = Direction.Right });
            Doors.Add(new Door { Location = new Rectangle(1851, 162, 48, 9), Name = "halls", Orientation = Direction.Down });

            Objects.Add(new InteractiveObject { Interact = SimpleDoor("coliseum/halls", "cell"), Location = new Rectangle(1850, 143, 49, 12) });
        }
    }
}

using Microsoft.Xna.Framework;
using SRPG.Data;
using Torch;
using Game = Microsoft.Xna.Framework.Game;

namespace SRPG.Zones.Coliseum
{
    class HallsNorth : Zone
    {
        public HallsNorth(Game game, Object parent) : base(game)
        {
            Name = "Coliseum Halls North";
            Sandbag = Grid.FromBitmap(Game.Services, "Zones/Coliseum/Halls/north-sb");
            ImageLayers.Add(new ImageObject(Game, parent, "Zones/Coliseum/Halls/north"));

            Doors.Add(new Door {Location = new Rectangle(500, 715, 50, 35), Name = "halls-south", Orientation = Direction.Up });
            Doors.Add(new Door { Location = new Rectangle(455, 100, 140, 35), Name = "village", Orientation = Direction.Up });

            Objects.Add(new InteractiveObject { Interact = SimpleDoor("coliseum/halls-south", "halls-north"), Location = new Rectangle(500, 740, 50, 15) });
            Objects.Add(new InteractiveObject { Interact = SimpleDoor("village/village", "coliseum"), Location = new Rectangle(455, 90, 140, 15) });
        }
    }
}

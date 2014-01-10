using Microsoft.Xna.Framework;
using SRPG.Data;
using Torch;
using Game = Microsoft.Xna.Framework.Game;
using Object = Torch.Object;

namespace SRPG.Zones.Coliseum
{
    class HallsNorth : Zone
    {
        public HallsNorth(Game game, Object parent) : base(game)
        {
            Name = "Coliseum Halls North";
            Sandbag = Grid.FromBitmap(Game.Services, "Zones/Coliseum/Halls/north-sb");
            ImageLayers.Add(new ImageObject(Game, parent, "Zones/Coliseum/Halls/north"));
            ImageLayers.Add(new ImageObject(Game, parent, "Zones/Coliseum/pillar") { X = 393, Y = 190, DrawOrder = 248 });
            ImageLayers.Add(new ImageObject(Game, parent, "Zones/Coliseum/pillar") { X = 393, Y = 390, DrawOrder = 448 });
            ImageLayers.Add(new ImageObject(Game, parent, "Zones/Coliseum/pillar") { X = 393, Y = 590, DrawOrder = 648 });
            ImageLayers.Add(new ImageObject(Game, parent, "Zones/Coliseum/pillar") { X = 593, Y = 190, DrawOrder = 248 });
            ImageLayers.Add(new ImageObject(Game, parent, "Zones/Coliseum/pillar") { X = 593, Y = 390, DrawOrder = 448 });
            ImageLayers.Add(new ImageObject(Game, parent, "Zones/Coliseum/pillar") { X = 593, Y = 590, DrawOrder = 648 });

            Doors.Add(new Door {Location = new Rectangle(500, 715, 50, 35), Name = "halls-south", Orientation = Direction.Up });
            Doors.Add(new Door { Location = new Rectangle(455, 100, 140, 35), Name = "village", Orientation = Direction.Up });

            Objects.Add(new InteractiveObject { Interact = SimpleDoor("coliseum/halls-south", "halls-north"), Location = new Rectangle(500, 740, 50, 15) });
            Objects.Add(new InteractiveObject { Interact = SimpleDoor("village/village", "coliseum"), Location = new Rectangle(455, 90, 140, 15) });

            Objects.Add(new InteractiveObject { Interact = TestBattle("coliseum/halls"), Location = new Rectangle(400, 200, 50, 50) });
            Objects.Add(new InteractiveObject { Interact = TestBattle("coliseum/halls"), Location = new Rectangle(400, 400, 50, 50) });
            Objects.Add(new InteractiveObject { Interact = TestBattle("coliseum/halls"), Location = new Rectangle(400, 600, 50, 50) });
            Objects.Add(new InteractiveObject { Interact = TestBattle("coliseum/halls"), Location = new Rectangle(600, 200, 50, 50) });
            Objects.Add(new InteractiveObject { Interact = TestBattle("coliseum/halls"), Location = new Rectangle(600, 400, 50, 50) });
            Objects.Add(new InteractiveObject { Interact = TestBattle("coliseum/halls"), Location = new Rectangle(600, 600, 50, 50) });

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using SRPG.Scene.Overworld;
using Torch;

namespace SRPG.Data
{
    public class Zone
    {
        /// <summary>
        /// Machine readable zone name.
        /// </summary>
        public string Name;
        /// <summary>
        /// A grid indicating whether or not a square is accessible. 0 means accessible, 1 means inaccessible.
        /// </summary>
        public Grid Sandbag;
        /// <summary>
        /// A collection of images that represent the visual elements of this zone. The player is at Z-index 1, so anything below
        /// will be background, anything above will be above the player.
        /// </summary>
        public List<ImageObject> ImageLayers;
        /// <summary>
        /// A collection of doors that, when entered by the player, will go to a new zone.
        /// </summary>
        public List<Door> Doors;

        public List<InteractiveObject> Objects;

        public static Zone Factory(string name)
        {
            var zone = new Zone();

            switch(name)
            {
                case "kakariko/village":
                    return new Zones.Kakariko.Village();
                case "kakariko/bombshop":
                    return new Zones.Kakariko.Bombshop();
                case "kakariko/inn":
                    return new Zones.Kakariko.Inn();
                default:
                    throw new ZoneException(String.Format("Unable to generate unknown zone '{0}'.", name));
            }

            return zone;
        }

        public EventHandler<InteractEventArgs> SimpleDialog(string filename, string objectname)
        {
            return (sender, args) =>
                {
                    var scene = ((OverworldScene)sender);
                    var dialog = Dialog.Fetch(filename, objectname);
                    scene.StartDialog(dialog);
                };
        }

        public EventHandler<InteractEventArgs> SimpleDoor(string zone, string zonedoor)
        {
            return (sender, args) => ((OverworldScene) sender).SetZone(Factory(zone), zonedoor);
        }

        public EventHandler<InteractEventArgs> SimpleMerchant(string filename, string merchantname)
        {
            return (sender, args) =>
                {
                    var scene = ((OverworldScene) sender);
                    var dialog = Dialog.Fetch(filename, merchantname);
                    dialog.OnExit = (s, a) => ((SRPGGame)Game.GetInstance()).LaunchShop(filename, merchantname);
                    scene.StartDialog(dialog);
                };
        }
    }

    class ZoneException : Exception
    {
        public ZoneException(string message) : base(message) { }
    }
}

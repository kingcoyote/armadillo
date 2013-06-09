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
                case "kakariko":
                    return new Zones.Kakariko();
                    break;
                case "kakariko shack":
                    return new Zones.KakarikoShack();
                    break;
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
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SRPG.Data
{
    public struct Door
    {
        /// <summary>
        /// A machine readable name for this door. It is used when a player enters the zone, to identify the starting location of the player.
        /// </summary>
        public string Name;
        /// <summary>
        /// An indicator of what way this door is facing, so that the player is oriented correctly upon entering the zone.
        /// </summary>
        public Direction Orientation;
        /// <summary>
        /// An X,Y pair indicating where in the zone this door is located.
        /// </summary>
        public Rectangle Location;
        /// <summary>
        /// A machine readable string indicating which zone this door will lead to. This corresponds to Zone.Name
        /// </summary>
        public string Zone;
        /// <summary>
        /// A machine readable string indicating which door in the target zone the player will spawn at. This corresponds to Door.Name
        /// </summary>
        public string ZoneDoor;
    }
}

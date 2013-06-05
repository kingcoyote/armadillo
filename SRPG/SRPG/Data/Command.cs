using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SRPG.Data
{
    public struct Command
    {
        /// <summary>
        /// The character who is performing the action
        /// </summary>
        public Character Character;
        /// <summary>
        /// The ability being performed.
        /// </summary>
        public Ability Ability;
        /// <summary>
        /// The X,Y coordinate that is being targetted.
        /// </summary>
        public Point Target;
        /// <summary>
        /// The direction the ability is aiming, for abilities that can be rotated.
        /// </summary>
        public Direction Orientation;
    }
}

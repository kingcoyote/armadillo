using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SRPG.Data
{
    public class BattleBoard
    {
        /// <summary>
        /// A list of all characters that are currently on the board.
        /// </summary>
        public List<Combatant> Characters = new List<Combatant>();
        /// <summary>
        /// A grid indicating what squares are inaccessible due to obstructions, such as water, walls, tables, etc. A 0 indicates
        /// that a square is inaccessible.
        /// </summary>
        public Grid Sandbag;

        /// <summary>
        /// Return a true value is a member of the specified faction is able to enter the square at location. This is used
        /// to determine movement ranges.
        /// </summary>
        /// <param name="location">An X,Y pair indicating which location to check.</param>
        /// <param name="faction">A integer indicating which faction is checking. Since characters can move through squares occupied
        /// by their allies, an enemy is considered an obstruction, while an ally is not.</param>
        /// <returns>true if the square is accessible</returns>
        public bool IsAccessible(Point location, int faction)
        {
            // out of bounds
            if (location.X < 0 || location.X >= Sandbag.Size.Width || location.Y < 0 || location.Y >= Sandbag.Size.Height)
            {
                return false;
            }

            // can't walk here - wall, table, tree, etc.
            if (Sandbag.Weight[location.X, location.Y] < 1)
            {
                return false;
            }

            // enemy is standing here
            var character = (from c in Characters where Math.Abs(c.Avatar.Location.X - location.X) < 0.1 && Math.Abs(c.Avatar.Location.Y - location.Y) < 0.1 select c);
            if(character.Any() && character.First().Faction != faction)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Return an integer value specifying which faction occupies a square.
        /// </summary>
        /// <param name="location">An X,Y pair indicating which location to check.</param>
        /// <returns>-1 for unoccupied, 0 for the player's faction, 1 for the enemy faction.</returns>
        public int IsOccupied(Point location)
        {
            throw new NotImplementedException();
        }
    }
}

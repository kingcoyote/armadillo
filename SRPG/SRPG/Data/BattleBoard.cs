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
        public List<Character> Characters;
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
            throw new NotImplementedException();
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

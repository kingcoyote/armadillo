using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SRPG.Data;

namespace SRPG.Abilities
{
    class SteelWall : Ability
    {
        public SteelWall(Game game)
            : base(game)
        {
            Name = "Steel Wall";
            AbilityType = AbilityType.Passive;
            ItemType = ItemType.Plate;
        }
    }
}

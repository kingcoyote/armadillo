using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SRPG.Data;

namespace SRPG.Abilities
{
    class Sprint : Ability
    {
        public Sprint(Game game)
            : base(game)
        {
            Name = "Sprint";
            AbilityType = AbilityType.Passive;
            ItemType = ItemType.Leather;
        }

    }
}

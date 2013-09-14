using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SRPG.Data;

namespace SRPG.Abilities
{
    class Focus : Ability
    {
        public Focus(Game game)
            : base(game)
        {
            Name = "Focus";
            AbilityType = AbilityType.Passive;
            ItemType = ItemType.Cloth;
        }
    }
}

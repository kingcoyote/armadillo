using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SRPG.Data;

namespace SRPG.Abilities
{
    class Untouchable : Ability
    {
        public Untouchable(Game game)
            : base(game)
        {
            Name = "Untouchable";
            AbilityType = AbilityType.Passive;
            ItemType = ItemType.Leather;
        }
    }
}

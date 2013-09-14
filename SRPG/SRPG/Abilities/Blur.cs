using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SRPG.Data;

namespace SRPG.Abilities
{
    class Blur : Ability
    {
        public Blur(Game game)
            : base(game)
        {
            Name = "Blur";
            AbilityType = AbilityType.Passive;
            ItemType = ItemType.Leather;
        }
    }
}

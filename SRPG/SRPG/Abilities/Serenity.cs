using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SRPG.Data;

namespace SRPG.Abilities
{
    class Serenity : Ability
    {
        public Serenity(Game game)
            : base(game)
        {
            Name = "Serenity";
            AbilityType = AbilityType.Passive;
            ItemType = ItemType.Cloth;
        }
    }
}

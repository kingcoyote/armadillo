using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SRPG.Data;

namespace SRPG.Abilities
{
    class Serenity : Ability
    {
        public Serenity()
        {
            Name = "Serenity";
            AbilityType = AbilityType.Passive;
            ItemType = ItemType.Cloth;
        }
    }
}

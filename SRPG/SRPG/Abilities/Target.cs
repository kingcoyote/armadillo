using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SRPG.Data;

namespace SRPG.Abilities
{
    class Target : Ability
    {
        public Target()
        {
            Name = "Target";
            AbilityType = AbilityType.Passive;
            ItemType = ItemType.Cloth;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SRPG.Data;

namespace SRPG.Abilities
{
    class Sprint : Ability
    {
        public Sprint()
        {
            Name = "Sprint";
            AbilityType = AbilityType.Passive;
            ItemType = ItemType.Leather;
        }

    }
}

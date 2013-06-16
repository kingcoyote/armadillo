using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SRPG.Data;

namespace SRPG.Abilities
{
    class Focus : Ability
    {
        public Focus()
        {
            Name = "Focus";
            AbilityType = AbilityType.Passive;
            ItemType = ItemType.Cloth;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SRPG.Data;

namespace SRPG.Abilities
{
    class Blur : Ability
    {
        public Blur()
        {
            Name = "Blur";
            AbilityType = AbilityType.Passive;
            ItemType = ItemType.Leather;
        }
    }
}

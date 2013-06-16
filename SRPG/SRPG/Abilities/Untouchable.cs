using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SRPG.Data;

namespace SRPG.Abilities
{
    class Untouchable : Ability
    {
        public Untouchable()
        {
            Name = "Untouchable";
            AbilityType = AbilityType.Passive;
            ItemType = ItemType.Leather;
        }
    }
}

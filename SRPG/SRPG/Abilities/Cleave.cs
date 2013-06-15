using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SRPG.Data;

namespace SRPG.Abilities
{
    class Cleave : Ability
    {
        public Cleave()
        {
            Name = "Cleave";
            AbilityType = AbilityType.Active;
            AbilityTarget = AbilityTarget.Enemy;
            ItemType = ItemType.Sword;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SRPG.Data;

namespace SRPG.Abilities
{
    class CobraPunch : Ability
    {

        public CobraPunch()
        {
            Name = "Cobra Punch";
            AbilityType = AbilityType.Active;
            AbilityTarget = AbilityTarget.Enemy;
            ItemType = ItemType.Unarmed;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SRPG.Data;

namespace SRPG.Abilities
{
    class WhipKick : Ability
    {
        public WhipKick()
        {
            Name = "Whip Kick";
            AbilityType = AbilityType.Active;
            AbilityTarget = AbilityTarget.Enemy;
            ItemType = ItemType.Unarmed;
        }
    }
}

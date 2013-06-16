using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SRPG.Data;

namespace SRPG.Abilities
{
    class FlyingKnee : Ability
    {

        public FlyingKnee()
        {
            Name = "Flying Knee";
            AbilityType = AbilityType.Active;
            AbilityTarget = AbilityTarget.Enemy;
            ItemType = ItemType.Unarmed;
        }
    }
}

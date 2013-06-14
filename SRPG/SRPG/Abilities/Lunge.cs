using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SRPG.Data;

namespace SRPG.Abilities
{
    class Lunge : Ability
    {
        public Lunge()
        {
            Name = "Lunge";
            AbilityType = AbilityType.Active;
            AbilityTarget = AbilityTarget.Enemy;
            ItemType = ItemType.Sword;
        }
    }
}

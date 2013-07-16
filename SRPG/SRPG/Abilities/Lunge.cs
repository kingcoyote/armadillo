using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SRPG.Data;
using Torch;

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
            SetIcon("swordicons", 0);
        }
    }
}

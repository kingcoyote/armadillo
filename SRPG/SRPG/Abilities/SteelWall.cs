using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SRPG.Data;

namespace SRPG.Abilities
{
    class SteelWall : Ability
    {
        public SteelWall()
        {
            Name = "Steel Wall";
            AbilityType = AbilityType.Passive;
            ItemType = ItemType.Plate;
        }
    }
}

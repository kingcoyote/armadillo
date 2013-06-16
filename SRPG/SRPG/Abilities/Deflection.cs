using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SRPG.Data;

namespace SRPG.Abilities
{
    class Deflection : Ability
    {
        public Deflection()
        {
            Name = "Deflection";
            AbilityType = AbilityType.Passive;
            ItemType = ItemType.Plate;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SRPG.Data;

namespace SRPG.Abilities
{
    class Deflection : Ability
    {
        public Deflection(Game game)
            : base(game)
        {
            Name = "Deflection";
            AbilityType = AbilityType.Passive;
            ItemType = ItemType.Plate;
        }
    }
}

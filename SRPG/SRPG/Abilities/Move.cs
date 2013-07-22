using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SRPG.Data;

namespace SRPG.Abilities
{
    class Move : Ability
    {
        public Move()
        {
            Name = "Move";
            AbilityTarget = AbilityTarget.Unoccupied;
        }

        public override Grid GenerateImpactGrid()
        {
            return new Grid(1, 1, 1);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SRPG.Data;

namespace SRPG.Abilities
{
    class Revive : Ability
    {
        public Revive()
        {
            Name = "Revive";
            AbilityType = AbilityType.Active;
            AbilityTarget = AbilityTarget.Friendly;
            ItemType = ItemType.Book;
        }
    }
}

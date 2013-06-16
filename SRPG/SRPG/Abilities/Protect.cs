using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SRPG.Data;

namespace SRPG.Abilities
{
    class Protect : Ability
    {
        public Protect()
        {
            Name = "Protect";
            AbilityType = AbilityType.Active;
            AbilityTarget = AbilityTarget.Friendly;
            ItemType = ItemType.Book;
        }
    }
}

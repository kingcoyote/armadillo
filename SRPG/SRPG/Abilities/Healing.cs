using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SRPG.Data;

namespace SRPG.Abilities
{
    class Healing : Ability
    {
        public Healing()
        {
            Name = "Healing";
            AbilityType = AbilityType.Active;
            AbilityTarget = AbilityTarget.Friendly;
            ItemType = ItemType.Book;
        }
    }
}

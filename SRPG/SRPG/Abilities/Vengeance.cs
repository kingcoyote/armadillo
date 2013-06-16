using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SRPG.Data;

namespace SRPG.Abilities
{
    class Vengeance : Ability
    {
        public Vengeance()
        {
            Name = "Vengeance";
            AbilityType = AbilityType.Passive;
            ItemType = ItemType.Mail;
        }
    }
}

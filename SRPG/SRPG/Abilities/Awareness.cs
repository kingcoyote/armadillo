using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SRPG.Data;

namespace SRPG.Abilities
{
    class Awareness : Ability
    {
        public Awareness()
        {
            Name = "Awareness";
            AbilityType = AbilityType.Passive;
            ItemType = ItemType.Mail;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SRPG.Data;

namespace SRPG.Abilities
{
    class Vengeance : Ability
    {
        public Vengeance(Game game)
            : base(game)
        {
            Name = "Vengeance";
            AbilityType = AbilityType.Passive;
            ItemType = ItemType.Mail;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SRPG.Data;

namespace SRPG.Abilities
{
    class Awareness : Ability
    {
        public Awareness(Game game)
            : base(game)
        {
            Name = "Awareness";
            AbilityType = AbilityType.Passive;
            ItemType = ItemType.Mail;
        }
    }
}

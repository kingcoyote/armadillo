using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
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
            SetIcon("magicicons", 0);
            ManaCost = 5;
        }

        public override List<Hit> GenerateHits(BattleBoard board, Point target)
        {
            return new List<Hit>
                {
                    new Hit
                        {
                            Critical = 50,
                            Damage = 10,
                            Delay = 500,
                            Target = target
                        }
                };
        }

        public override Grid GenerateTargetGrid()
        {
            return Grid.FromBitmap("Items/target_melee_medium");
        }
    }
}

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SRPG.Data;

namespace SRPG.Abilities
{
    class Lunge : Ability
    {
        public Lunge()
        {
            Name = "Lunge";
            AbilityType = AbilityType.Active;
            AbilityTarget = AbilityTarget.Enemy;
            ItemType = ItemType.Sword;
            SetIcon("swordicons", 0);
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
            return Grid.FromBitmap("Items/target_melee_small");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SRPG.Data;

namespace SRPG.Abilities
{
    class DrinkPotion : Ability
    {
        public DrinkPotion(Game game) : base(game)
        {
            Name = "Drink Potion";
            AbilityTarget = AbilityTarget.Friendly;
        }

        public override List<Hit> GenerateHits(BattleBoard board, Point target)
        {
            if (!CanHit(board.Sandbag, target)) return new List<Hit>();

            return new List<Hit>
                {
                    new Hit
                        {
                            Critical = 0,
                            Damage = -12,
                            Delay = 200,
                            Target = target
                        }
                };
        }

        public override Grid GenerateTargetGrid()
        {
            return Grid.FromBitmap(Game.Services, "Items/target_melee_small");
        }
    }
}

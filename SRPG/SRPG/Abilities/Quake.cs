using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SRPG.Data;

namespace SRPG.Abilities
{
    class Quake : Ability
    {
        public Quake()
        {
            Name = "Quake";
            AbilityType = AbilityType.Active;
            AbilityTarget = AbilityTarget.Enemy;
            ItemType = ItemType.Staff;
            SetIcon("magicicons", 0);
            ManaCost = 5;
        }

        public override List<Hit> GenerateHits(BattleBoard board, Point target)
        {
            var grid = GenerateImpactGrid();
            var hits = new List<Hit>();

            for(var x = 0; x < grid.Size.Width; x++)
            {
                for(var y = 0; y < grid.Size.Height; y++)
                {
                    if (grid.Weight[x, y] < 1) continue;

                    var currentTarget = new Point(target.X - 12 + x, target.Y - 12 + y);

                    var combatant = board.GetCharacterAt(currentTarget);

                    if(combatant == null || ! CanTarget(combatant.Faction)) continue;

                    hits.Add(new Hit {
                            Target = currentTarget,
                            Critical = 50,
                            Damage = 7,
                    });
                }
            }

            return hits;
        }

        public override Grid GenerateTargetGrid()
        {
            return Grid.FromBitmap("Items/target_melee_large");
        }

        public override Grid GenerateImpactGrid()
        {
            return Grid.FromBitmap("Items/target_melee_small");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SRPG.Data;

namespace SRPG.Abilities
{
    class Attack : Ability
    {
        public Attack()
        {
            Name = "Attack";
        }

        public override List<Hit> GenerateHits(BattleBoard board, Point target)
        {
            return new List<Hit>()
                {
                    new Hit()
                        {
                            Critical = 50,
                            Damage = 5,
                            Delay = 200,
                            Target = target
                        }
                };
        }

        public override Grid GenerateImpactGrid()
        {
            var grid = new Grid(25, 25);
            grid.Weight[12, 12] = 255;

            return grid;
        }
    }
}

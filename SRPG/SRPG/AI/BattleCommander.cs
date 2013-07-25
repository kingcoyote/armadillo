using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using SRPG.Data;
using Torch;

namespace SRPG.AI
{
    class BattleCommander
    {
        public BattleBoard BattleBoard;

        public Point CalculateDestination(Combatant character)
        {
            var grid = character.GetMovementGrid(BattleBoard.GetAccessibleGrid(character.Faction));

            var destination = TorchHelper.Vector2ToPoint(character.Avatar.Location);
            var best = CalculateCellWeight(character, (int)character.Avatar.Location.X, (int)character.Avatar.Location.Y);

            for (var x = 0; x < grid.Size.Width; x++)
            {
                for (var y = 0; y < grid.Size.Height; y++)
                {
                    if (grid.Weight[x, y] < 1) continue;

                    var currCell = new Point(x, y);

                    if (BattleBoard.GetCharacterAt(currCell) != null) continue;

                    var score = CalculateCellWeight(character, currCell.X, currCell.Y);

                    if (score <= best) continue;

                    best = score;
                    destination = new Point(x, y);
                }
            }

            return destination;
        }

        public Command CalculateAction(Combatant currChar, Point destination)
        {
            throw new NotImplementedException();
        }

        private int CalculateCellWeight(Combatant character, int cellX, int cellY)
        {
            var score = 0;

            // for each enemy the character can attack from here, add 3
            score += 3 * CalculateAttackableEnemies(character, cellX, cellY).Count;

            // calculate max damage that could be received
            score += (CalculateMaxDamageReceived(cellX, cellY) / character.CurrentHealth) * 3;

            // calculate the max damage possible in a single move
            score += (CalculateMaxDamageInflicted(character, cellX, cellY))/(character.MaxHealth/6);

            return score;
        }

        private List<Combatant> CalculateAttackableEnemies(Combatant character, int cellX, int cellY)
        {
            var enemies = new List<Combatant>();

            var attackGrid = character.GetEquippedWeapon().TargetGrid;

            for (var x = 0; x < attackGrid.Size.Width; x++)
            {
                for (var y = 0; y < attackGrid.Size.Height; y++)
                {
                    if (attackGrid.Weight[x, y] < 1) continue;

                    var c = BattleBoard.GetCharacterAt(
                        new Point(
                            cellX - attackGrid.Size.Width / 2 + x,
                            cellY - attackGrid.Size.Height / 2 + y
                        )
                    );

                    if (c != null && c.Faction == 0)
                    {
                        enemies.Add(c);
                    }
                }
            }

            return enemies;
        }

        private int CalculateMaxDamageReceived(int cellX, int cellY)
        {
            return (from t in BattleBoard.Characters where t.Faction == 0 select t)
                .Aggregate(0, (current, c) => current + CalculateMaxDamage(c, cellX, cellY));
        }

        private int CalculateMaxDamageInflicted(Combatant character, int cellX, int cellY)
        {
            return 0;
        }

        private int CalculateMaxDamage(Combatant character, int targetX, int targetY)
        {
            var best = 0;

            var attackAbility = Ability.Factory("attack");
            attackAbility.Character = character;

            var hits = attackAbility.GenerateHits(
                BattleBoard, new Point(targetX, targetY)
            ).Where(h => h.Target.X == targetX && h.Target.Y == targetY).ToArray();

            if (hits.Any())
            {
                best += hits.Sum(hit => hit.Damage);
            }



            return best;
        }
    }
}

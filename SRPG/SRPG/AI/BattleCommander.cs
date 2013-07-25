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

        public Point CalculateDestination(Combatant currChar)
        {
            var grid = currChar.GetMovementGrid(BattleBoard.GetAccessibleGrid(currChar.Faction));

            var destination = TorchHelper.Vector2ToPoint(currChar.Avatar.Location);
            var best = CalculateCellWeight(currChar, (int)currChar.Avatar.Location.X, (int)currChar.Avatar.Location.Y);

            for (var x = 0; x < grid.Size.Width; x++)
            {
                for (var y = 0; y < grid.Size.Height; y++)
                {
                    if (grid.Weight[x, y] < 1) continue;

                    var currCell = new Point(
                        x + (int)currChar.Avatar.Location.X - grid.Size.Width / 2,
                        y + (int)currChar.Avatar.Location.Y - grid.Size.Height / 2
                    );

                    if (BattleBoard.GetCharacterAt(currCell) != null) continue;

                    var score = CalculateCellWeight(currChar, currCell.X, currCell.Y);

                    if (score <= best) continue;

                    best = score;
                    destination = new Point(
                        x + (int)currChar.Avatar.Location.X - grid.Size.Width / 2,
                        y + (int)currChar.Avatar.Location.Y - grid.Size.Height / 2
                    );
                }
            }

            return destination;
        }

        private int CalculateCellWeight(Combatant character, int cellX, int cellY)
        {
            var score = 0;

            // for each enemy the character can attack from here, add 3
            score += 3 * CalculateAttackableEnemies(character, cellX, cellY).Count;

            // for each enemy that can attack the character from here, subtract 1
            score += (CalculatePossibleDamage(cellX, cellY) / character.CurrentHealth) * 3;

            // for each character that can be splashed in a single attack, add 1

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

        private int CalculatePossibleDamage(int cellX, int cellY)
        {
            return (from t in BattleBoard.Characters where t.Faction == 0 select t)
                .Aggregate(0, (current, c) => current + CalculateMaxDamage(c, cellX, cellY));
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

        public Command CalculateAction(Combatant currChar, Point destination)
        {
            throw new NotImplementedException();
        }
    }
}

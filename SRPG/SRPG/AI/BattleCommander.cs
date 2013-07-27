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
            var origLocation = character.Avatar.Location;
            
            var grid = character.GetMovementGrid(BattleBoard.GetAccessibleGrid(character.Faction));

            var destination = TorchHelper.Vector2ToPoint(character.Avatar.Location);
            var best = CalculateCellWeight(character, (int)character.Avatar.Location.X, (int)character.Avatar.Location.Y);

            foreach(var currCell in grid.GetPointList())
            {
                if (grid.Weight[currCell.X, currCell.Y] < 1) continue;

                if (BattleBoard.GetCharacterAt(currCell) != null) continue;

                character.Avatar.Location.X = currCell.X;
                character.Avatar.Location.Y = currCell.Y;

                var score = CalculateCellWeight(character, currCell.X, currCell.Y);

                if (score <= best) continue;

                best = score;
                destination = currCell;
            }

            character.Avatar.Location = origLocation;

            return destination;
        }

        public Command CalculateAction(Combatant currChar, Point destination)
        {
            throw new NotImplementedException();
        }

        private int CalculateCellWeight(Combatant character, int cellX, int cellY)
        {
            // movement algorithm:
            // d = maximum damage that can be inflicted in a single move
            // r = maximum damage received in a single round
            // h = current health
            // m = max health
            // 3e + 3d/h - r/6m

            var d = CalculateMaxDamageInflicted(character, cellX, cellY);
            //var r = CalculateMaxDamageReceived(cellX, cellY);
            //var h = character.CurrentHealth;
            //var m = character.MaxHealth;

            return d;
        }

        private int CalculateMaxDamageReceived(int cellX, int cellY)
        {
            return (from t in BattleBoard.Characters where t.Faction == 0 select t)
                .Aggregate(0, (current, c) => current + CalculateMaxDamage(c, cellX, cellY));
        }

        private int CalculateMaxDamageInflicted(Combatant character, int cellX, int cellY)
        {
            var best = 0;
            
            var validAbilities = new List<Ability>();

            validAbilities.Add(Ability.Factory("attack"));
            validAbilities[0].Character = character;

            //validAbilities.AddRange(character.GetAbilities().Where(character.CanUseAbility).Where(a => a.AbilityType == AbilityType.Active));

            // foreach ability
            foreach(var ability in validAbilities)
            {
                // foreach valid target for that ability
                var targetGrid = ability.GenerateTargetGrid();

                foreach(var point in targetGrid.GetPointList())
                {
                    if (targetGrid.Weight[point.X, point.Y] < 1) continue;

                    var hits = ability.GenerateHits(BattleBoard, new Point(cellX + point.X - 12, cellY + point.Y - 12));
                    var score = 0;

                    foreach(var hit in hits)
                    {
                        var targetCharacter = BattleBoard.GetCharacterAt(hit.Target);

                        if (targetCharacter == null) continue;

                        score += targetCharacter.ProcessHit(hit).Damage;
                    }

                    // if hits is greater than best
                    if (score > best)
                    {
                        best = score;
                    }
                }
            }

            // return best

            return best;
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

            foreach (var specialAbility in character.GetAbilities().Where(character.CanUseAbility).Where(a => a.AbilityType == AbilityType.Active))
            {
                var score = 0;

                hits = specialAbility.GenerateHits(
                    BattleBoard, new Point(targetX, targetY)
                ).Where(h => h.Target.X == targetX && h.Target.Y == targetY).ToArray();

                if (hits.Any())
                {
                    score += hits.Sum(hit => hit.Damage);
                }

                if(score > best)
                {
                    best = score;
                }
            }

            return best;
        }
    }
}

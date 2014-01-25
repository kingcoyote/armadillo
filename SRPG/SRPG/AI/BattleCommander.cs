using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using SRPG.Data;

namespace SRPG.AI
{
    class BattleCommander
    {
        public BattleBoard BattleBoard;

        private Game _game;
        private readonly Dictionary<Combatant, int> _threat = new Dictionary<Combatant, int>(); 

        public BattleCommander(Game game)
        {
            _game = game;
        }

        public BattleDecision CalculateAction(Combatant character)
        {
            var decision = new BattleDecision();
            decision.Destination = new Point((int)character.Avatar.Location.X, (int)character.Avatar.Location.Y);

            if (_threat.Count == 0) return decision;

            // find the enemy with the highest threat
            var enemy = (from ckv in _threat orderby ckv.Value descending select ckv.Key).First();
            
            // find the point that brings you closest to them
            var grid = character.GetMovementGrid(BattleBoard.GetAccessibleGrid(character.Faction));
            
            var distance = 65535;
            for (var x = 0; x < grid.Size.Width; x++)
            {
                for (var y = 0; y < grid.Size.Height; y++)
                {
                    if (grid.Weight[x, y] != 1) continue;

                    var d = BattleBoard.Sandbag.Pathfind(
                        new Point(x, y), 
                        new Point((int)enemy.Avatar.Location.X, (int)enemy.Avatar.Location.Y)
                    ).Count();

                    if (d >= distance || d > 2) continue;

                    decision.Destination = new Point(x, y);
                    distance = d;
                }
            }

            // attack
            decision.Command = new Command
                {
                    Ability = Ability.Factory(_game, "attack"),
                    Character = character,
                    Target = new Point((int)enemy.Avatar.Location.X, (int)enemy.Avatar.Location.Y)
                };

            return decision;
        }

        public void RecordCommand(Command command, List<Hit> hits)
        {
            var enemy = command.Character;
            if (!_threat.ContainsKey(enemy))
            {
                _threat.Add(enemy, 0);
            }

            foreach (var hit in hits)
            {
                var target = BattleBoard.GetCharacterAt(hit.Target);
                var damage = hit.Damage;
                
                // if nobody is at this spot, ignore it. this shouldn't come up
                // since a hit is only generated it someone is there, but i'll
                // account for it anyway
                if (target == null) continue;

                // hits against the players team (heal/friendly fire) are inverted
                // to maintain sanity. healing raises threat, friendly fire lowers it
                if (target.Faction == 0) damage = 0 - damage;

                _threat[enemy] += damage;
            }
        }
    }

    struct BattleDecision
    {
        public Point Destination;
        public Command Command;
    }
}

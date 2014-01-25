using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SRPG.Data;

namespace SRPG.AI
{
    class BattleCommander
    {
        public BattleBoard BattleBoard;

        private Dictionary<Combatant, int> _threat = new Dictionary<Combatant, int>(); 

        public BattleDecision CalculateAction(Combatant character)
        {
            throw new NotImplementedException();
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

using System;
using Microsoft.Xna.Framework;
using SRPG.Data;

namespace SRPG.AI
{
    class BattleCommander
    {
        public BattleBoard BattleBoard;

        public BattleDecision CalculateAction(Combatant character)
        {
            throw new NotImplementedException();
        }
    }

    struct BattleDecision
    {
        public Point Destination;
        public Command Command;
    }
}

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

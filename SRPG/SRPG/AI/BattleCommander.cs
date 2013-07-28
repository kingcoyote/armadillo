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
            throw new NotImplementedException();
        }

        public Command CalculateAction(Combatant currChar, Point destination)
        {
            throw new NotImplementedException();
        }
    }
}

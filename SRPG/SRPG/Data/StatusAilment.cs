using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SRPG.Data
{
    class StatusAilment
    {
        /// <summary>
        /// The character that this status ailment is afflicting.
        /// </summary>
        private Character _character;

        /// <summary>
        /// Process status ailment effects that happen at the start of the round, such as sleep or stun
        /// stripping movement points.
        /// </summary>
        public void BeginRound()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Process status ailment effects that happen at the end of the round, such as poison causing damage.
        /// </summary>
        public void EndRound()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Process a hit received by the character. This would remove sleep.
        /// </summary>
        public void ProcessHit()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Check whether or not a status ailment can be removed from a character.
        /// </summary>
        public void CheckRemoval()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Remove a status ailment from a character. I'm not entirely sure what this does.
        /// </summary>
        public void Remove()
        {
            throw new NotImplementedException();
        }
    }
}

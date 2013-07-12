
using Microsoft.Xna.Framework;
using Torch;

namespace SRPG.Data
{
    public struct Hit
    {
        /// <summary>
        /// Number of milliseconds that this hit is delayed from the start of the attack. This is used for multi-hit attacks
        /// such as Drill, or for slight delay between targets on Cleave or Chain Lightning.
        /// </summary>
        public int Delay;
        /// <summary>
        /// Base damage that this attack will inflict. This is before any armor or passive abilities are involved.
        /// </summary>
        public int Damage;
        /// <summary>
        /// Base odds that a critical strike is inflicted, with a value of 0-100.
        /// </summary>
        public int Critical;
        /// <summary>
        /// Any status ailments that this hit will inflict if it is successful.
        /// </summary>
        public StatusAilmentType StatusAilment;
        /// <summary>
        /// A small sprite animation of what to show at the target when the hit is received.
        /// </summary>
        public SpriteObject Animation;

        public Point Target;
    }
}

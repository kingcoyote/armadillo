using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SRPG.Abilities;
using Torch;

namespace SRPG.Data
{
    abstract public class Ability
    {
        public string Name;
        public int ManaCost;
        /// <summary>
        /// The character who this ability is tied to. This is used to generate the hit, especially base damage.
        /// </summary>
        public Combatant Character;
        /// <summary>
        /// Indicating whether this is a passive or active ability.
        /// </summary>
        public AbilityType AbilityType;
        /// <summary>
        /// Indicating what type of item this ability is tied to, to ensure it can only be used if the character is
        /// equipped with the right item.
        /// </summary>
        public ItemType ItemType;
        /// <summary>
        /// An indication of who this ability can target - friend or foe.
        /// </summary>
        public AbilityTarget AbilityTarget;

        public SpriteObject Icon;

        /// <summary>
        /// Generate one or more hits to be used to damage the targets.
        /// </summary>
        /// <returns>A non-zero list of hits to be applied to a target.</returns>
        public virtual List<Hit> GenerateHits(BattleBoard board, Point target)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Generate a 25x25 grid showing what squares are targetable by this ability. Square 12,12 is the orientation square, where
        /// the character is located, and it is always oriented up. The caller is responsible for proper orientation. A value of 1
        /// indicates that this square can be targetted, 0 indicates it cannot be.
        /// </summary>
        /// <returns>A grid indicating where this ability can be cast.</returns>
        public Grid GenerateTargetGrid()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Generate a 25x25 grid showing what squares are impacted by this ability. The value is a weight corresponding to how much each
        /// square is impacted (damaged or healed). Square 12,12 is the orientation square, where the ability is targeted, and it is always oriented
        /// up. The caller is responsible for proper orientation.
        /// </summary>
        /// <returns>A grid indicating the impact zone of an ability.</returns>
        public virtual Grid GenerateImpactGrid()
        {
            throw new NotImplementedException();
        }

        public static Ability Factory(string name)
        {
            // todo : there has to be a less tedious way to do this...

            switch(name)
            {
                case "lunge": return new Lunge();
                case "cleave": return new Cleave();
                // todo : 3rd sword ability

                case "headshot": return new Headshot();
                case "drill": return new Drill();
                // todo : 3rd gun ability

                case "healing": return new Healing();
                case "protect": return new Protect();
                case "revive": return new Revive();

                case "fire": return new Fire();
                case "lightning": return new Lightning();
                case "quake": return new Quake();

                case "cobra punch": return new CobraPunch();
                case "flying knee": return new FlyingKnee();
                case "whip kick": return new WhipKick();

                case "target": return new Target();
                case "focus": return new Focus();
                case "serenity": return new Serenity();

                case "sprint": return new Sprint();
                case "untouchable": return new Untouchable();
                case "blur": return new Blur();

                case "awareness": return new Awareness();
                case "vengeance": return new Vengeance();

                case "deflection": return new Deflection();
                case "steel wall": return new SteelWall();

                case "move": return new Move();
                case "attack": return new Attack();

                default: throw new Exception(string.Format("Unknown ability {0}", name));

            }
        }

        public override bool Equals(object a)
        {
            if (!(a is Ability)) return false;

            return ((Ability) a).Name == Name;
        }

        protected void SetIcon(string iconName, int i)
        {
            Icon = new SpriteObject("Abilities/" + iconName) {Height = 50, Width = 50};
            Icon.AddAnimation(Name,
                              new SpriteAnimation
                                  {
                                      StartRow = 0,
                                      StartCol = i * 50,
                                      FrameCount = 1,
                                      FrameRate = 1,
                                      Size = new Rectangle(0, 0, 50, 50)
                                  });
            Icon.SetAnimation(Name);
        }
    }
}

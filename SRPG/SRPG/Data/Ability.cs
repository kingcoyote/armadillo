using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SRPG.Abilities;

namespace SRPG.Data
{
    abstract public class Ability
    {
        public string Name;
        public int ManaCost;
        /// <summary>
        /// The character who this ability is tied to. This is used to generate the hit, especially base damage.
        /// </summary>
        public Character Character;
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

        /// <summary>
        /// Generate one or more hits to be used to damage the targets.
        /// </summary>
        /// <returns>A non-zero list of hits to be applied to a target.</returns>
        public List<Hit> GenerateHits()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Generate a 25x25 grid showing what squares are targetable by this ability. Square 12,12 is the orientation square, where
        /// the character is located, and it is always oriented up. The caller is responsible for proper orientation. A value of 1
        /// indicates that this square can be targetted, 0 indicates it cannot be.
        /// </summary>
        /// <returns>A grid indicating where this ability can be cast.</returns>
        public Grid GenerateTargetGrid(BattleBoard battleboard)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Generate a 25x25 grid showing what squares are impacted by this ability. The value is a weight corresponding to how much each
        /// square is impacted (damaged or healed). Square 12,12 is the orientation square, where the ability is targeted, and it is always oriented
        /// up. The caller is responsible for proper orientation.
        /// </summary>
        /// <returns>A grid indicating the impact zone of an ability.</returns>
        public Grid GenerateImpactGrid(BattleBoard battleboard)
        {
            throw new NotImplementedException();
        }

        public static Ability Factory(string name)
        {
            switch(name)
            {
                case "lunge":
                    return new Lunge();
                case "cleave":
                    return new Cleave();
            }

            throw new Exception("unknown ability");
        }
    }
}

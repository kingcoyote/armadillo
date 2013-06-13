using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace SRPG.Data
{
    public class Item
    {
        /// <summary>
        /// Human readable name of this item.
        /// </summary>
        public string Name;
        /// <summary>
        /// A small picture showing an icon for this item.
        /// </summary>
        public Texture2D Picture;
        /// <summary>
        /// A dictionary indicating what stats are boosted by this item, and by how much.
        /// </summary>
        public Dictionary<Stat, int> StatBoosts;
        /// <summary>
        /// A dictionary indicationg what stats are given xp multipliers by this item, and by how much.
        /// </summary>
        public Dictionary<Stat, int> StatMultipliers;
        /// <summary>
        /// An optional ability tied to this item - passive for armor and active for weapons.
        /// </summary>
        public Ability Ability;
        /// <summary>
        /// A value indicating how much this is worth when buying / selling.
        /// </summary>
        public int Cost;
        /// <summary>
        /// An ItemType value indicating what type of item this is, to ensure that items are only equipped when valid.
        /// </summary>
        public ItemType ItemType;

        public ItemEquipType GetEquipType()
        {
            switch(ItemType)
            {
                case ItemType.Sword:
                case ItemType.Gun:
                case ItemType.Book:
                case ItemType.Staff:
                case ItemType.Unarmed:
                    return ItemEquipType.Weapon;

                case ItemType.Cloth:
                case ItemType.Leather:
                case ItemType.Mail:
                case ItemType.Plate:
                    return ItemEquipType.Armor;

                case ItemType.Accessory:
                    return ItemEquipType.Accessory;

                default:
                    return ItemEquipType.None;
            }
        }
    }
}

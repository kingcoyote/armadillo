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

        private static Dictionary<string, Item> _itemList;

        public static void InitializeItemList()
        {
            _itemList = new Dictionary<string, Item>
                {
                    {"Shortsword", new Item { Name = "Shortsword", ItemType = ItemType.Sword }}, 
                    {"Longsword", new Item { Name = "Longsword", ItemType = ItemType.Sword }}, 
                    {"Greatsword", new Item { Name = "Greatsword", ItemType = ItemType.Sword }}, 
                    {"Pistol", new Item { Name = "Pistol", ItemType = ItemType.Gun }}, 
                    {"Revolver", new Item { Name = "Revolver", ItemType = ItemType.Gun }}, 
                    {"Big Iron", new Item { Name = "Big Iron", ItemType = ItemType.Gun }}, 
                    {"Scroll", new Item { Name = "Scroll", ItemType = ItemType.Book }}, 
                    {"Holy Book", new Item { Name = "Holy Book", ItemType = ItemType.Book }}, 
                    {"Anthology", new Item { Name = "Anthology", ItemType = ItemType.Book }}, 
                    {"Fire Wand", new Item { Name = "Fire Wand", ItemType = ItemType.Staff }}, 
                    {"Spark Staff", new Item { Name = "Spark Staff", ItemType = ItemType.Staff }}, 
                    {"Ground Staff", new Item { Name = "Ground Staff", ItemType = ItemType.Staff }}, 
                    {"Brass Knuckles", new Item { Name = "Brass Knuckles", ItemType = ItemType.Unarmed }}, 
                    {"Tiger Claws", new Item { Name = "Tiger Claws", ItemType = ItemType.Unarmed }}, 
                    {"Old Shoes", new Item { Name = "Old Shoes", ItemType = ItemType.Unarmed }}, 

                    {"Vest", new Item { Name = "Vest", ItemType = ItemType.Cloth }}, 
                    {"Robe", new Item { Name = "Robe", ItemType = ItemType.Cloth }}, 
                    {"Kimono", new Item { Name = "Kimono", ItemType = ItemType.Cloth }}, 
                    {"Jerkin", new Item { Name = "Jerkin", ItemType = ItemType.Leather }}, 
                    {"Studded Leather", new Item { Name = "Studded Leather", ItemType = ItemType.Leather }}, 
                    {"Brigandine", new Item { Name = "Brigandine", ItemType = ItemType.Leather }}, 
                    {"Hauberk", new Item { Name = "Hauberk", ItemType = ItemType.Mail }}, 
                    {"Scale Mail", new Item { Name = "Scale Mail", ItemType = ItemType.Mail }}, 
                    {"Dragonskin", new Item { Name = "Dragonskin", ItemType = ItemType.Mail }}, 
                    {"Breastplate", new Item { Name = "Breastplate", ItemType = ItemType.Plate }}, 
                    {"Full Plate", new Item { Name = "Full Plate", ItemType = ItemType.Plate }}, 
                    {"Gothic Plate", new Item { Name = "Gothic Plate", ItemType = ItemType.Plate }}, 
                };
        }

        public static Item Factory(string name)
        {
            return _itemList[name];
        }
    }
}

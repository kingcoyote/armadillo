using System;
using System.Collections.Generic;
using System.IO;
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
        public Dictionary<Stat, int> StatBoosts = new Dictionary<Stat, int>()
            {
                {Stat.Health, 0},
                {Stat.Mana, 0},
                {Stat.Defense, 0},
                {Stat.Attack, 0},
                {Stat.Wisdom, 0},
                {Stat.Intelligence, 0},
                {Stat.Speed, 0},
                {Stat.Hit, 0},
            };
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

        /// <summary>
        /// A 25x25 grid indicating where this item can target. This generally applies only to weapons and consumables.
        /// 12,12 is the center and is oriented on the character using the item. The orientation is always up and it
        /// is the caller's responsibility to adjust orientation.
        /// </summary>
        public Grid TargetGrid;

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

        private readonly static Dictionary<string, Item> _itemList = new Dictionary<string, Item>();

        public static Item Factory(string name)
        {
            if (_itemList.ContainsKey(name)) return _itemList[name];

            var item = new Item();

            var itemType = name.Split('/')[0];
            var itemName = name.Split('/')[1];

            string settingString = String.Join("\r\n", File.ReadAllLines("Content/Items/" + itemType + ".js"));

            var nodeList = Newtonsoft.Json.Linq.JObject.Parse(settingString);

            item.Name = nodeList[itemName]["name"].ToString();
            item.ItemType = StringToItemType(nodeList[itemName]["itemType"][0].ToString());
            item.Cost = (int)(nodeList[itemName]["cost"]);

            item.TargetGrid = nodeList[itemName].SelectToken("targetGrid") != null ? Grid.FromBitmap("Items/" + nodeList[itemName]["targetGrid"].ToString()) : new Grid(25, 25);

            if (nodeList[itemName].SelectToken("statBoosts") != null) foreach (var node in nodeList[itemName]["statBoosts"])
            {
                item.StatBoosts[StringToStat(node["stat"].ToString())] = Convert.ToUInt16(node["amount"].ToString());
            }

            if(nodeList[itemName].SelectToken("ability") != null)
            {
                item.Ability = Ability.Factory(nodeList[itemName].SelectToken("ability").ToString());
            }

            _itemList.Add(name, item);

            return item;
        }

        private static Stat StringToStat(string name)
        {
            switch(name)
            {
                case "health": return Stat.Health;
                case "mana": return Stat.Mana;
                case "defense": return Stat.Defense;
                case "attack": return Stat.Attack;
                case "wisdom": return Stat.Wisdom;
                case "intelligence": return Stat.Intelligence;
                case "speed": return Stat.Speed;
                case "hit": return Stat.Hit;
            }

            throw new Exception("item has invalid stat boost");
        }

        public static ItemType StringToItemType(string name)
        {
            switch (name)
            {
                case "sword": return ItemType.Sword;
                case "gun": return ItemType.Gun;
                case "book": return ItemType.Book;
                case "staff": return ItemType.Staff;
                case "unarmed": return ItemType.Unarmed;
                case "cloth": return ItemType.Cloth;
                case "leather": return ItemType.Leather;
                case "mail": return ItemType.Mail;
                case "plate": return ItemType.Plate;
                case "accessory": return ItemType.Accessory;
                case "consumable": return ItemType.Consumable;
            }

            throw new Exception("item does not have a valid item type");
        }
    }
}

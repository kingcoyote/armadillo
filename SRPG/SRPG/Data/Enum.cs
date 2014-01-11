using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SRPG.Data
{
    public enum Stat
    {
        Health,
        Mana,
        Defense,
        Attack,
        Wisdom,
        Intelligence,
        Speed,
        Hit
    }

    [Flags]
    public enum ItemType
    {
        Sword = 1,
        Hammer = 2,
        Gun = 4,
        Book = 8,
        Staff = 16,
        Unarmed = 32,

        Cloth = 64,
        Leather = 128,
        Mail = 256,
        Plate = 512,
        
        Accessory = 1024,
        
        Consumable = 2048
    }

    public enum ItemEquipType
    {
        None,
        Weapon,
        Armor,
        Accessory
    }

    public enum AbilityType
    {
        Passive,
        Active
    }

    public enum AbilityTarget
    {
        Friendly,
        Enemy,
        Unoccupied,
        Any
    }

    public enum StatusAilmentType
    {
        Sleep,
        Stun,
        Blind,
        Poison
    }

    public enum Direction
    {
        None = 0,
        Up = 1,
        Right = 2,
        Down = 4,
        Left = 8
    }
}

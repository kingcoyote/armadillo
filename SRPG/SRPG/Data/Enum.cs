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

    public enum ItemType
    {
        Sword,
        Hammer,
        Gun,
        Book,
        Staff,
        Unarmed,

        Cloth,
        Leather,
        Mail,
        Plate,
        
        Accessory,
        
        Consumable
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
        Unoccupied
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

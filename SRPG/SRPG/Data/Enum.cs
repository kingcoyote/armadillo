using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SRPG.Data
{
    [Flags]
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
        Sword,
        Hammer,
        Bow,
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

    [Flags]
    public enum AbilityType
    {
        Passive,
        Active
    }

    [Flags]
    public enum AbilityTarget
    {
        Friendly,
        Enemy
    }

    [Flags]
    public enum StatusAilmentType
    {
        Sleep,
        Stun,
        Blind,
        Poison
    }

    [Flags]
    public enum Direction
    {
        None = 0,
        Up = 1,
        Right = 2,
        Down = 4,
        Left = 8
    }
}

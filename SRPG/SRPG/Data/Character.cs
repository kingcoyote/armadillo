using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Torch;

namespace SRPG.Data
{
    public class Character
    {
        /// <summary>
        /// Printable name for the character.
        /// </summary>
        public string Name;
        /// <summary>
        /// Character's current maximum health pool.
        /// </summary>
        public int MaxHealth;
        /// <summary>
        /// Character's current maximum mana pool.
        /// </summary>
        public int MaxMana;
        /// <summary>
        /// Character's currently available health pool. The character will die if this reaches 0.
        /// </summary>
        public int CurrentHealth;
        /// <summary>
        /// Character's currently available mana pool. Each time an active ability is used, this is reduced.
        /// </summary>
        public int CurrentMana;
        /// <summary>
        /// DAWISH stat list for this character. This list is the unmodified list, and thus ignores status ailments,
        /// buffs and debuffs
        /// </summary>
        public Dictionary<Stat, int> Stats = new Dictionary<Stat, int>()
            {
                {Stat.Defense, 0},
                {Stat.Attack, 0},
                {Stat.Wisdom, 0},
                {Stat.Intelligence, 0},
                {Stat.Speed, 0},
                {Stat.Hit, 0}
            };
        /// <summary>
        /// Temporary modifications to a character's DAWISH stats, such as from passive abilities, buffs, debuffs
        /// or status ailments such as blind.
        /// </summary>
        public Dictionary<Stat, int> BonusStats;

        /// <summary>
        /// List of items currently equipped by the character, including weapon, armor and accessory.
        /// </summary>
        public List<Item> Inventory = new List<Item>() {};
        /// <summary>
        /// List of abilities the character can perform. This includes active, passive and attack.
        /// </summary>
        public List<Ability> Abilities = new List<Ability>(){};
        /// <summary>
        /// A small portrait to be shown when this character is highlighted.
        /// </summary>
        public Texture2D Portrait;
        /// <summary>
        /// The SpriteObject that acts as a visual representation of this character on the battlefield.
        /// </summary>
        public SpriteObject Sprite;
        /// <summary>
        /// An indicator of the character's class, which will determine weapon / armor usability.
        /// </summary>
        public CharacterClass Class;
        /// <summary>
        /// An X,Y pair indicating where on the current battlefield this character is located. Outside of a battle scene,
        /// this is not applicable and can be ignored.
        /// </summary>
        public Vector2 Location;
        /// <summary>
        /// An integer indicating what faction this character belongs to - 0 represents the player and 1 represents the enemy.
        /// This is used to determine targeting, AI control, player control, and character movement grids.
        /// </summary>
        public int Faction;
        /// <summary>
        /// A boolean value indicating whether or not the character is able to move in the current turn. This is set to true at the start
        /// of a round, and false upon moving. Status ailments such as sleep and stun can set this to false during BeginRound.
        /// </summary>
        public bool CanMove;
        /// <summary>
        /// A boolean value indicating whether or not the character is able to act in the current turn. An action is defined as using an
        /// item, attacking, using an active ability, or switching weapons. This is set to true at the start of the round, but can
        /// be set to false due to sleep or stun.
        /// </summary>
        public bool CanAct;

        public Direction Direction;

        public Vector2 Velocity;

        public Dictionary<Stat, int> StatExperienceLevels;
        public Dictionary<Ability, int> AbilityExperienceLevels;

        public int FeetWidth = 0;
        public int FeetHeight = 0;

        
        /// <summary>
        /// Process any status ailments that would impact a character at the start of the round,
        /// such as sleep or stun.
        /// </summary>
        public void BeginRound()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Process any status ailments that would impact a character at the end of the round, such as poison.
        /// Also check to see if any status ailments naturally were removed, or if any passive abilities can
        /// be processed, such as focus.
        /// </summary>
        public void EndRound()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Receive a hit and process it accordingly. This processing will decrease incoming damage based on defense,
        /// process passive abilities, decrease character health and other related processing.
        /// </summary>
        /// <param name="hit">A hit generated by a call to Ability.GenerateHits().</param>
        public void ProcessHit(Hit hit)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Indicate whether or not the character is being threatened. Being threatened is defined as being within attack
        /// range of any regular attacks. Regular attacks do not include active or passive abilities and are strictly limited
        /// to weapon attacks.
        /// </summary>
        /// <param name="board">The game board for the current battle. This will be asked about enemy positioning.</param>
        /// <returns></returns>
        public bool IsThreatened(BattleBoard board)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Raise the character's experience levels, in accordance with the stat multipliers granted.
        /// </summary>
        /// <param name="experience">amount of experience points gained</param>
        public void GainExperience(int experience)
        {
            throw new NotImplementedException();
        }

        public void Die()
        {
            
        }

        public int ReadStat(Stat stat)
        {
            var number = Stats[stat];

            if (GetEquippedWeapon() != null) number += GetEquippedWeapon().StatBoosts[stat];
            if (GetEquippedArmor() != null) number += GetEquippedArmor().StatBoosts[stat];
            if (GetEquippedAccessory() != null) number += GetEquippedAccessory().StatBoosts[stat];

            return number;
        }

        public int GenerateExperience()
        {
            throw new NotImplementedException();
        }

        public Item GetEquippedWeapon()
        {
            return (from item in Inventory where item.ItemType == Class.WeaponTypes select item).FirstOrDefault();
        }

        public Item GetEquippedArmor()
        {
            return (from item in Inventory where item.ItemType == Class.ArmorTypes select item).FirstOrDefault();
        }

        public Item GetEquippedAccessory()
        {
            return (from item in Inventory where item.ItemType == ItemType.Accessory select item).FirstOrDefault();
        }

        public Item EquipItem(Item item)
        {
            var oldItem = new Item();
            
            switch(item.GetEquipType())
            {
                case ItemEquipType.Weapon:
                    oldItem = GetEquippedWeapon();
                    break;
                case ItemEquipType.Armor:
                    oldItem = GetEquippedArmor();
                    break;
                case ItemEquipType.Accessory:
                    throw new NotImplementedException();
                    break;

                default:
                    throw new Exception("unable to equip this item");
            }

            if (oldItem != null) Inventory.Remove(oldItem);
            Inventory.Add(item);

            return oldItem;
        }

        public void UpdateAnimation()
        {
            // if they are currently moving
            if (Math.Abs(Velocity.X) > 0 || Math.Abs(Velocity.Y) > 0)
            {
                // find out what directions they are actually moving...
                var actualDir = ParseActualDirection(Velocity.X, Velocity.Y);
                // ... and what direction the animation is facing
                var currentDir = StringToDirection(Sprite.GetAnimation().Split(' ')[1]);

                Direction = actualDir[0];

                // if they aren't facing a valid direction, correct it
                if (!actualDir.Contains(currentDir))
                {
                    Sprite.SetAnimation(String.Format("walking {0}", actualDir[0].ToString().ToLower()));
                }

                // if the animation is standing, change it to moving
                if (Sprite.GetAnimation().Split(' ')[0] == "standing")
                {
                    Sprite.SetAnimation(Sprite.GetAnimation().Replace("standing", "walking"));
                }
            }
            else
            {
                // make sure they are standing if they have no velocity
                Sprite.SetAnimation(Sprite.GetAnimation().Replace("walking", "standing"));
            }
        }

        private static List<Direction> ParseActualDirection(float x, float y)
        {
            var dirs = new List<Direction>();

            if (x > 0) dirs.Add(Direction.Right);
            if (x < 0) dirs.Add(Direction.Left);

            if (y > 0) dirs.Add(Direction.Down);
            if (y < 0) dirs.Add(Direction.Up);

            return dirs;
        }

        private static Direction StringToDirection(string str)
        {
            switch (str.ToLower())
            {
                case "up": return Direction.Up;
                case "down": return Direction.Down;
                case "left": return Direction.Left;
                case "right": return Direction.Right;
                default: throw new Exception();
            }
        }

        public Rectangle GetFeet()
        {
            return new Rectangle(
                (int)(Location.X + Sprite.Width / 2 - FeetWidth / 2),
                (int)(Location.Y + Sprite.Height - FeetHeight),
                FeetWidth,
                FeetHeight
            );
        }
    }
}

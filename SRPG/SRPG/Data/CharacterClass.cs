using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Torch;
using Game = Torch.Game;

namespace SRPG.Data
{
    public class CharacterClass
    {
        /// <summary>
        /// Human readable class name.
        /// </summary>
        public string Name;
        /// <summary>
        /// Flags indicating what armor types this class can equip.
        /// </summary>
        public readonly ItemType ArmorTypes;
        /// <summary>
        /// Flags indicating what weapon types this class can equip.
        /// </summary>
        public readonly ItemType WeaponTypes;

        public CharacterClass(string name, ItemType weaponTypes, ItemType armorTypes)
        {
            Name = name;
            ArmorTypes = armorTypes;
            WeaponTypes = weaponTypes;
        }

        /// <summary>
        /// Indicate if this class is able to equip the specified armor.
        /// </summary>
        /// <param name="armor">The armor that is being requested.</param>
        /// <returns>true if this class can equip this armor.</returns>
        public bool CanEquipArmor(Item armor)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Indicate if this class is able to equip the specified weapon.
        /// </summary>
        /// <param name="weapon">The weapon that is being requested.</param>
        /// <returns>true if this class can equip this weapon.</returns>
        public bool CanEquipWeapon(Item weapon)
        {
            throw new NotImplementedException();
        }

        public static Character GenerateCharacter(string className)
        {
            Character character;

            int spriteWidth;
            int spriteHeight;

            switch(className)
            {
                case "fighter":
                    spriteWidth = 65;
                    spriteHeight = 75;
                    character = new Character
                        {
                            Class = new CharacterClass(className, ItemType.Sword, ItemType.Plate),
                            Sprite = new SpriteObject(Game.GetInstance().Content.Load<Texture2D>("Characters/TempFighter")),
                            FeetWidth = 40,
                            FeetHeight = 25
                        };
                    character.Sprite.AddAnimation("walking down", new SpriteAnimation { FrameCount = 8, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = 1, FrameRate = 8 });
                    character.Sprite.AddAnimation("standing down", new SpriteAnimation { FrameCount = 1, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = 1, FrameRate = 1 });
                    character.Sprite.AddAnimation("walking up", new SpriteAnimation { FrameCount = 8, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = 76, FrameRate = 8 });
                    character.Sprite.AddAnimation("standing up", new SpriteAnimation { FrameCount = 1, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = 76, FrameRate = 1 });
                    character.Sprite.AddAnimation("walking left", new SpriteAnimation { FrameCount = 8, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = 151, FrameRate = 8 });
                    character.Sprite.AddAnimation("standing left", new SpriteAnimation { FrameCount = 1, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = 151, FrameRate = 1 });
                    character.Sprite.AddAnimation("walking right", new SpriteAnimation { FrameCount = 8, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = 226, FrameRate = 8 });
                    character.Sprite.AddAnimation("standing right", new SpriteAnimation { FrameCount = 1, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = 226, FrameRate = 1 });
                    break;
                case "ranger":
                    spriteWidth = 65;
                    spriteHeight = 75;
                    character = new Character
                    {
                        Class = new CharacterClass(className, ItemType.Gun, ItemType.Mail),
                        Sprite = new SpriteObject(Game.GetInstance().Content.Load<Texture2D>("Characters/TempRanger")),
                        FeetWidth = 40,
                        FeetHeight = 25
                    };
                    character.Sprite.AddAnimation("walking down", new SpriteAnimation { FrameCount = 8, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = 1, FrameRate = 8 });
                    character.Sprite.AddAnimation("standing down", new SpriteAnimation { FrameCount = 1, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = 1, FrameRate = 1 });
                    character.Sprite.AddAnimation("walking up", new SpriteAnimation { FrameCount = 8, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = 76, FrameRate = 8 });
                    character.Sprite.AddAnimation("standing up", new SpriteAnimation { FrameCount = 1, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = 76, FrameRate = 1 });
                    character.Sprite.AddAnimation("walking left", new SpriteAnimation { FrameCount = 8, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = 151, FrameRate = 8 });
                    character.Sprite.AddAnimation("standing left", new SpriteAnimation { FrameCount = 1, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = 151, FrameRate = 1 });
                    character.Sprite.AddAnimation("walking right", new SpriteAnimation { FrameCount = 8, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = 226, FrameRate = 8 });
                    character.Sprite.AddAnimation("standing right", new SpriteAnimation { FrameCount = 1, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = 226, FrameRate = 1 });
                    break;
                case "cleric":
                    spriteWidth = 65;
                    spriteHeight = 75;
                    character = new Character
                    {
                        Class = new CharacterClass(className, ItemType.Book, ItemType.Cloth),
                        Sprite = new SpriteObject(Game.GetInstance().Content.Load<Texture2D>("Characters/TempCleric")),
                        FeetWidth = 40,
                        FeetHeight = 25
                    };
                    character.Sprite.AddAnimation("walking down", new SpriteAnimation { FrameCount = 8, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = 1, FrameRate = 8 });
                    character.Sprite.AddAnimation("standing down", new SpriteAnimation { FrameCount = 1, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = 1, FrameRate = 1 });
                    character.Sprite.AddAnimation("walking up", new SpriteAnimation { FrameCount = 8, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = 76, FrameRate = 8 });
                    character.Sprite.AddAnimation("standing up", new SpriteAnimation { FrameCount = 1, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = 76, FrameRate = 1 });
                    character.Sprite.AddAnimation("walking left", new SpriteAnimation { FrameCount = 8, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = 151, FrameRate = 8 });
                    character.Sprite.AddAnimation("standing left", new SpriteAnimation { FrameCount = 1, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = 151, FrameRate = 1 });
                    character.Sprite.AddAnimation("walking right", new SpriteAnimation { FrameCount = 8, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = 226, FrameRate = 8 });
                    character.Sprite.AddAnimation("standing right", new SpriteAnimation { FrameCount = 1, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = 226, FrameRate = 1 });
                    break;
                case "wizard":
                    spriteWidth = 65;
                    spriteHeight = 75;
                    character = new Character
                    {
                        Class = new CharacterClass(className, ItemType.Staff, ItemType.Cloth),
                        Sprite = new SpriteObject(Game.GetInstance().Content.Load<Texture2D>("Characters/TempWizard")),
                        FeetWidth = 40,
                        FeetHeight = 25
                    };
                    character.Sprite.AddAnimation("walking down", new SpriteAnimation { FrameCount = 8, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = 1, FrameRate = 8 });
                    character.Sprite.AddAnimation("standing down", new SpriteAnimation { FrameCount = 1, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = 1, FrameRate = 1 });
                    character.Sprite.AddAnimation("walking up", new SpriteAnimation { FrameCount = 8, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = 76, FrameRate = 8 });
                    character.Sprite.AddAnimation("standing up", new SpriteAnimation { FrameCount = 1, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = 76, FrameRate = 1 });
                    character.Sprite.AddAnimation("walking left", new SpriteAnimation { FrameCount = 8, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = 151, FrameRate = 8 });
                    character.Sprite.AddAnimation("standing left", new SpriteAnimation { FrameCount = 1, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = 151, FrameRate = 1 });
                    character.Sprite.AddAnimation("walking right", new SpriteAnimation { FrameCount = 8, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = 226, FrameRate = 8 });
                    character.Sprite.AddAnimation("standing right", new SpriteAnimation { FrameCount = 1, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = 226, FrameRate = 1 });
                    break;
                case "monk":
                    spriteWidth = 65;
                    spriteHeight = 75;
                    character = new Character
                    {
                        Class = new CharacterClass(className, ItemType.Unarmed, ItemType.Leather),
                        Sprite = new SpriteObject(Game.GetInstance().Content.Load<Texture2D>("Characters/TempMonk")),
                        FeetWidth = 40,
                        FeetHeight = 25
                    };
                    character.Sprite.AddAnimation("walking down", new SpriteAnimation { FrameCount = 8, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = 1, FrameRate = 8 });
                    character.Sprite.AddAnimation("standing down", new SpriteAnimation { FrameCount = 1, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = 1, FrameRate = 1 });
                    character.Sprite.AddAnimation("walking up", new SpriteAnimation { FrameCount = 8, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = 76, FrameRate = 8 });
                    character.Sprite.AddAnimation("standing up", new SpriteAnimation { FrameCount = 1, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = 76, FrameRate = 1 });
                    character.Sprite.AddAnimation("walking left", new SpriteAnimation { FrameCount = 8, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = 151, FrameRate = 8 });
                    character.Sprite.AddAnimation("standing left", new SpriteAnimation { FrameCount = 1, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = 151, FrameRate = 1 });
                    character.Sprite.AddAnimation("walking right", new SpriteAnimation { FrameCount = 8, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = 226, FrameRate = 8 });
                    character.Sprite.AddAnimation("standing right", new SpriteAnimation { FrameCount = 1, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = 226, FrameRate = 1 });
                    break;
                case "enemy":
                    spriteWidth = 65;
                    spriteHeight = 75;
                    character = new Character
                    {
                        Class = new CharacterClass(className, ItemType.Sword, ItemType.Plate),
                        Sprite = new SpriteObject(Game.GetInstance().Content.Load<Texture2D>("Characters/TempEnemy")),
                        FeetWidth = 40,
                        FeetHeight = 25
                    };
                    character.Sprite.AddAnimation("walking down", new SpriteAnimation { FrameCount = 8, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = 1, FrameRate = 8 });
                    character.Sprite.AddAnimation("standing down", new SpriteAnimation { FrameCount = 1, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = 1, FrameRate = 1 });
                    character.Sprite.AddAnimation("walking up", new SpriteAnimation { FrameCount = 8, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = 76, FrameRate = 8 });
                    character.Sprite.AddAnimation("standing up", new SpriteAnimation { FrameCount = 1, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = 76, FrameRate = 1 });
                    character.Sprite.AddAnimation("walking left", new SpriteAnimation { FrameCount = 8, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = 151, FrameRate = 8 });
                    character.Sprite.AddAnimation("standing left", new SpriteAnimation { FrameCount = 1, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = 151, FrameRate = 1 });
                    character.Sprite.AddAnimation("walking right", new SpriteAnimation { FrameCount = 8, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = 226, FrameRate = 8 });
                    character.Sprite.AddAnimation("standing right", new SpriteAnimation { FrameCount = 1, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = 226, FrameRate = 1 });
                    break;
                case "link":
                    spriteWidth = 87;
                    spriteHeight = 72;
                    character = new Character
                        {
                            Class = new CharacterClass(className, ItemType.Sword, ItemType.Mail),
                            Sprite = new SpriteObject(Game.GetInstance().Content.Load<Texture2D>("Characters/link")),
                            FeetWidth = 40,
                            FeetHeight = 25
                        };
                    character.Sprite.AddAnimation("walking down", new SpriteAnimation { FrameCount = 8, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = 1, FrameRate = 8 });
                    character.Sprite.AddAnimation("standing down", new SpriteAnimation { FrameCount = 1, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = 1, FrameRate = 1 });
                    character.Sprite.AddAnimation("walking up", new SpriteAnimation { FrameCount = 8, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = spriteHeight, FrameRate = 8 });
                    character.Sprite.AddAnimation("standing up", new SpriteAnimation { FrameCount = 1, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = spriteHeight, FrameRate = 1 });
                    character.Sprite.AddAnimation("walking left", new SpriteAnimation { FrameCount = 8, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = spriteHeight*2, FrameRate = 8 });
                    character.Sprite.AddAnimation("standing left", new SpriteAnimation { FrameCount = 1, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = spriteHeight*2, FrameRate = 1 });
                    character.Sprite.AddAnimation("walking right", new SpriteAnimation { FrameCount = 8, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = spriteHeight*3, FrameRate = 8 });
                    character.Sprite.AddAnimation("standing right", new SpriteAnimation { FrameCount = 1, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = spriteHeight*3, FrameRate = 1 });
                    break;
                default:
                    throw new Exception(string.Format("unknown character class {0}", className));
            }

            return character;
        }
    }
}

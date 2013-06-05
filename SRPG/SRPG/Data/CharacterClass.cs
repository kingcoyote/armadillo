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
        public ItemType ArmorTypes;
        /// <summary>
        /// Flags indicating what weapon types this class can equip.
        /// </summary>
        public ItemType WeaponTypes;

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
            var character = new Character {Class = new CharacterClass {Name = className}};

            // todo : find a way to only write to ArmorTypes / WeaponTypes here, and readonly elsewhere

            switch(className)
            {
                case "fighter":
                    character.Class.ArmorTypes = ItemType.Mail | ItemType.Plate;
                    character.Class.WeaponTypes = ItemType.Hammer | ItemType.Sword;
                    character.Sprite = new SpriteObject(Game.GetInstance().Content.Load<Texture2D>("fighter"));
                    character.Sprite.AddAnimation("walking down", new SpriteAnimation { FrameCount = 8, Size = new Rectangle(0, 0, 65, 75), StartRow = 1, FrameRate = 8 });
                    character.Sprite.AddAnimation("standing down", new SpriteAnimation { FrameCount = 1, Size = new Rectangle(0, 0, 65, 75), StartRow = 1, FrameRate = 1 });
                    character.Sprite.AddAnimation("walking up", new SpriteAnimation { FrameCount = 8, Size = new Rectangle(0, 0, 65, 75), StartRow = 76, FrameRate = 8 });
                    character.Sprite.AddAnimation("standing up", new SpriteAnimation { FrameCount = 1, Size = new Rectangle(0, 0, 65, 75), StartRow = 76, FrameRate = 1 });
                    character.Sprite.AddAnimation("walking left", new SpriteAnimation { FrameCount = 8, Size = new Rectangle(0, 0, 65, 75), StartRow = 151, FrameRate = 8 });
                    character.Sprite.AddAnimation("standing left", new SpriteAnimation { FrameCount = 1, Size = new Rectangle(0, 0, 65, 75), StartRow = 151, FrameRate = 1 });
                    character.Sprite.AddAnimation("walking right", new SpriteAnimation { FrameCount = 8, Size = new Rectangle(0, 0, 65, 75), StartRow = 226, FrameRate = 8 });
                    character.Sprite.AddAnimation("standing right", new SpriteAnimation { FrameCount = 1, Size = new Rectangle(0, 0, 65, 75), StartRow = 226, FrameRate = 1 });
                    break;
            }

            return character;
        }
    }
}
